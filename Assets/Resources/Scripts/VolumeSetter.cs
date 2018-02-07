using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour {

    public Slider masterVSlider, sfxSlider;
    public Text masterText, sfxText;

    public void ShowText(Text t)
    {
        //called = false;
        if (t == masterText)
            StartCoroutine(ShowTextFade(t));
        else
            StartCoroutine(FadeNew(t));
        
    }
    #region TextFadeControl

    bool calledForMasterVSlider = false;
    IEnumerator ShowTextFade(Text t)
    {
        t.text = System.Math.Round(masterVSlider.value, 2).ToString();
        t.color = Color.white;

        if (!calledForMasterVSlider)
        {
            calledForMasterVSlider = true;
            Debug.Log(t + " 1");
            while (t.color != Color.clear)
            {
                t.color = Color.Lerp(t.color, Color.clear, Time.deltaTime);
                yield return null;
            }
            calledForMasterVSlider = false;
        }
    }

    bool callForSfxSlider = false;
    IEnumerator FadeNew(Text t)
    {
        t.text = System.Math.Round(sfxSlider.value, 2).ToString();
        t.color = Color.white;

        if (!callForSfxSlider)
        {
            callForSfxSlider = true;
            Debug.Log(t + " 2");
            while (t.color != Color.clear)
            {
                t.color = Color.Lerp(t.color, Color.clear, Time.deltaTime);
                yield return null;
            }
            callForSfxSlider = false;
        }
    }

    #endregion
    
    public void SaveValue()
    {
        PlayerPrefs.SetFloat(AudioManager.masterVol, masterVSlider.value);
        PlayerPrefs.SetFloat(AudioManager.sfxVol, sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume()
    {
        //Camera.main.GetComponent<AudioSource>().volume = AudioManager.Instance.masterVolumeAttr; //JR SAYS TEMP FIX BECUS UR BGM STILL PLAYS FROM MAIN CAMERA
        //AudioManager.Instance.audioSource.volume = AudioManager.Instance.masterVolumeAttr; //JR SAYS use master volume
        //masterText.text = AudioManager.Instance.masterVolumeAttr.ToString("F1");
        //AudioManager.Instance.audioSource2.volume = AudioManager.Instance.masterVolumeAttr; //JR SAYS use master volume
    }

    public void DisplayValue()  
    {
        masterVSlider.value = PlayerPrefs.GetFloat(AudioManager.masterVol);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.sfxVol);
        calledForMasterVSlider = false;
        callForSfxSlider = false;
        sfxText.color = Color.clear;
        masterText.color = Color.clear;
    }
}
