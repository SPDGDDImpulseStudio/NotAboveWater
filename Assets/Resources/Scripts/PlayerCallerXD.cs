using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallerXD : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        if (!FindObjectOfType<Player>())
        {
            Debug.Log("False");
            Player.Instance.transform.position = new Vector3(-90.3f, 2.973083f, -417.26f);
            Player.Instance.transform.localEulerAngles = new Vector3(-18.945f, -2.145f, 0f);
            Player.Instance.RegisterSelf();
        }
        else
        {

            Debug.Log("True");
        }
        if (!FindObjectOfType<AudioManager>())
        {
            AudioManager.Instance.RegisterSelf();
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
