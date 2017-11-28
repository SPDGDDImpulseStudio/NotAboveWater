using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventG", fileName = "EventG_ShootCrates", order = 7)]
public class EventG : EventsInterface {
    public override void CurrEvent()
    {
        throw new NotImplementedException();
    }
    public List<GameObject> crates_;
    public float timingItTakes = 5, sizeOfSphereCheck = 10;
    public bool timerBool = false;
    public override IEnumerator EnumEvent()
    {

        Collider[] crates = Physics.OverlapSphere(Player.Instance.transform.position, sizeOfSphereCheck);
        
         //crates_ = new List<GameObject>();
        
        
        for (int i  =0; i < crates.Length; i++)
        {
            if (crates[i].GetComponent<Interact_CompulCrates>())
                CratesManager.Instance.PopulateList(crates[i].GetComponent<Interact_CompulCrates>());
        }
        float timer = 0;

        while (CratesManager.Instance.cratesToDestroy.Count != 0)
        {
            if (timerBool)
            {
                if (timer > timingItTakes) break;
                timer += Time.deltaTime;
            }
            yield return null;

        }
        EndOfEvent();
    }
    
}
