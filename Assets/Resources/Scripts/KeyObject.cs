using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyObject : MonoBehaviour {

    public Collider col;
    // Use this for initialization
    void Start () {
		
	}

    public CirclePosUpdate circle;
    public void DeductCircleHealth()
    {
        if (!circle) return;
        circle.health--;
        if (circle.health < 1)
        {
            Debug.Log(circle.health);
            TurnOff();
        }
    }

    void TurnOff()
    {
        Time.timeScale = 1f;
        if (circle != null)
            circle.TurnOff();
        gameObject.SetActive(false);
        Player.Instance.GainScore(UnityEngine.Random.Range(95, 105));

    }
}
