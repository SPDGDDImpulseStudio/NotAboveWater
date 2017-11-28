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
        //bool x = true;
        //CameraManager.Instance.LookAtWayp(thisPos, out x);
        //Debug.Log(x);
        //yield return new WaitUntil(() => x == false);

        //Debug.Log(x);
        Debug.Log("IN");
        while (thisPos.gameObject.activeInHierarchy)
        {
            Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(dir);

            CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 14 * Time.deltaTime);
            //Debug.Log(angle);
            Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
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
