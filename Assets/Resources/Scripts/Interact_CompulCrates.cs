using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_CompulCrates : Interact_Crates {

    public override void Interact()
    {
        base.Interact();

    }
    
    public void OnDestroy()
    {
        //if(Application.isPlaying)
        if(CratesManager.Instance != null)
        CratesManager.Instance.RemoveThisCrate(this);
    }

   
}
