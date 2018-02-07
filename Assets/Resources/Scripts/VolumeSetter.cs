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
        called = false;
        StartCoroutine(ShowTextFade(t));
        
    }
    static bool called = false;
    IEnumerator ShowTextFade(Text t)
    {
        called = true;
        if (t == masterText)
            t.text = masterVSlider.value.ToString();

        else
            t.text = sfxSlider.value.ToString();
        t.color = Color.white;

        while (t.color != Color.clear)
        {
            if (!called) yield break;
            t.color = Color.Lerp(t.color, Color.clear, 3f);
            yield return null;
        }

        called = false;
    }
    public void SaveValue()
    {
        AudioManager.Instance.masterVolumeAttr = masterVSlider.value;
        AudioManager.Instance.sfxAttr = sfxSlider.value;
        PlayerPrefs.SetFloat(AudioManager.masterVol, masterVSlider.value);//JR SAYS U maybe want to set the value of the audimanager then save only when set finish
        PlayerPrefs.SetFloat(AudioManager.sfxVol, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume()
    {
        Camera.main.GetComponent<AudioSource>().volume = AudioManager.Instance.masterVolumeAttr; //JR SAYS TEMP FIX BECUS UR BGM STILL PLAYS FROM MAIN CAMERA
        AudioManager.Instance.audioSource.volume = AudioManager.Instance.masterVolumeAttr; //JR SAYS use master volume
        masterText.text = AudioManager.Instance.masterVolumeAttr.ToString("F1");
        //AudioManager.Instance.audioSource2.volume = AudioManager.Instance.masterVolumeAttr; //JR SAYS use master volume
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
