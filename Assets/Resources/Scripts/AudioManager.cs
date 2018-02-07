using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class AudioManager : ISingleton<AudioManager>
{
    [Tooltip("Please put the audioclips in the same order of scene build index.")]
    public List<AudioClip> loopingAmbienceClips = new List<AudioClip>();
    public AudioSource audioSource, audioSource2, sfxAS;

    int currInt, rndInt;

    bool fadedOut, fadedIn;

    [Range(0.1f, 2f)]
    public float musicFadeSpd = 1f;

    public override void RegisterSelf()
    {
        base.RegisterSelf();
        Call();
    }
    void Call()
    {
        AudioSource[] allAS = FindObjectsOfType<AudioSource>();
        allASScene.Clear();
        allASScene = new List<AudioSource>(allAS);
        for (int i = 0; i < allASScene.Count; i++)
        {
            if (allASScene[i])
                allASScene[i].volume = PlayerPrefs.GetFloat(masterVol);
        }

        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex)
        {
            case 0: Instance.FadeFromSceneChanger(0, 1); break;
            //0 and 1 for menu 

            case 1: Instance.FadeFromSceneChanger(5, 4); break;
            //4 and 5 for boss fight

            case 2: Instance.FadeFromSceneChanger(0, 1); break;

        }
    }
    public List<AudioSource> allASScene;


    public const string masterVol = "MasterVol", sfxVol = "sfxVol";


    //The fn i call from scenechanger x when scene change
    // i want this argument to be filled with the scene.buildindex
    //
    public void PlaySecAS(int BGM)
    {
        audioSource2.clip = loopingAmbienceClips[BGM];
        audioSource2.Play();
    }
    public void FadeFromSceneChanger(int BGM, int BGM2 = 0)
    {
        Debug.Log("BGM = " + BGM + " BGM2 = " + BGM2);
        StartCoroutine(FadeToNext(BGM));
        StartCoroutine(FadeSecAS(BGM2));
    }
    IEnumerator FadeToNext(int nextBGM)
    {
        if (audioSource.isPlaying)
        {
            while (audioSource.volume > 0.2f)
            {
                audioSource.volume -= Time.deltaTime * musicFadeSpd;
                yield return null;
            }
        }
        audioSource.clip = loopingAmbienceClips[nextBGM];
        audioSource.Play();
        while (audioSource.volume < PlayerPrefs.GetFloat(masterVol))
        {
            audioSource.volume += Time.deltaTime * musicFadeSpd;
            yield return null;
        }
    }
    IEnumerator FadeSecAS(int BGM2)
    {
        if (audioSource2.isPlaying)
        {
            while (audioSource2.volume > 0.2f)
            {
                audioSource2.volume -= Time.deltaTime * musicFadeSpd;
                yield return null;
            }
        }
            PlaySecAS(BGM2);
        while (audioSource2.volume < PlayerPrefs.GetFloat(masterVol))
        {
            audioSource2.volume += Time.deltaTime * musicFadeSpd;
            yield return null;
        }
    }
}
