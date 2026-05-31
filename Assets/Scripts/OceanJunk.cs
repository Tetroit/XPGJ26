using UnityEngine;

public class OceanJunk : MonoBehaviour
{
    public JunkManager junkManager;
    public WaveGen waveGen; 
    public MeshRenderer meshRenderer;
    private Vector3 startPos;
    [SerializeField]
    public Vector3 speed;
    public bool isSwimming = true;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isSwimming)
        {
            startPos += speed * Time.deltaTime;
            var waveLoc = waveGen.SampleGerstner(Time.time, new Vector2(startPos.x, startPos.z));
            transform.position = waveLoc.position;
            if (transform.position.z < -50)
            {
                junkManager.junks.Remove(this);
                Destroy(gameObject);
            }
        }
    }
    public void HighlightJunk()
    {
        meshRenderer.material.SetInt("_Highlighted", 1);
    }
    public void UnhighlightJunk()
    {
        meshRenderer.material.SetInt("_Highlighted", 0);
    }
}
