using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : PoolObject {
    
	void Start () {
        Init();
        rb = GetComponent<Rigidbody>();
        iniMass = rb.mass;
    }
    float iniMass;

    public Transform target;
    public CirclePosUpdate circle;
    float speed = 100f, rotSpeed = 5f;
    Rigidbody rb;
    void FixedUpdate()
    {
        if (!target) return;

        Vector2 dir = target.position - rb.position;
        dir.Normalize();

        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, rotSpeed);
        //float rotateAmt = Vector3.Cross(dir, transform.up).z;

        ////rb.angularVelocity = -rotateAmt * rotSpeed;
        //rb.velocity = transform.up * speed;
        
    }
    
    void OnTriggerEnter(Collider x)
    {
        //TurnOff();
        if (x.transform.GetComponent<Player>())
        {
            //Player.Instance.currHealth -= 15f;
            TurnOff();
            
        }
    }

    public override void Init()
    {
        gameObject.SetActive(true); 
    }
    public void Init_(CirclePosUpdate cir, Transform target_)
    {
        Init();
        target = target_;
        circle = cir;
        circle.Init_(this.gameObject);
        circle.bulletCheck = true;
        if (Time.timeScale == 1.0f) Time.timeScale = 0.6f;
    }

    public override void TurnOff()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        

    }
}
