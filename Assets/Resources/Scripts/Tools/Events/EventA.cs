using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventA", fileName = "EventA_", order = 1)]
public class EventA : EventsInterface{

    public Transform thisPos = null;
    public string[] toWhere;
    public float speed;
    int currInt = 0;
    public void OnEnable()
    {
    }

    public override void CurrEvent()
    {
        CameraManager.Instance.WalkTo(thisPos.transform.position, 5);
    }

    public override IEnumerator EnumEvent()
    {
        currInt = 0;
        //bool x = true;
        //CameraManager.Instance.LookAtWayp(thisPos, out x);
        //Debug.Log(x);
        //yield return new WaitUntil(() => x == false);

        thisPos = GameObject.Find(toWhere[currInt]).transform;
        CameraManager.Instance.target = thisPos;
        currInt++;
        Debug.Log(currInt + " " + toWhere.Length);
        while (currInt != toWhere.Length + 1)
        {
            if (thisPos.gameObject.activeInHierarchy) {
                Vector3 dir = (thisPos.position - CameraManager.Instance.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(dir);

                CameraManager.Instance.transform.localRotation = Quaternion.RotateTowards(CameraManager.Instance.transform.rotation, rotation, 14 * Time.deltaTime);
                //Debug.Log(angle);
                Debug.DrawRay(CameraManager.Instance.transform.position, thisPos.position - CameraManager.Instance.transform.position);
                //CameraManager.Instance.transform.position = Vector3.Lerp(CameraManager.Instance.transform.position, thisPos.position, speed * Time.deltaTime);
                //Debug.Log(Vector3.Distance(thisPos.position, CameraManager.Instance.transform.position));

            }
            else if(!thisPos.gameObject.activeInHierarchy)
            {
                if (currInt == toWhere.Length)  break;
                thisPos = GameObject.Find(toWhere[currInt]).transform;
                CameraManager.Instance.target = null;
                CameraManager.Instance.target = thisPos;
                currInt++;
                Debug.Log("thisPos: " + thisPos + " is now " + thisPos.gameObject.activeInHierarchy);

            }
            yield return null;
        }
        CameraManager.Instance.target = null;

        thisPos = null;
        EndOfEvent();
    }

    public void OnDestroy()
    {
        thisPos = null;
    }

}
