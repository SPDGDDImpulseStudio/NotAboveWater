using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishScript : MonoBehaviour, PoolObject {
    public float fishSpeed = 25f;   
	// Use this for initialization
	void Start () {
		
	}
	
 
	// Update is called once per frame
	void FixedUpdate () {
        this.transform.position = Vector3.MoveTowards(this.transform.position, Player.Instance.transform.position, fishSpeed* Time.deltaTime);
	}

    public void Init()
    {
        throw new NotImplementedException();
    }

    public void TurnOff()
    {
        throw new NotImplementedException();
    }
}
