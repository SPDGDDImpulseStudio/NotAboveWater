using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookDirection : MonoBehaviour {

    public Transform LookAt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnDrawGizmos()
    {
        if (LookAt)
        {
            Gizmos.DrawWireCube(LookAt.position, Vector3.one);
            Gizmos.DrawLine(this.transform.position, LookAt.position);
        }
    }
}
