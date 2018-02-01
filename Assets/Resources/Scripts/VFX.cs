using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : PoolObject {
    ParticleSystem pSystem;
    void Start()
    {
        if (!pSystem)
            pSystem = GetComponentInChildren<ParticleSystem>();

    }
    public override void Init()
    {
        this.gameObject.SetActive(true);
        if(!pSystem)
        pSystem = GetComponentInChildren<ParticleSystem>();


        pSystem.Stop();
        pSystem.Play();
    }

    public override void TurnOff()
    {
        //if (!pSystem)
        //    pSystem = GetComponentInChildren<ParticleSystem>();
        if(pSystem)
        pSystem.Stop();

        this.gameObject.SetActive(false);

    }

    void RepositionSelf()
    {

    }
}
