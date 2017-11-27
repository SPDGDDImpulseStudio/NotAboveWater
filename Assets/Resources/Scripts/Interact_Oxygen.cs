using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Oxygen : InteractableObj {
    public override void Interact()
    {
        Player.Instance.AddOxygen(oxygenCarry);
    }
    public float oxygenCarry;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
