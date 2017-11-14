using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public int value;
    public float rotateSpeed;
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
	}

    void OnTriggerEnter()
    {
        SoundManager.instance.Collect(value, gameObject); //to call the value and gameobject so you can destroy the gameobject when collected

        AudioSource source = GetComponent<AudioSource>(); //to get the audio from audio source
        source.Play(); // Play sound 

    }
}
