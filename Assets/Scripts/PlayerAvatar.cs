using Unity.VisualScripting;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField]
    JunkManager junkManager;
    private OceanJunk context;
    public float reachDist = 3f;
    void Update()
    {
        if (junkManager)
            ScanJunk();
    }
    
    void ScanJunk()
    {
        if (context != null)
        { 
            if (Vector3.Distance(context.transform.position, transform.position) > reachDist)
            {
                context.UnhighlightJunk();
                context = null;
            }
        }
        var inst = junkManager.GetClosest(transform.position, out float distance);
        if (context != null && inst != context)
        {
            context.UnhighlightJunk();
        }
        if (distance < reachDist)
        {
            context = inst;
            context.HighlightJunk();
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, reachDist);
        if (context != null)
            Gizmos.DrawLine(transform.position, context.transform.position);
    }
}
