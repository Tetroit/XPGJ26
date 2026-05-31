using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField]
    JunkManager junkManager;
    public OceanJunk context;
    public float reachDist = 3f;
    public bool useFurnace = false;
    public bool hasItem = false;
    void Update()
    {
        if (junkManager && !hasItem)
            ScanJunk();
        if (hasItem)
        {
            context.transform.position = Vector3.Lerp(context.transform.position, transform.position + transform.up * 2, Time.deltaTime * 3);
        }
    }
    private void OnEnable()
    {
        GameManager.instance.inputS.Player.Interact.started+=ProcessInteract;
    }
    private void OnDisable()
    {
        GameManager.instance.inputS.Player.Interact.started-=ProcessInteract;
    }
    public void StopGame()
    {
        if (context != null)
        {
            Destroy(context.gameObject);
        }
    }
    void Awake()
    {
        GameManager.instance.inputS = new InputS();
    }
    void ScanJunk()
    {
        if (context != null)
        { 
            if (Vector3.Distance(context.transform.position, transform.position) > reachDist)
            {
                context.UnhighlightJunk();
                context = null;
            }
        }
        var inst = junkManager.GetClosest(transform.position, out float distance);
        if (distance < reachDist)
        {
            if (context != null && inst != context)
            {
                context.UnhighlightJunk();
            }
            context = inst;
            context.HighlightJunk();
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, reachDist);
        if (context != null)
            Gizmos.DrawLine(transform.position, context.transform.position);
    }

    void ProcessInteract(InputAction.CallbackContext ctx)
    {
        if (!hasItem && context != null)
        {
            TakeObject();
        }
        else if (hasItem)
        {
            if (useFurnace)
            {
                BurnObject();
            }
            else
            {
                EatObject();
            }
        }
    }
    void TakeObject()
    {
        context.junkManager.junks.Remove(context);
        context.isSwimming = false;
        hasItem = true;
    }
    void BurnObject()
    {
        hasItem = false;
        var eff = context.GetComponent<JunkEffects>();
        eff.PutToOven();
        Destroy(context.gameObject);
    }
    void EatObject()
    {
        hasItem = false;
        var eff = context.GetComponent<JunkEffects>();
        eff.Apply();
        Destroy(context.gameObject);
    }
}
