using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObj : MonoBehaviour {

    public abstract void Interact();

}
