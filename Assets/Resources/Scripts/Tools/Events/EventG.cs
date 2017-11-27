using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventG", fileName = "EventG_", order = 7)]
public class EventG : EventsInterface {
    public override void CurrEvent()
    {
        throw new NotImplementedException();
    }

    public float timingItTakes = 5;
    public override IEnumerator EnumEvent()
    {

        Collider[] crates = Physics.OverlapSphere(Player.Instance.transform.position,10);
        
        List<GameObject> crates_ = new List<GameObject>();
        
        
        for (int i  =0; i < crates.Length; i++)
        {
            if (crates[i].GetComponent<Interact_CompulCrates>())
                crates_.Add(crates[i].gameObject);
        }
        float timer = 0;
        while(crates_.Count != 0)
        {
            timer += Time.deltaTime;
            if (timer > timingItTakes) break;

            yield return null;

        }
        EndOfEvent();
    }

    // Use this for initialization
    void Start () {
		
	}
	void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Player.Instance.transform.position, 10);
    }
   
	// Update is called once per frame
	void Update () {
		
	}
}
