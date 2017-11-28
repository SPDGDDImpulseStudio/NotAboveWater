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
    }

    public override void CurrEvent()
    {
        CameraManager.Instance.WalkTo(thisPos.transform.position, 5);
    }

    public override IEnumerator EnumEvent()
    {
        thisPos = GameObject.Find(toWhere).transform;
        CameraManager.Instance.target = thisPos;
        Debug.Log("IN");
        while (thisPos.gameObject.activeInHierarchy)
        {
            //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
            //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));
            yield return null;

        }
        CameraManager.Instance.target = null;
        EndOfEvent();
    }

    public void OnDestroy()
    {
        thisPos = null;
    }

}
