using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource audioSource, _audioSource;

    // Use this for initialization
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeTo());
        //audioSource.clip = audioClips[1];

    }

    IEnumerator FadeTo()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
        yield return new WaitForSeconds(3f);
        _audioSource.clip = audioClips[1];
        _audioSource.Play();
        _audioSource.loop = true;
        while (audioSource.volume != 0)
        {
            audioSource.volume -= Time.deltaTime;
            //Debug.Log(audioSource.volume);
            yield return null;
        }
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
