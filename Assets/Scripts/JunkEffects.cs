using DG.Tweening;
using UnityEngine;

public class JunkEffects : MonoBehaviour
{
    public enum JunkType
    {
        Food,
        Poop,
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
        if (junkType == JunkType.Maxwell)
        {
            GameManager.instance.AddPlayerSpeed(2);
            GameManager.instance.AddScore(5);
        }
        if (junkType == JunkType.Pearto)
        {
            GameManager.instance.DiePearto();
        }
        if (junkType == JunkType.Coal)
        {
            GameManager.instance.DieOOM();
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
            GameManager.instance.AddScore(2);
        }
        if (junkType == JunkType.Coal)
        {
            GameManager.instance.AddFuel(5);
        }
        if (junkType == JunkType.Maxwell)
        {
            GameManager.instance.DieMaxwell();
        }
        if (junkType == JunkType.Pearto)
        {
            GameManager.instance.AddSpeed(1);
            GameManager.instance.AddFuel(2);
        }
    }
}
