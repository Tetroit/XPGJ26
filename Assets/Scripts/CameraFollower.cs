using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 startOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startOffset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + startOffset, Time.deltaTime * 2);
    }
}
