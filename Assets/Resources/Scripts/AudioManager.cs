using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class AudioManager : ISingleton<AudioManager> {

    public List<AudioClip> loopingAmbienceClips = new List<AudioClip>();
    public AudioSource audioSource, audioSource2, sfxAS;

    int currInt, rndInt;

    bool fadedOut, fadedIn;

    void Start()
    {
        
    }

    //The fn i call from scenechanger x when scene change
    // i want this argument to be filled with the scene.buildindex
    //
    public void FadeFromSceneChanger(int BGM)
    {
        StartCoroutine(FadeToNext(BGM));
    }
    IEnumerator FadeToNext(int nextBGM)
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(FadeOutASOne());
            StartCoroutine(FadeInASTwo());
        }
        else
        {
            StartCoroutine(FadeOutASTwo());
            StartCoroutine(FadeInASOne());
        }
        yield return new WaitUntil(() => fadedIn && fadedOut);
        fadedIn = false;
        fadedOut = false;
    }

    IEnumerator FadeOutASOne()
    {
        while(audioSource.volume != 0)
        {
            audioSource.volume -= Time.deltaTime;
            yield return null;
        }
        fadedOut = true;
    }
    IEnumerator FadeInASOne()
    {
        while(audioSource.volume != 1)
        {
            audioSource.volume += Time.deltaTime;
            yield return null;
        }
        fadedIn = true;
    }

    IEnumerator FadeOutASTwo()
    {
        while (audioSource2.volume != 0)
        {
            audioSource2.volume -= Time.deltaTime;
            yield return null;
        }
        fadedOut = true;
    }
    IEnumerator FadeInASTwo()
    {
        while (audioSource2.volume != 1)
        {
            audioSource2.volume += Time.deltaTime;
            yield return null;
        }
        fadedIn = true;
    }

    IEnumerator FadeTo()
    {
        if (loopingAmbienceClips.Count == 0) yield break;
        currInt = Random.Range(0, loopingAmbienceClips.Count - 1);
        audioSource.clip = loopingAmbienceClips[currInt];
        audioSource.Play();
        float rndSec = Random.Range(4.0f, 8.0f);
        yield return new WaitForSeconds(rndSec);

        rndInt = Random.Range(0, loopingAmbienceClips.Count - 1);
        while (rndInt == currInt)
        {
            rndInt = Random.Range(0, loopingAmbienceClips.Count - 1);
            yield return null;
        }
        Debug.Log(currInt);
        audioSource2.clip = loopingAmbienceClips[rndInt];
        audioSource2.Play();
        //_audioSource.loop = true;
        while (audioSource.volume != 0)
        {
            audioSource.volume -= Time.deltaTime;
            //Debug.Log(audioSource.volume);
            yield return null;
        }
        rndSec = Random.Range(2.0f, 5.0f);
        Debug.Log(rndInt);
        yield return new WaitForSeconds(rndSec);
        StartCoroutine(FadeIn());
    }


    IEnumerator FadeIn()
    {
        currInt = Random.Range(0, loopingAmbienceClips.Count - 1);
        while (currInt == rndInt)
        {
            currInt = Random.Range(0, loopingAmbienceClips.Count - 1);
            yield return null;
        }
        audioSource.clip = loopingAmbienceClips[currInt];
        Debug.Log(currInt);

        audioSource.Play();
        while (audioSource.volume != 1)
        {
            audioSource.volume += Time.deltaTime;

            Debug.Log(audioSource.volume);
            yield return null;
        }
        float rndSec = Random.Range(4.0f, 8.0f);
        yield return new WaitForSeconds(rndSec);
        StartCoroutine(FadeTo());

    }
}
