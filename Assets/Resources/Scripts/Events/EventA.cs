using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventA", fileName = "EventA_", order = 1)]
public class EventA : EventsInterface{

    public Transform thisPos;
    public string toWhere;
    public float speed;
    
    public void OnEnable()
    {
        thisPos = GameObject.Find(toWhere).transform;
    }

    public override void CurrEvent()
    {
        CameraManager.Instance.WalkTo(thisPos.transform.position, 5);
    }

    public override IEnumerator EnumEvent()
    {
        Debug.Log("IN");
        while ( thisPos.gameObject.activeInHierarchy)
        {
            CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);

            yield return null;

        }
        EndOfEvent();
    }

    public void OnDestroy()
    {
        thisPos = null;
    }

}
