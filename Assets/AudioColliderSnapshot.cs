using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioColliderSnapshot : MonoBehaviour {

    public AudioMixerSnapshot InsideColliderSnap;
    public AudioMixerSnapshot OutsideColliderSnap;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        InsideColliderSnap.TransitionTo(0.5f);
    }

    private void OnTriggerExit(Collider other)
    {
        OutsideColliderSnap.TransitionTo(0.5f);
    }


}
