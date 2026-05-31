using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FurnaceDriver : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField]
    private float minZ = -9.15f;
    [SerializeField]
    private float maxZ = 8.75f;
    [SerializeField]
    private bool maxLoc = false;
    private Rigidbody rb;
    public bool canPush = false;

    public float minDur = 5f;
    public float maxDur = 15f;
    
    public float furnaceDuration = 2.0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        transform.localPosition = new Vector3(startPos.x, startPos.y, maxZ);
    }
    public void StartGame()
    {
        StartCoroutine(MoveToOtherSide());
    }
    public void StopGame()
    {
        StopAllCoroutines();
    }
    IEnumerator MoveToOtherSide()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDur, maxDur));
            if (maxLoc)
                rb.DOMove(new Vector3(Random.Range(-9f, 9f), rb.position.y, maxZ), furnaceDuration).SetEase(Ease.InQuad);
            else
                rb.DOMove(new Vector3(Random.Range(-9f, 9f), rb.position.y, minZ), furnaceDuration).SetEase(Ease.InQuad);
            maxLoc = !maxLoc;
            Sequence seq = DOTween.Sequence()
                .AppendInterval(furnaceDuration/2)
                .AppendCallback(() => canPush = true)
                .AppendInterval(furnaceDuration/2)
                .AppendCallback(() => canPush = false);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (canPush)
            {
                Debug.Log("SLAMMM!");
                collision.gameObject.GetComponent<Rigidbody>().linearVelocity -= collision.contacts[0].normal * 100f;
            }
        }
    }
}
