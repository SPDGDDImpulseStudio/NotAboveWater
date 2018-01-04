using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioColliderSnapshot : MonoBehaviour {

    public AudioMixerSnapshot InsideColliderSnap;
    public AudioMixerSnapshot OutsideColliderSnap;
    public AudioSource inside, outside;
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        InsideColliderSnap.TransitionTo(0.5f);
        inside.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        OutsideColliderSnap.TransitionTo(0.5f);
    }


}
