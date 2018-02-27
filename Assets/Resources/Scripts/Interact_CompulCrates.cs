using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Interact_CompulCrates : Interact_Crates {

    public override void Interact()
    {
        base.Interact();
        gameObject.SetActive(false);

    }
    
    public void OnDestroy()
    {
        //if(Application.isPlaying)
        
    }
    public void OnMouseDown()
    {
        Destroy(gameObject);
    }
   
}
