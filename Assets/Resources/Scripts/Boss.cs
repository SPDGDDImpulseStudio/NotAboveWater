using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float bossCurrHealth, bossMaxHealth;
    public List<GameObject> protectorTentacles = new List<GameObject>();
    public List<Animator> animatorTentacles;
    public UnityEngine.UI.Slider bossSlider;
    public List<Tentacle> tentacles = new List<Tentacle>();
    float timer = 3f, timerNow = 0f;

    public List<AudioClip> sfxBoss = new List<AudioClip>();
    public AudioSource aSource;


    public int someoneAttacking = 0;

    void Start () {
        bossCurrHealth = bossMaxHealth;
        animatorTentacles = new List<Animator>();
        for (int i = 0; i< protectorTentacles.Count; i++)
        {
            Animator g = protectorTentacles[i].GetComponent<Animator>();
            animatorTentacles.Add(g);
        }
        tentacles = new List<Tentacle>(FindObjectsOfType<Tentacle>());
        
        if (!aSource) aSource = GetComponent<AudioSource>();
     
    }

    public void Init()
    {

        bossSlider = Player.Instance.bossHpSlider;
        bossSlider.gameObject.SetActive(true);

        StartCoroutine(HealthChecker());
        StartCoroutine(TriggerTen());
        StartCoroutine(BossHealthUpdate());
    }

    public void AttackingUpdate()
    {
        someoneAttacking++;
        InvulnerableTentacle(true);

        Debug.Log("someoneattacking " + someoneAttacking);
    }

    public void InvulnerableTentacle(bool x)
    {
        if ((someoneAttacking > 0 && x )|| (someoneAttacking == 0 && !x))
        {
            for (int i = 0; i < animatorTentacles.Count; i++)
            {
                animatorTentacles[i].SetBool("Up", x);
            }
            killable = !x;
        }
    }
    public void PopUpdate()
    {
        someoneAttacking--;

        InvulnerableTentacle(false);

        Debug.Log("someoneattacking " + someoneAttacking);
    }

    IEnumerator BossHealthUpdate()
    {
        int x = tentacles.Count - 1;
        yield return new WaitUntil(() => (bossCurrHealth / bossMaxHealth < 0.75));
        StopTentacle(x);
        x--;
        yield return new WaitUntil(() => (bossCurrHealth / bossMaxHealth < 0.50));
        StopTentacle(x);
        x--;
        yield return new WaitUntil(() => (bossCurrHealth / bossMaxHealth < 0.25));
        StopTentacle(x);
        x--;
        yield return new WaitUntil(() => (bossCurrHealth / bossMaxHealth < 0.10));
    }

    void StopTentacle(int index)
    {
        tentacles[index].uglyStop = true;
        tentacles.RemoveAt(index);
        //Stops 1 
        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].attackTimer -= 0.6f;
        }
        Debug.Log(bossCurrHealth / bossMaxHealth);
    }
    public int bossState = 0;
    public UnityEngine.UI.Slider slider;

    IEnumerator HealthChecker()
    {
        while(bossCurrHealth > 0)
        {
            bossSlider.value = bossCurrHealth / bossMaxHealth;
            yield return null;
        }
        BossDie();
    }
    bool killable = true;


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
    void BossDie()
    {
        //yield return new WaitUntil(()=> )
        for(int i = 0; i< tentacles.Count; i++)
        {
            tentacles[i].uglyStop = true;
        }
        bossSlider.gameObject.SetActive(false);
        
        SceneChanger.Instance.Fading(0);
    }

    public void OnHit()
    {

        if (someoneAttacking > 0) return;
        if (!aSource.isPlaying)
        {
            int x = (int)Random.Range(0, sfxBoss.Count);
            aSource.clip = sfxBoss[x];
            aSource.Play();
        }
        float rnd = Random.Range(50, 60);
        Player.Instance.GainScore(rnd);
        Stats.Instance.TrackStats(1, 1);

        bossCurrHealth -= Random.Range(10f, 20f);
    }

}
