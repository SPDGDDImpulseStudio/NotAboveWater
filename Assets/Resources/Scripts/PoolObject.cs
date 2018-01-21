using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolObject:MonoBehaviour {
    public virtual void Init()
    {

    }

    public virtual void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
