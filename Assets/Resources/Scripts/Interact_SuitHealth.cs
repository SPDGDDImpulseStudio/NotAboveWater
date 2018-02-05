using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_SuitHealth : InteractableObj
{
    public AudioClip suitClip;
    public  void Start()
    {

        suitClip = Resources.Load<AudioClip>("Audio/NewSuitGet");
    }
    public override void Interact()
    {
        Player.Instance.currSuitHealth = Player.Instance.maxSuitHealth;
        AudioManager.Instance.sfxAS.PlayOneShot(suitClip);
    }

}
