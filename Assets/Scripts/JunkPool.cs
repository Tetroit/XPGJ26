using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Junk Pool", menuName = "Junk Pool")]
public class JunkPool : ScriptableObject
{
    [System.Serializable]
    public struct JunkProb
    {
        public float prob;
        public OceanJunk junkType;
    }
    [SerializeField]
    List<JunkProb> src;
    private float total = -1;
    public void Bake()
    {
        float val = 0;
        for (int i = 0; i < src.Count; i++)
        {
            val += src[i].prob;
        }
        total = val;
    }
    public OceanJunk Pull()
    {
        if (total < 0)
        {
            Bake();
        }
        float rng = Random.Range(0, total);
        float val = 0;
        for (int i = 0; i < total; i++)
        {
            val += src[i].prob;
            if (val > rng)
            {
                return src[i].junkType;
            }
        }
        return src[src.Count - 1].junkType;
    }
}
