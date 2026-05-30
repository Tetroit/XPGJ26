using UnityEngine;

public class VisualClone : MonoBehaviour
{
    [SerializeField] protected Transform parent;
    [SerializeField] protected Transform visual;
    void Update()
    {
        visual.position = parent.TransformPoint(transform.position);
        visual.rotation = parent.rotation * transform.rotation;
    }
}
