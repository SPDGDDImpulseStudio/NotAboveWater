﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour, PoolObject {
    
	void Start () {
        Init();
        rb = GetComponent<Rigidbody>();
        iniMass = rb.mass;
    }
    float iniMass;

    public Transform target;
    public CirclePosUpdate circle;
    float speed = 100f, rotSpeed = 3.6f;
    Rigidbody rb;

    public void DeductCircleHealth()
    {
        if (circle)
        {
            circle.health--;
            Pop();
            if (circle.health < 1)
            {
                TurnOff();
            }
        }
    }

    void Pop()
    {
        Player.Instance.GainScore(UnityEngine.Random.Range(95, 105)) ;
    }
    void FixedUpdate()
    {
        if (!target) return;

        this.transform.position = Vector3.MoveTowards(this.transform.position, target.position + anyHow, rotSpeed);
        //if (Vector3.Distance(this.transform.localPosition, Player.Instance.transform.position) < 4.8f)
        //    Hit();
        //Debug.Log(Vector3.Distance(this.transform.localPosition, Player.Instance.transform.localPosition) + " " +
        // Vector3.Distance(this.transform.position, Player.Instance.transform.position) + " " +
        //    Vector3.Distance(this.transform.localPosition, Player.Instance.transform.position) + " " +         
        //    Vector3.Distance(this.transform.position, Player.Instance.transform.localPosition) //+ " " +

        


            //);
        //float rotateAmt = Vector3.Cross(dir, transform.up).z;
        ////rb.angularVelocity = -rotateAmt * rotSpeed;
        //rb.velocity = transform.up * speed;
    }
    /*
        To Check for player getting damaged by blobs

        Choice 1:
            OnTriggerEnter 
        
        Choice 2:
            Check for distance 


         
         
         
         */
    void OnTriggerEnter(Collider x)
    {
        //TurnOff();

        if (x.transform.GetComponentInChildren<Player>())
        {
            Debug.Log("HIT");
            Debug.Log(Vector3.Distance(this.transform.position, Player.Instance.transform.position));

            //Player.Instance.currHealth -= 15f;
            //TurnOff();
            Hit();
        }
    }

    Vector3 anyHow;
    public void Init()
    {
        gameObject.SetActive(true); 
    }
    public void Init_(CirclePosUpdate cir,  Transform target_, bool circleBool = false)
    {
        Init();
        target = target_;
        circle = cir;
        circle.Init_(this.gameObject, circleBool);
        //circle.bulletCheck = true;
        anyHow = transform.right * UnityEngine.Random.Range(1f, 4f);
        if (Time.timeScale == 1.0f) Time.timeScale = 0.6f;
    }
    void Hit()
    {
        Player.Instance.ShakeCam();
        Player.Instance.currHealth -= 15f;
        TurnOff();

    }
    public void TurnOff()
    {
        Time.timeScale = 1f;
        target = null;
        if(circle!= null)
        circle.TurnOff();
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        TurnOff();
    }
}
