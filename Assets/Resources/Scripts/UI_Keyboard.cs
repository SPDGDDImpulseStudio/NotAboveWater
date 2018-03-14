using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class UI_Keyboard : MonoBehaviour
{
    public InputField input;
    public Text _text;
    string originalText;

    public List<GameObject> letters = new List<GameObject>();
    public List<Text> allText = new List<Text>();
    public List<Button> allBtn = new List<Button>();

    bool lowerCased = false;

    public void ClickKey(string character)
    {
        //Debug.Log(character);
        if (input.text.Length >10)
        {
            StartCoroutine(ChangeText("Please keep your name 10 characters long"));
            return;
        }
        input.text += character;
    }

    public void UpperLowerCased()
    {
        RefreshKeys();
        lowerCased = !lowerCased;
    }

    void Awake()
    {
        originalText = _text.text;
        string Q = letters[0].GetComponentInChildren<Text>().text;
        lowerCased = (letters[0].GetComponentInChildren<Text>().text == Q.ToLower()) ? true : false;
        for (int i = 0; i < letters.Count; i++)
        {
            allText.Add(letters[i].GetComponentInChildren<Text>());
            allBtn.Add(letters[i].GetComponentInChildren<Button>());
        }
        RefreshKeys();
    }
    void RefreshKeys()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            string textInThere = allText[i].text;

            allText[i].text = lowerCased ? allText[i].text.ToUpper() : allText[i].text.ToLower();

            allBtn[i].name = lowerCased ? allBtn[i].name.ToUpper() : allBtn[i].name.ToLower();
            textInThere = allText[i].text;
            
            allBtn[i].onClick.RemoveAllListeners();

            allBtn[i].onClick.AddListener(
                () => {
                    ClickKey(textInThere);
                });
        }
    }


    void OnEnable()
    {
        
   
            input.text = "";
        
    }
    public void TypingName()    
    {
        if(input.text.Length > 10)
        {
            StartCoroutine(ChangeText("Please keep your name 10 characters long"));
            input.text = input.text.Substring(0, input.text.Length - 1);
        }
    }
    public void Backspace()
    {
        if (input.text.Length > 0)
        {
            input.text = input.text.Substring(0, input.text.Length - 1);
        }
    }
    public void Enter()
    {
        if (input.text == "")
        {
            StartCoroutine(ChangeText("Please dont leave your name empty"));
            return;
        }else if(input.text.Length < 3)
        {
            StartCoroutine(ChangeText("Please make sure your name contains at least 3 characters."));
            return;
        }


        //This is where i save + plus continue fadeOut

        SceneChanger.Instance.PostTypingName(input.text);
        this.gameObject.SetActive(false);
    }

    float duration = 3f;
    bool textChange = false;
    IEnumerator ChangeText(string txtToChange)
    {
        duration = 3f;
        _text.text = txtToChange;
        if (textChange) yield break;
        textChange = true;
        float currDur = 0f;
        
        while (true)
        {
            if (currDur < duration)duration -= Time.deltaTime;
            else break;
            yield return null;
        }
        _text.text = originalText;
        textChange = false;
    }

}