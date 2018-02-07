using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour {

    public Slider masterVSlider, sfxSlider;
    public Text masterText, sfxText;
	// Use this for initialization
	void Start () {
        DisplayValue();	
	}
    public void ShowText(Text t)
    {
        StartCoroutine(ShowTextFade(t));
    }

    IEnumerator ShowTextFade(Text t)
    {
        if (t == masterText)
            t.text = masterVSlider.value.ToString();

        else
            t.text = sfxSlider.value.ToString();
            yield return null;
    }
    public void SaveValue()
    {
        PlayerPrefs.SetFloat(AudioManager.masterVol, masterVSlider.value);
        PlayerPrefs.SetFloat(AudioManager.sfxVol, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void DisplayValue()
    {
        masterVSlider.value = PlayerPrefs.GetFloat(AudioManager.masterVol);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.sfxVol);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
