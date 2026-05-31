using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InputS inputS;
    public static GameManager instance;
    public EventDispatcher dispatcher;
    public float speed;
    public int score;
    public float fuel;
    public float fuelSpeed = 0.1f;
    public bool gameRunning = false;
    public float maxFuel = 10;

    [SerializeField]
    CanvasGroup tutorial;
    [SerializeField]
    CanvasGroup menu;


    [SerializeField]
    private GameObject ship;
    [SerializeField]
    private Camera SideCam;
    [SerializeField]
    private Camera MainCam;
    [SerializeField]
    private CanvasGroup peartoDeathNote;
    [SerializeField]
    JunkManager junkManagr;
    
    [SerializeField]
    PlayerControls controls;
    [SerializeField]
    GameObject BOOM;
    [SerializeField] private FurnaceDriver furnace;
    void Awake()
    {
        inputS = new InputS();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance.dispatcher = dispatcher;
            Destroy(gameObject);
        }
    }
    void Start()
    {
        inputS.Enable();
        inputS.Player.Disable();
        ResetGame();
    }
    private Coroutine fuelCoroutine;
    private Coroutine scoreCoroutine;
    public void StartGame()
    {
        gameRunning = true;
        inputS.Player.Enable();
        menu.interactable = false;
        menu.DOFade(0f, 0.25f); 
        dispatcher.OnGameStart?.Invoke();
        fuelCoroutine = StartCoroutine(SpeedUpFuel());
        scoreCoroutine = StartCoroutine(DistScore());
        Debug.Log("Game Started");
        inputS.Player.Move.performed += ctx =>
        {
            Debug.Log(ctx.ReadValue<Vector2>());
        };
    }
    public void ShowTutorial()
    {
        menu.interactable = false;
        tutorial.DOFade(1f, 0.25f).OnComplete(() => {
            tutorial.interactable = true;
            tutorial.blocksRaycasts = true;
        });
    }
    public void HideTutorial()
    {
        tutorial.interactable = false;
        tutorial.blocksRaycasts = false;
        tutorial.DOFade(0f, 0.25f).OnComplete(() => {
            menu.interactable = true;
        });  
    }
    public void StopGame()
    {
        junkManagr.ClearJunks();
        gameRunning = false;
        inputS.Player.Disable();
        dispatcher.OnGameStop?.Invoke();
        StopCoroutine(fuelCoroutine);
        StopCoroutine(scoreCoroutine);
        menu.DOFade(1f, 0.25f).OnComplete(() => {
            menu.interactable = true;
        });
        Debug.Log("Game Stopped");
        ResetGame();
    }
    IEnumerator DistScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(50f / speed);
            score++;
            dispatcher.OnScoreChange?.Invoke(score);
        }
    }
    public void ResetGame()
    {
        fuel = maxFuel;
        fuelSpeed = 0.1f;
        score = 0;
        speed = 5f;
        furnace.furnaceDuration = 2.0f;
        controls.movementSpeed = 5f;
        dispatcher.OnSpeedChange?.Invoke(speed);
        dispatcher.OnScoreChange?.Invoke(score);
        dispatcher.OnFuelChange?.Invoke(fuel);
    }
    IEnumerator SpeedUpFuel()
    {
        fuelSpeed += 0.005f;
        yield return new WaitForSeconds(1);
    }
    void Update()
    {
        if (gameRunning)
        {
            fuel -= Time.deltaTime * fuelSpeed;
            dispatcher.OnFuelChange?.Invoke(fuel);
        }
        if (fuel <= 0)
        {
            DieOOM();
        }
    }
    public void AddScore(int score)
    {
        this.score += score;
        dispatcher.OnScoreChange?.Invoke(this.score);
    }
    public void SetScore(int score)
    {
        this.score = score;
        dispatcher.OnScoreChange?.Invoke(this.score);
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
        dispatcher.OnSpeedChange?.Invoke(speed);
    }
    public void AddFuel(int val)
    {
        fuel += val;
        fuel = Mathf.Clamp(fuel, 0, maxFuel);
    }
    public void DieMaxwell()
    {
        Debug.Log("Died Maxwell");
        StopGame();
        ResetGame();
        var seq = DOTween.Sequence()
            .AppendCallback(() => {
                MainCam.enabled = false;
                SideCam.enabled = true;
            })
            .AppendInterval(0.5f)
            .AppendCallback(() => {
                BOOM.SetActive(true);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() => {
                MainCam.enabled = true;
                SideCam.enabled = false;
                BOOM.SetActive(false);
            });
    }
    public void DieOOM()
    {
        Debug.Log("Died OOM");
        StopGame();
        ResetGame();
        ship.GetComponent<WaveDriver>().enabled = false;
        var seq = DOTween.Sequence().AppendCallback(() => {
                MainCam.enabled = false;
                SideCam.enabled = true;
            })
            .AppendInterval(0.5f)
            .Append(ship.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.7f))
            .AppendInterval(0.3f)
            .Append(ship.transform.DOMove(new Vector3(0, -50, 0), 1f))
            .AppendCallback(() => {
                MainCam.enabled = true;
                SideCam.enabled = false;
                ship.GetComponent<WaveDriver>().enabled = true;
            });

    }

    public void DiePearto()
    {
        Debug.Log("Died OOM");
        StopGame();
        ResetGame();
        ship.GetComponent<WaveDriver>().enabled = false;
        var seq = DOTween.Sequence()
            .Append(peartoDeathNote.DOFade(1, 0.5f))
            .AppendInterval(1f)
            .Append(peartoDeathNote.DOFade(0, 0.5f));
    }
    public void AddSpeed(int i)
    {
        SetSpeed(speed + i);
    }
    public void AddPlayerSpeed(int i)
    {
        controls.movementSpeed += i;
    }

    public void AddFurnaceSpeed()
    {
        furnace.furnaceDuration *= 0.92f;
    }
    public void AddFurnaceFreq()
    {
        furnace.minDur *= 0.92f;
        furnace.maxDur *= 0.92f;
    }
}
