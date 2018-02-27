using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : KeyObject {

    public void Start()
    {
        col = this.GetComponent<Collider>();
    }
    public void Init()
    {

    }
  
   
    public void OnTriggerEnter(Collider x)
    {
        Debug.Log(x.name);
    }

    public void OnCollisionEnter(Collision x)
    {
        if (x.transform.GetComponent<Player>())
        {
            Player.Instance.PlayerDeath();
        }
    }
}
