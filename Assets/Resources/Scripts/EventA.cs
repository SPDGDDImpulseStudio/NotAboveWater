using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventA", fileName = "EventA_", order = 1)]
public class EventA : EventsInterface{

    public Transform thisPos;
    public string toWhere;

    public void OnEnable()
    {
        thisPos = GameObject.Find(toWhere).transform;
    }

    public override void CurrEvent()
    {
        CameraManager.Instance.WalkTo(thisPos.transform.position, 5);
    }
    

	
}
