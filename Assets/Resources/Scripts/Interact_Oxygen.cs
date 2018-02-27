using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Oxygen : InteractableObj {
    public override void Interact()
    {
        Player.Instance.GainScore(oxygenCarry);
        Player.Instance.AddOxygen(oxygenCarry);
        Destroy(this.gameObject);
    }
    public float oxygenCarry;
    // Use this for initialization

}
