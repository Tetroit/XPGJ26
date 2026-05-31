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

    public Button startB;
    public Button tutorialB;
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
    private CanvasGroup maxWellDeathNote;
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
    public void StartGame()
    {
        inputS.Player.Enable();
        menu.interactable = false;
        menu.DOFade(0f, 0.25f); 
        dispatcher.OnGameStart?.Invoke();
        StartCoroutine(SpeedUpFuel());
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
        });
    }
    public void HideTutorial()
    {
        tutorial.interactable = false;
        tutorial.DOFade(0f, 0.25f).OnComplete(() => {
            menu.interactable = true;
        });  
    }
    public void StopGame()
    {
        inputS.Player.Disable();
        dispatcher.OnGameStop?.Invoke();
        StopCoroutine(SpeedUpFuel());
        tutorial.DOFade(1f, 0.25f).OnComplete(() => {
            tutorial.interactable = true;
        });
        Debug.Log("Game Stopped");
    }
    public void ResetGame()
    {
        fuel = maxFuel;
        fuelSpeed = 0.1f;
        score = 0;
        speed = 5;
        dispatcher.OnSpeedChange?.Invoke(speed);
        dispatcher.OnScoreChange?.Invoke(score);
        dispatcher.OnFuelChange?.Invoke(fuel);
    }
    IEnumerator SpeedUpFuel()
    {
        fuelSpeed += 0.002f;
        yield return new WaitForSeconds(1);
    }
    void Update()
    {
        fuel -= Time.deltaTime * fuelSpeed;
        dispatcher.OnFuelChange?.Invoke(fuel);
    }
    public void AddScore(int score)
    {
        this.score += score;
        dispatcher.OnScoreChange?.Invoke(score);
    }
    public void SetScore(int score)
    {
        this.score = score;
        dispatcher.OnScoreChange?.Invoke(score);
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
        dispatcher.OnSpeedChange?.Invoke(speed);
    }

    public void SpawnMaxwell()
    {
        
    }
    public void AddFuel(int val)
    {
        fuel += val;
        fuel = Mathf.Clamp(fuel, 0, maxFuel);
    }
    public void DieMaxwell()
    {
        
    }
    public void DieOOM()
    {
        var seq = DOTween.Sequence().AppendCallback(() => {
            MainCam.enabled = false;
            SideCam.enabled = false;
        }).AppendCallback(0.5).;
        
}
}
