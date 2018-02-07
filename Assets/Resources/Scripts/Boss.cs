using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float bossCurrHealth, bossMaxHealth;

    List<Tentacle> tentacles = new List<Tentacle>();
    float timer = 3f, timerNow = 0f;

    public List<AudioClip> sfxBoss = new List<AudioClip>();
    public AudioSource aSource;

    void Start () {
        bossCurrHealth = bossMaxHealth;
        tentacles = new List<Tentacle>(FindObjectsOfType<Tentacle>());
        if (!aSource)
            aSource = GetComponent<AudioSource>();
        StartCoroutine(HealthChecker());
        StartCoroutine(TriggerTen());
    }
    public UnityEngine.UI.Slider slider;
    IEnumerator HealthChecker()
    {
        while(bossCurrHealth > 0)
        {
            //slider.value = (bossCurrHealth / bossMaxHealth);

            yield return null;
        }
        StartCoroutine(BossDie());
    }

    IEnumerator TriggerTen()
    {
        while (true)
        {
            if (timerNow < timer) timerNow += Time.deltaTime;
            else
            {
                int b = (int)Random.Range(0, tentacles.Count);
                for (int i = 0; i < tentacles.Count; i++)
                {
                    if (i == b) tentacles[i].selectOne = true;
                    else
                        tentacles[i].selectOne = false;
                }
                timer = Random.Range(3f, 4f);
                timerNow = 0f;
            }
            yield return null;
        }
    }
    IEnumerator BossDie()
    {
        //yield return new WaitUntil(()=> )
        yield return null;
        Debug.Log("Boss DIe");
        SceneChanger.Instance.Fading(0);
    }

    public void OnHit()
    {
        if (!aSource.isPlaying)
        {
            int x = (int)Random.Range(0, sfxBoss.Count);
            aSource.clip = sfxBoss[x];
            aSource.Play();
        }
        bossCurrHealth -= Random.Range(15f, 30f);
    }

}
