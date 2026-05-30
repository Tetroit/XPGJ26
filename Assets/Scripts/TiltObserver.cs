using System;
using UnityEngine;

public class TiltObserver : MonoBehaviour
{
    private Rigidbody rb;
    private Quaternion tilt;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 gravityCorrection = Quaternion.Inverse(tilt) * Physics.gravity;
        rb.AddForce(gravityCorrection * Time.fixedDeltaTime, ForceMode.Acceleration);
    }
    public void ObserceTilt(Quaternion tilt)
    {   
        this.tilt = tilt;
    }
}
