using UnityEngine;

public class JunkEffects : MonoBehaviour
{
    public enum JunkType
    {
        Food,
        Poop,
        GoldenPoop,
        Pearto,
        Maxwell,
        Coal,
        
    }
    [SerializeField]
    private JunkType junkType;
    void Apply()
    {
        if (junkType == JunkType.Food)
        {
            GameManager.instance.AddScore(Random.Range(1, 4));
        }
        else if (junkType == JunkType.Poop)
        {
            GameManager.instance.AddScore(-1);
        }
    }
}
