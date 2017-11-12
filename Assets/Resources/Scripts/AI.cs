using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour {

    public float currHealth, maxHealth, speed;

    public Transform[] waypoints;

    int currInt = 0;
    public Transform currWaypoint;
    NavMeshAgent nav;
	// Use this for initialization
	void Start () {
        currHealth = maxHealth;	
	}
	
    public void UpdateWaypoint()
    {
        currWaypoint = waypoints[currInt];
        nav.destination = currWaypoint.position;
        currInt++;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
