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
        transform.rotation = Quaternion.Euler(0f, 0f, 10 * Mathf.Cos(Time.time * Mathf.Lerp(1.3f, 2.1f, Mathf.Cos(Time.time))));
        transform.position = startPos + new Vector3(0, Mathf.Sin(Time.time) * 1.2f, 0);
        onTilt?.Invoke(transform.rotation);
    }
}
