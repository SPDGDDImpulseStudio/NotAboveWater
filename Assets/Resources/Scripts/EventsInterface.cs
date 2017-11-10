using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventsInterface : ScriptableObject {

    
    bool forcable;

    [HideInInspector]
    public bool runningOfEvent = false;

    public bool coroutineAvail = false;
    public abstract void CurrEvent();

    public virtual void Init()
    {
        GameManager.Instance.UpdateGame += CurrEvent;
        runningOfEvent = true;
    }

    public virtual void Out()
    {
        GameManager.Instance.UpdateGame -= CurrEvent;
    }

    public void CallEvent()
    {
        runningOfEvent = true;
        CurrEvent();
    }
    public virtual void Test(string x) { }

    public virtual void EndOfEvent()
    {
        runningOfEvent = false;
        Debug.Log("End Of Event");
    }

    public abstract IEnumerator EnumEvent();
}
