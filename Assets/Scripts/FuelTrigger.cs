using UnityEngine;

public class FuelTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject indicator;
    void Start()
    {
        indicator.SetActive(false);
    }
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            indicator.SetActive(true);
            other.GetComponent<PlayerAvatar>().useFurnace = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            indicator.SetActive(false);
            other.GetComponent<PlayerAvatar>().useFurnace = false;
        }
    }
}
