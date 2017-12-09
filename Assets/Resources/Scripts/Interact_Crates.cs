using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Crates : InteractableObj {
    public int currHealth, maxHealth = 2 ;

    public override void Interact()
    {
        currHealth--;
        if (currHealth == 0) DestroySelf();
    }

    // Use this for initialization
    void Start () {
        currHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void DestroySelf()
    {

        if (CratesManager.Instance != null)
            CratesManager.Instance.RemoveThisCrate(this);
        Destroy(this.gameObject);
    }
}
