using System;
using UnityEngine;

public class TiltObserver : MonoBehaviour
{
    private PlayerControls playerControls;
    private Rigidbody rb;
    private Quaternion tilt;
    void Awake()
    {
        playerControls = GetComponent<PlayerControls>();
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 gravityCorrection = Quaternion.Inverse(tilt) * Physics.gravity;
        playerControls.tilt = new Vector2(gravityCorrection.x, gravityCorrection.z);
    }
    public void ObserceTilt(Quaternion tilt)
    {
        this.tilt = tilt;
    }
}
