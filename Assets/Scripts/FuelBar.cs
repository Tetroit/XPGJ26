using UnityEngine;

public class FuelBar : MonoBehaviour
{
    [SerializeField]
    private RectTransform innerBar;
    private RectTransform outerBar;
    private float maxFuel = 10;

    void Awake()
    {
        outerBar = GetComponent<RectTransform>();
    }
    public void UpdateFuelDisplay(float fuel)
    {
        float fraction = fuel / maxFuel;
        innerBar.sizeDelta  = new Vector2(innerBar.sizeDelta.x, outerBar.sizeDelta.y * fraction);
        
    }
}
