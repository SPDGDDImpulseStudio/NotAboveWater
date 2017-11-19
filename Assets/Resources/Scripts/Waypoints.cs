using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {

    Collider col;
	// Use this for initialization
	void Start () {
        col = this.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    void OnTriggerEnter(Collider x) {

        if(x.transform.GetComponent<CameraManager>())
        {
            this.gameObject.SetActive(false);
            //Debug.Log("off");
        }

    }
}
