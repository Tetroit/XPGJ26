using UnityEngine;
using UnityEngine.Events;

public class WaveDriver : MonoBehaviour
{
    public Vector3 startPos;
    public UnityEvent<Quaternion> onTilt;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 5 * Mathf.Cos(Time.time * 1.3f));
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time) * 0.5f, 0);
        onTilt?.Invoke(transform.rotation);
    }
}
