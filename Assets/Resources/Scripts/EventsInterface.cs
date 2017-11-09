using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventsInterface : ScriptableObject {

    
    bool forcable;

    [HideInInspector]
    public bool runningOfEvent = false;

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
}
