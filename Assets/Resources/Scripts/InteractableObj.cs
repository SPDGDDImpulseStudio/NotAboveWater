using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(Collider))]
public abstract class InteractableObj : MonoBehaviour {

    public abstract void Interact();

    public Action Interaction;
    

}
