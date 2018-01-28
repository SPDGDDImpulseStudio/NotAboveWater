using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : PoolObject {
    
	void Start () {
        Init();
	}
    float iniMass;

    Transform target;
    float speed = 100f, rotSpeed = 5f;
    Rigidbody rb;

    void FixedUpdate()
    {
        if (!target) return;

        Vector2 dir = target.position - rb.position;
        dir.Normalize();

        float rotateAmt = Vector3.Cross(dir, transform.up).z;

        //rb.angularVelocity = -rotateAmt * rotSpeed;
        rb.velocity = transform.up * speed;
        
    }

    void OnTriggerEnter(Collider x)
    {
        //TurnOff();
    }

    public override void Init()
    {
        rb = GetComponent<Rigidbody>();
        iniMass = rb.mass;
    }

    public override void TurnOff()
    {
        gameObject.SetActive(false);

    }
}
