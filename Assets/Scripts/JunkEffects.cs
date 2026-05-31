using DG.Tweening;
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
    public void Apply()
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
    public void PutToOven()
    {
        if (junkType == JunkType.Food)
        {
            GameManager.instance.AddFuel(1);
        }
        if (junkType == JunkType.Poop)
        {
            GameManager.instance.AddFuel(-1);
        }
        if (junkType == JunkType.Coal)
        {
            GameManager.instance.AddFuel(1);
        }
        if (junkType == JunkType.Maxwell)
        {
            GameManager.instance.DieMaxwell();
        }
    }
}
