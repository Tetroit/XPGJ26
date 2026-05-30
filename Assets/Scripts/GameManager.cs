using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public EventDispatcher dispatcher;
    public float speed;
    public int score;
    public float fuel;

    void Awake()
    {
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
}
