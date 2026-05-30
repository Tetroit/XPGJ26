using UnityEngine;
using UnityEngine.Events;

public class EventDispatcher : MonoBehaviour
{
    public UnityEvent<float> OnSpeedChange;
    public UnityEvent<int> OnScoreChange;
}
