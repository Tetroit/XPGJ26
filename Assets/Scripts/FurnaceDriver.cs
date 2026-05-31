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
            yield return new WaitForSeconds(Random.Range(4, 15));
            if (maxLoc)
                rb.DOMoveZ(maxZ, 1f).SetEase(Ease.InQuad);
            else
                rb.DOMoveZ(minZ, 1f).SetEase(Ease.InQuad);
            maxLoc = !maxLoc;
            Sequence seq = DOTween.Sequence()
                .AppendInterval(0.5f)
                .AppendCallback(() => canPush = true)
                .AppendInterval(0.5f)
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
