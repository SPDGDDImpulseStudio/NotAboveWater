using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance = null;
    public GameObject scoreTextObject;
    int score;
    Text scoreText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        scoreText = scoreTextObject.GetComponent<Text>(); //call the score text so you can edit it automatically when you collect something
        scoreText.text = "Score:" + score.ToString(); //to put the score with string so it can update automatically when you collect something
    }

    public void Collect(int passedValue, GameObject passedObject)
    {
        passedObject.GetComponent<Renderer>().enabled = false; //destroy gameobject renderer once is collected
        passedObject.GetComponent<Collider>().enabled = false; //destroy gameobject collider once is collected
        Destroy(passedObject, 1.0f); //destroy gameobject once is collected
        score = score + passedValue; //update score value
        scoreText.text = "Score:" + score.ToString(); //update our score UI

    }
}