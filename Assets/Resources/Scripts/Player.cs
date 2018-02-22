﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

[RequireComponent(typeof(AudioSource))]
public class Player : ISingleton<Player> {

    #region Attributes
    
    [HideInInspector]
    public Vector3 pointHit;
    GameObject targetHit;

    [Header("[None of this should be empty]")]
    public AudioSource gunASource;
    public AudioSource playerASource;
    [Header("[VFX]")]
    public GameObject VFX_HitShark;
    public GameObject VFX_BulletMark;
    public GameObject VFX_BulletSpark;
    public Image SlimeTexture;
    [Header("[AudioClips]")]
    public AudioClip gunFire;
    public AudioClip reloadingClip, emptyGunFire;
    public List< AudioClip> characterDamagedClips = new List<AudioClip>(); 

    [Header("[Various UI]")]
    public GameObject ammoCounterBar;
    public Slider compassSlider, oxygenBar;
    public GameObject playerCanvas, titleCanvas, leaderboardUI;

    public PlayableDirector currentPD;
    public Image spr_OxygenBar;
    public Image redImage;
    public Text reloadText, oxygenText, comboText;

    
    List<Image> bullets = new List<Image>();
    List<heartrateScript> heartRates = new List<heartrateScript>();
    [Header("[Player's Attributes]")]

    [Header("[Things to Set]")]
    public float maxHealth;
    [Tooltip("Oxygen drops every frame, beware to set properly")]
    public float maxOxygen,
        oxyDrop, 
        reloadTime = 3, bulletDamage, shootEvery = 1;
    int maxBullet = 30;

    [Header("Dont+Touch+For+Debug+Purpose")]
    public int currBullet;
    public float currHealth, currOxygen, shootTimerNow;
    public GameObject parentCam;
    public Cinemachine.CinemachineBrain CB;
    public GameObject PlayableDirectorsParent;
    PlayableDirector[] playables;
    #endregion
    public void PlayerTurnOnTitleOff()
    {
        titleCanvas.SetActive(false);
        transform.localPosition = new Vector3(0, 0, 0);
        currentHighestComboCount = 0;
        Screen.SetResolution((int)originScreen.x, (int)originScreen.y, Screen.fullScreen);

        PlayableDirectorsParent = GameObject.Find("PlaybleDirectors");
        Debug.Log(PlayableDirectorsParent);

        playables = FindObjectsOfType<PlayableDirector>();

        StartCoroutine(PlayerHax());
        StartCoroutine(DisgustingShit());
        Debug.Log(originScreen);
    }

    void SwitchingTimeline(int x)
    {

        for (int i = 0; i < playables.Length; i++)
        {
            if (playables[i].gameObject.name == "Gameplay_0" + x + "_Timeline")
            {
                currentPD = playables[i];
                break;
            }
        }
        
        currentPD.Play();
    }
    float duration;

    
    IEnumerator DisgustingShit()
    {
        SwitchingTimeline(0);
        duration = (float)currentPD.duration;

        yield return new WaitUntil(() => currentPD.time + 2 > duration);
        yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(1);
        duration = (float)currentPD.duration;

        yield return new WaitUntil(() => currentPD.time + 2 > duration);
        yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(2);
        duration = (float)currentPD.duration;

        yield return new WaitUntil(() => currentPD.time + 2 > duration);
        yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(3);
        duration = (float)currentPD.duration;

        yield return new WaitUntil(() => currentPD.time + 2 > duration);
        yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(4);
        duration = (float)currentPD.duration;

        yield return new WaitUntil(() => currentPD.time + 2 > duration);
        yield return new WaitUntil(() => Input.anyKeyDown);
        //SwitchingTimeline(1);

    }

    void Start () {
        gunASource = GetComponent<AudioSource>();
        CB = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        if (!ammoCounterBar) ammoCounterBar = GameObject.Find("AmmoCounterBar");
        bullets = new List<Image>(ammoCounterBar.GetComponentsInChildren<Image>());
        reloadTime = reloadingClip.length;
        GameObject parentOf =//( FindObjectOfType<GameObject>().name == "VFX Container")? 
            new GameObject();
        parentOf.transform.SetParent(this.transform);
        parentOf.name = "VFX Container";
        heartRates = new List<heartrateScript>(playerCanvas.GetComponentsInChildren<heartrateScript>());
        Debug.Log(heartRates.Count);
        //Debug.Log(heartRates.Count);
        StartCoroutine(MakeSureThisThing());
        
        //PoolManager.Instance.ClearPool();
        PoolManager.RequestCreatePool(VFX_BulletMark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_BulletSpark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_HitShark, 60, parentOf.transform);
        originScreen = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

    }
    Vector2 originScreen;
    IEnumerator SharkAttack()
    {
        yield return new WaitUntil(() => currentPD != null);
        yield return new WaitUntil(() => currentPD.duration > 48f);

    }
    IEnumerator PlayerHax()
    {
        Debug.Log("OUTSIDE");
        yield return new WaitUntil(() => Input.anyKeyDown);

        //if (Input.GetKeyDown(KeyCode.Semicolon))
        //{
        //    Debug.Log("IN");
#if UNITY_EDITOR

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Semicolon))
            {
                currentPD.time += 2f;

            }
            yield return null;
        }
#endif
    }

    IEnumerator HardcodedDisgustingEvents()
    {
        yield return new WaitUntil(() => currentPD.time > 16f);
        //Shoot At Door
        currentPD.Pause(); 
        yield return new WaitUntil(() => Input.anyKeyDown);
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 18f);
        currentPD.Pause();
        yield return new WaitUntil(() => Input.anyKeyDown);
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 37f);
        currentPD.Pause();
        yield return new WaitUntil(() => Input.anyKeyDown);
        currentPD.Resume();

        yield return new WaitUntil(() => currentPD.time > 50f);
        //Pop circle on it but dont kill it
        Debug.Log("Shark");


    }
    public override void RegisterSelf()
    {
        base.RegisterSelf();
        GetFunctionWithSceneIndex
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void GetFunctionWithSceneIndex(int sceneBuildIndex)
    {
        Debug.Log(sceneBuildIndex);
        switch (sceneBuildIndex)
        {
            case 0:
                StartCoroutine(SceneZeroFunction());
                break;
            case 1:
                //Init();
                AssignTentacleList();
                break;
        }
    }

    bool setPos = false;
    IEnumerator SceneZeroFunction()
    {
        if (!setPos) setPos = true;
        
        else
        {
            playerCanvas.SetActive(false);
            titleCanvas.SetActive(true);
            leaderboardUI.SetActive(true);
        }
        PlayableDirector[] playables = FindObjectsOfType<PlayableDirector>();
        for (int i = 0; i < playables.Length; i++)
        {
            if (playables[i].gameObject.name == "GameplayTimeline")
            {
                currentPD = playables[i];
                break;
            }
        }

        yield return new WaitUntil(() => currentPD != null);
        yield return new WaitUntil(() => currentPD.time > 5f);
        StartCoroutine(PlayerHax());
        yield return new WaitUntil(() => currentPD.time > 20f);
        AttributeReset();
        StartCoroutine(OxyDropping());
        playerCanvas.SetActive(true);
        for(int i = 0; i < heartRates.Count; i ++)
        {
            heartRates[i].Init();
        }
        StartCoroutine(UIUpdate());
        StartCoroutine(GameplayUpdate()); // Settle these coroutines properly can alrd
            // make sure they stop updating when player die
            //make sure they only update when player play
        StartCoroutine(CheapFadeToNextScene());
    }

    void AttributeReset()
    {
        currHealth = maxHealth;
        currOxygen = maxOxygen;
        currBullet = maxBullet;
        for (int i = 0; 0 < maxBullet - 1; i++)
        {
            int b = (maxBullet - 1) - i;

            if (b < 0) break;

            if (!bullets[b].gameObject.activeSelf) bullets[b].gameObject.SetActive(true);
        }
        uglyStop = false;
    }
    public void Init()
    {


    }

    IEnumerator CheapFadeToNextScene()
    {
        //pd = FindObjectOfType<UnityEngine.Playables.PlayableDirector>();
        if (currentPD == null) yield break;
        yield return new WaitUntil(() => currentPD.state != PlayState.Paused);
        yield return new WaitUntil(() => currentPD.time + 5f > currentPD.duration);
        SceneChanger.Instance.Fading(1);
    }

    List<Tentacle> tentacles;
    public void AssignTentacleList()
    {
        tentacles = new List<Tentacle>(GameObject.FindObjectsOfType<Tentacle>());
        StartCoroutine(TriggerTentacles());        
    }
    bool uglyStop = false;
    public bool fuckW = false;
    IEnumerator GameplayUpdate()
    {
        yield return new WaitUntil(() => playerCanvas.activeInHierarchy);
        while (true)
        {
            if (!playerCanvas.activeInHierarchy) yield break;

            if (currHealth < 0)
            {
                PlayerDeath();
                yield break;
            }
            Stats.Instance.timeTaken3 += 1 * Time.deltaTime;

            if (shootTimerNow < shootEvery) shootTimerNow += Time.deltaTime;
            fuckW = false;
            if (Input.GetMouseButton(0))
            {
                
                if (shootTimerNow > shootEvery && !reloading)
                {
                    RaycastHit hit;
                    Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawRay(point.origin,point.direction* 100f,Color.red,1f);
                    if (currBullet > 0)
                    {
                        bullets[(maxBullet - currBullet)].gameObject.SetActive (false);
                        shootTimerNow = 0;
                        GunAudioPlay(gunFire);
                        
                        if (Physics.Raycast(this.transform.position, point.direction, out hit))
                        {
                            targetHit = hit.transform.gameObject;
                            pointHit = hit.point;
                            fuckW = true;
                            if (hit.transform.GetComponent<AI>())
                                DamageShark(targetHit, pointHit);
                            else if (hit.transform.GetComponent<InteractableObj>())                                   //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                                hit.transform.GetComponent<InteractableObj>().Interact();
                            else if (hit.transform.GetComponent<Boss>())
                                hit.transform.GetComponent<Boss>().OnHit();
                            else if (hit.transform.GetComponentInChildren<Canvas>())
                                Debug.Log("Canvas " + hit.transform.name);
                            else
                            { 
                                //if (hit.transform.name == "Bone023")                          //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                                //    hit.transform.GetComponentInParent<Tentacle>().OnHit();
                                //else
                                DamageProps(targetHit, pointHit);
                            }
                        }
                        if (currBullet - 1 == 0)
                            StartCoroutine(WorkAroundButton());
                        else
                        {
                            currBullet--;
                            Stats.Instance.TrackStats(0, 1);
                        }
                    }
                    else
                    {
                        if (!gunASource.isPlaying)  GunAudioPlay(emptyGunFire);
                    }
                }

            }
            if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());
            yield return null;

        }
    }
  
    void PlayerDeath()
    {
        //Fade back to scene 1
        Debug.Log("DEATH!");
        SceneChanger.Instance.Fading(0);
        uglyStop = true;
    }

    
    #region Public Functions

    public void HealthDropping(float _healthDrop)
    {
        Stats.Instance.TrackStats(2, _healthDrop);
        currHealth -= _healthDrop;
    }
    public void AddOxygen(float x)
    {   //Called from Interact_OxygenTank
        StartCoroutine(AddOx(x));
    }

    public void ShakeCam ()
    {   
        StartCoroutine(ShakerShaker());
        int rnd = Random.Range(0, characterDamagedClips.Count);
        ForcedPlayerAudioPlay(characterDamagedClips[rnd]);
        //playerASource.Play();
    }

    #endregion

    #region PrivateCoroutines under active gameplay

    public float triggerTenTime = 3.2f;
    IEnumerator TriggerTentacles()
    {
        if (tentacles.Count < 1) yield break;
        List<float> dist;
        while (true)
        {
            dist = new List<float>();

            for (int i = 0; i < tentacles.Count; i++)
            {if (tentacles[i] == null) yield break;
                float newV3 = Vector3.Distance(this.transform.position, tentacles[i].transform.position);
                dist.Add(newV3);
            }
            //Tentacle furthest = tentacles.Max(x => Vector3.Distance(x.transform.position, transform.position));

            int chosen = 0, chosen2 = 0;
            for (int j = chosen; j < tentacles.Count - 1; j++)
            {

                if (dist[chosen] < dist[j + 1]) chosen = j + 1;

                if (dist[chosen2] > dist[j + 1]) chosen2 = j + 1;
            }

            tentacles[chosen2].nearAttack = true;
            tentacles[chosen].rangeAttack = true;
            for (int k = 0; k < tentacles.Count; k++)
            {
                if (tentacles[k] == null) yield break;
                if (k != chosen) tentacles[k].rangeAttack = false;
                if (k != chosen2) tentacles[k].nearAttack = false;
            }

            yield return new WaitForSeconds(triggerTenTime);
        }
    }

    IEnumerator WorkAroundButton()
    {
        yield return new WaitForSeconds(shootEvery);
        currBullet--;
        Stats.Instance.TrackStats(0, 1);
    }
    IEnumerator UIUpdate()
    {
        while (true)
        {
            if (uglyStop) yield break;
            if(parentCam)
            compassSlider.value = (parentCam.transform.localEulerAngles.y / 360f);
            oxygenBar.value = currOxygen / maxOxygen;
            yield return null;
        }
    }
    bool reloading = false;
    IEnumerator Reload()
    {
        if (reloading) yield break;
        reloading = true;
        GunAudioPlay(reloadingClip);
        yield return new WaitForSeconds(reloadingClip.length);

        for (int i = 0; 0 < maxBullet - 1; i++)
        {
            int b = (maxBullet - 1) - i;

            if (b < 0) break;

            if (!bullets[b].gameObject.activeSelf)
            {
                bullets[b].gameObject.SetActive(true);
            }
        }
        currBullet = maxBullet;
        reloading = false;
    }
    IEnumerator MakeSureThisThing()
    {
        while (true)
        {
            if (this.transform.localPosition != Vector3.zero) { 
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, Vector3.zero, 4f);
                
        }
            yield return null;
        }
    }
    IEnumerator OxyDropping()
    {
        while (true)
        {
            if (!SceneChanger.Instance.transitting)
            {
                if (uglyStop) yield break;
                currOxygen -= oxyDrop;
                Stats.Instance.TrackStats(5, oxyDrop);
            }
            yield return null;
        }
    }

    IEnumerator AddOx(float x)
    {
        //System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        float amtToAdd = x,
            finishAddingIn = 4,
            perUpdateAdd = amtToAdd / (finishAddingIn * 60);
        int numberOfUpdate = 0;

        //Debug.Log("finishAddingIn: " + finishAddingIn +  " perUpdatesAdd: " + perUpdateAdd);
        //timer.Start();
        while (amtToAdd > 0)
        {
            amtToAdd -= perUpdateAdd;
            currOxygen = ((perUpdateAdd + currOxygen) < maxOxygen) ? currOxygen + perUpdateAdd : maxOxygen;
            //Debug.Log(Time.deltaTime);
            numberOfUpdate++;
            //yield return null;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator ShakerShaker()
    {
        Vector3 originValue = parentCam.transform.localEulerAngles;
        if (!CB)
            CB = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        CB.enabled = false;

        int rndToGo = Random.Range(0, 3);
        Vector3 val;
        switch (rndToGo)
        {
            case 0:
                val =// (pos.x > Screen.width / 2) ?
                    new Vector3(0, 30f, 0);

                break;
            case 1:
                val = new Vector3(30,0,0);
                break;

            case 2:
                val = new Vector3(-30,0,0);
                break;
            default:
                val = new Vector3(-20,0,0);
                break;
        }

        StartCoroutine(GotHit());
        parentCam.transform.localEulerAngles += val / 2;
        yield return null;
        parentCam.transform.localEulerAngles += val / 2;
        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            parentCam.transform.localEulerAngles = Vector3.Lerp(parentCam.transform.localEulerAngles, originValue, Time.deltaTime * 2f);
            yield return null;
        }
        CB.enabled = true;

    }

    //public void Damaged()
    //{
    //    StartCoroutine(GotHit())
    //}

    IEnumerator GotHit()
    {
        //put alpha up to 25 then cut down in a second
        redImage.color = new Color(1, 0, 0, 0.25f);
        //Should go Up to a number then go down back to zero again
        //yield return new WaitUntil(() => currHealth < maxHealth / 2);

        float timer = 1f, timerNow = 0f;
        while (timerNow < timer)
        {
            timerNow += Time.deltaTime;
            redImage.color = Color.Lerp(redImage.color, Color.clear, Time.deltaTime);
            yield return null;
        }
        redImage.color = Color.clear;
    }
    bool comboCounting;
    int currentComboCount;
    int currentHighestComboCount = 0;
    IEnumerator ComboText()
    {
        currentComboCount++;
        comboText.color = Color.white;
        comboText.text = currentComboCount.ToString();
        if (comboCounting) yield break;
        comboCounting = true;
        while(comboText.color.a  > 0)
        {
            comboText.color = Color.Lerp(comboText.color, Color.clear, Time.deltaTime);
            yield return null;
        }
        if (currentComboCount > currentHighestComboCount)
            currentHighestComboCount = currentComboCount;
        currentComboCount = 0;
        comboCounting = false;
    }

    #endregion

    #region Shooting

    void DamageShark(GameObject targetHitName, Vector3 pointHitPosition)
    {
        targetHitName.GetComponent<AI>().currHealth -= bulletDamage;
        Stats.Instance.TrackStats(1, 1);
        GetGOWithPrefab(VFX_HitShark, pointHitPosition, targetHitName.transform.rotation);
    }

    void DamageProps(GameObject targetHitName, Vector3 pointHitPosition)
    {
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, pointHitPosition.normalized);
        GetGOWithPrefab(VFX_BulletSpark, pointHitPosition, targetHitName.transform.rotation);
        GetGOWithPrefab(VFX_BulletMark, pointHitPosition, targetHitName.transform.rotation);
    }
    GameObject GetGOWithPrefab(GameObject prefab, Vector3 pos, Quaternion quat)
    {
        GameObject x = PoolManager.Instance.ReturnGOFromList(prefab);
        x.transform.position = pos;
        x.transform.rotation = quat;
        return x;
    }
    void GunAudioPlay(AudioClip clip)//, float volume = 1.0f)
    {
        gunASource.clip = clip;
        gunASource.PlayOneShot(clip, PlayerPrefs.GetFloat(AudioManager.masterVol));
    }

    void ForcedPlayerAudioPlay(AudioClip clip)
    {
        //int rnd = Random.Range(0, characterDamagedClips.Count);
        playerASource.clip = clip;
        playerASource.PlayOneShot(clip, PlayerPrefs.GetFloat(AudioManager.masterVol));
    }

    void CheckedPlayerAudioPlay(AudioClip clip)
    {
        if (!playerASource.isPlaying)
        {
            playerASource.clip = clip;
            playerASource.PlayOneShot(clip, PlayerPrefs.GetFloat(AudioManager.masterVol));

        }
    }

    #endregion
}
