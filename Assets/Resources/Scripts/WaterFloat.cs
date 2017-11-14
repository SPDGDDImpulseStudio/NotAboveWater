using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFloat : MonoBehaviour
{
    public float waterSurfaceLevel = 7;
    public float floatingHeight = 2;
    public float bouncingDamp = 0.05f;
    public Vector3 buoyancyCentreOffset;

    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
        forceFactor = 1f - ((actionPoint.y - waterSurfaceLevel) / floatingHeight);

        if (forceFactor > 0f)
        {
            upLift = -Physics.gravity * (forceFactor - rb.velocity.y * bouncingDamp);
            rb.AddForceAtPosition(upLift, actionPoint);
        }
    }
}
