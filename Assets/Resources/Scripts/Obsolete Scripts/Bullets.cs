using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour {

    public float damage;
	// Use this for initialization
	void Start () {
        damage = Player.Instance.bulletDamage;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider x)
    {
        if (x.GetComponent<AI>())
        {
            x.GetComponent<AI>().currHealth -= damage;
            Stats.Instance.roundsHit++;
        }
    }
}
