using System.Collections.Generic;
using UnityEngine;

public class JunkManager : MonoBehaviour
{
    public List<OceanJunk> junks;
    public OceanJunk GetClosest(Vector3 pos, out float distance)
    {
        OceanJunk closest = null;
        distance = float.MaxValue;
        foreach (var junk in junks)
        {
            float itemDist = Vector3.Distance(junk.transform.position, pos);
            if (itemDist <= distance)
            {
                closest = junk;
                distance = itemDist;
            }
        }
        return closest;
    }
    public void ClearJunks()
    {
        foreach (var junk in junks)
        {
            Destroy(junk.gameObject);
        }
        junks.Clear();
    }
}
