using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Tools/EventC", fileName = "EventC_", order =3)]

public class EventC : EventsInterface
{

    public string toType;
    public InputField inputField;
    Canvas canvas;
    public override void CurrEvent()
    {
        throw new NotImplementedException();
    }

    public override IEnumerator EnumEvent()
    {
        while(inputField.text != toType)
        {
            inputField = canvas.GetComponentInChildren<InputField>();
            Debug.Log("Text is now =" + inputField.text);
            yield return new WaitForSeconds(0.1f);
        }
        //yield return new WaitUntil(() => inputField.text == toType);
        EndOfEvent();
    }

    // Use this for initialization
    void OnEnable()
    {
        //inputField = FindObjectOfType<Canvas>().GetComponentInChildren<InputField>();
        canvas = FindObjectOfType<Canvas>();
        inputField = canvas.GetComponentInChildren<InputField>();
    }
    
    void OnDisable()
    {
        inputField = null;
    }

}
