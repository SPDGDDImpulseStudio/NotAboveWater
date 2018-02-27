using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour {

    public Slider masterVSlider, sfxSlider, bgmSlider;
    public Text masterText, sfxText , bgmText;

    public void ShowText(Text t)
    {
        //called = false;
        if (t == masterText)
            StartCoroutine(ShowTextFade(t));
        else if (t == sfxText)
            StartCoroutine(FadeNew(t));
        else if (t == bgmText)
            StartCoroutine(FadeNew_(t));

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
            while (t.color != Color.clear)
            {
                t.color = Color.Lerp(t.color, Color.clear, Time.deltaTime);
                yield return null;
            }
            callForSfxSlider = false;
        }
    }

    bool callForBGMSlider = false;
    IEnumerator FadeNew_(Text t)
    {
        t.text = System.Math.Round(bgmSlider.value, 2).ToString();
        t.color = Color.white;

        if (!callForBGMSlider)
        {
            callForBGMSlider = true;
            while (t.color != Color.clear)
            {
                t.color = Color.Lerp(t.color, Color.clear, Time.deltaTime);
                yield return null;
            }
            callForBGMSlider = false;
        }
    }

    #endregion

    public void SaveValue()
    {
        PlayerPrefs.SetFloat(AudioManager.masterVol, masterVSlider.value);
        PlayerPrefs.SetFloat(AudioManager.sfxVol, sfxSlider.value);
        PlayerPrefs.SetFloat(AudioManager.bgmVol, bgmSlider.value);


        PlayerPrefs.Save();
        AudioManager.Instance.ChangeVolumeOfAllAS();
        AudioManager.Instance.ChangeBGMVolume(bgmSlider.value * masterVSlider.value);
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
        bgmSlider.value = PlayerPrefs.GetFloat(AudioManager.bgmVol);
        calledForMasterVSlider = false;
        callForSfxSlider = false;
        callForBGMSlider = false;
        sfxText.color = Color.clear;
        masterText.color = Color.clear;
        bgmText.color = Color.clear;
    }
}
