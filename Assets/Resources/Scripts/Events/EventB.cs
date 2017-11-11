using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/EventB", fileName = "EventB_", order = 2)]
public class EventB : EventsInterface {

    public float duration;

    public override void CurrEvent()
    {
        CameraManager.Instance.Wait(5f);
    }
    public override IEnumerator EnumEvent()
    {
        Debug.Log("IN");
        yield return new WaitForSecondsRealtime(duration);
        EndOfEvent();
        
    }
}
