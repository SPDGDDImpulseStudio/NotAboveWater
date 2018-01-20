using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        iniMass = rb.mass;
        Debug.Log("HEY");
	}
    float iniMass;
	// Update is called once per frame
	void Update () {
		
	}
    Transform target;
    float speed = 200f, rotSpeed = 5f;
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
        PoolManager.Instance.EnqBullet(this);
    }
}
