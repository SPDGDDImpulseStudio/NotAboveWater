using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public List<AudioClip> loopingAmbienceClips = new List<AudioClip>();
    public AudioSource audioSource, _audioSource, backGroundAudioSource;

    int currInt, rndInt;
    // Use this for initialization
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeTo());
        //audioSource.clip = audioClips[1];

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
        _audioSource.clip = loopingAmbienceClips[rndInt];
        _audioSource.Play();
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

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance == null)
                {
                    GameObject newGO = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/AudioManager"));
                    _instance = newGO.GetComponent<AudioManager>();
                }
                if (_instance == null)
                    Debug.LogError("STOP");
                DontDestroyOnLoad(_instance.gameObject);
            }
            if (!_instance.gameObject.activeSelf) _instance.gameObject.SetActive(true);
            return _instance;               
        }
    }

    static AudioManager _instance;
    
}
