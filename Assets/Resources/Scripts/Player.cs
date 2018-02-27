using System.Collections;
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
    public Text gainScoreText; 
    [Header("[VFX]")]
    public GameObject VFX_HitShark;
    public GameObject VFX_BulletMark;
    public GameObject VFX_BulletSpark;
    public Image SlimeTexture;

    [Header("[AudioClips]")]
    public AudioClip gunFire;
    public AudioClip reloadingClip, emptyGunFire;
    public List<AudioClip> characterDamagedClips = new List<AudioClip>(); 

    [Header("[Various UI]")]
    public GameObject ammoCounterBar;
    public Slider compassSlider, oxygenBar , bossHpSlider;
    public GameObject playerCanvas, titleCanvas, leaderboardUI;
    public Image lowHp;
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

    public PlayableDirector currentPD;
    public GameObject playableDirParent;
    PlayableDirector[] playables;
    float duration;
    public List<GameObject> blobs = new List<GameObject>();
    public GameObject circlePrefab;
    public GameObject pauseMenuUI;
    GameObject grenade;
    GameObject block;
    #endregion
    

    #region OnlyCallOnce
   
    Vector2 originScreen;
    void Start()
    {
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
        StartCoroutine(MakeSureThisThing());
        PoolManager.RequestCreatePool(VFX_BulletMark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_BulletSpark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_HitShark, 60, parentOf.transform);
        originScreen = new Vector2(1920f, 1080f);
        sharkEventGO = GameObject.Find("NewSharkPrefab");
        originC = lowHp.color;
    }
    #endregion

    void SwitchingTimeline(int x)
    {
        if (playables.Length < 1) playables = FindObjectsOfType<PlayableDirector>();

        for (int i = 0; i < playables.Length; i++)
        {
            if (playables[i].gameObject.name == "Gameplay_0" + x + "_Timeline")
            {
                currentPD = playables[i];
                break;
            }
        }
        Debug.Log("Playing " + x + " timeline");

        currentPD.Play();
    }

    public void CallCircleBlobEvent()
    {
        StartCoroutine(CircleOnBlobs());
        GetCircle();
        nextNum = 0;
    }

    void GetCircle()
    {

        GameObject go = new GameObject();
        go.name = "Circle parent";
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            if (!canvases[i].GetComponent<SceneChanger>())
            {
                go.transform.SetParent(canvases[i].transform);
                break;
            }
        }
        PoolManager.RequestCreatePool(circlePrefab, 10, go.transform);

    }
    int nextNum = 0;
    void SetCircle(GameObject x, bool b = false)
    {
        CirclePosUpdate circle = PoolManager.Instance.ReturnGOFromList(circlePrefab).GetComponent<CirclePosUpdate>();

        if(x.name == "Grenade" || x.name == "Block") {
            circle.Init_(x, true);
            x.GetComponent<KeyObject>().circle = circle;
        }
        else {
            if (!b)
            {
                circle.Init_(x, true);
                x.GetComponent<BulletScript>().circle = circle;
                Debug.Log("Set Circle on blob number: " + nextNum);
                nextNum++;
            }
            else
            {
                circle.Init_(x, true);
                x.GetComponentInChildren<Shark>().circle = circle;
                // x = > sshark
            }
        }
    }

    IEnumerator CircleOnBlobs()
    {
        yield return new WaitUntil(() => currentPD != null);

        yield return new WaitUntil(() => currentPD.name == "Gameplay_02_Timeline");

        yield return new WaitUntil(() => currentPD.time > 16);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 18);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 24);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 28);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 31.7f);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 36);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 38.6f);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 40.7f);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 42.5f);
        SetCircle(blobs[nextNum]);

        yield return new WaitUntil(() => currentPD.time > 45f);
        SetCircle(blobs[nextNum]);

    }
    
    public void Reset()
    {
        if (pause) PauseFunction();
        SceneChanger.Instance.Fading(0);
    }
    public void GainScore(float toShow)
    {
        Stats.Instance.TrackStats(10, toShow);
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        string toShowString = "+"+ toShow.ToString();
        StartCoroutine(ScoreText(pos, toShowString));
    }

    public void GainCombo()
    {
        StartCoroutine(ComboText());
    }
    bool scoring = false;

    IEnumerator ScoreText(Vector3 pos , string textToShow)
    {
        //t.text = System.Math.Round(masterVSlider.value, 2).ToString();
        gainScoreText.text = textToShow;
        gainScoreText.color = Color.white;
        gainScoreText.transform.position = pos;
        if (!scoring)
        {
            gainScoreText.gameObject.SetActive(true);
            scoring = true;
            Debug.Log(gainScoreText + " 1");
            while (gainScoreText.color != Color.clear)
            {
                gainScoreText.color = Color.Lerp(gainScoreText.color, Color.clear, Time.deltaTime);
                yield return null;
            }
            scoring = false;
            gainScoreText.gameObject.SetActive(false);
        }
    }

    bool comboCounting;
    int currentComboCount;
    int currentHighestComboCount = 0;

    IEnumerator ComboText()
    {
        currentComboCount++;
        comboText.color = Color.white;
        comboText.text = currentComboCount.ToString();
        StartCoroutine(PopUp());
        if (comboCounting) yield break;

        comboCounting = true;
        while (comboText.color.a < 30)
        {
            comboText.color = Color.Lerp(comboText.color, Color.clear, Time.deltaTime/10);
            yield return null;
        }
        comboText.gameObject.SetActive(false);

        if (currentComboCount > currentHighestComboCount)
            currentHighestComboCount = currentComboCount;
        currentComboCount = 0;
        comboCounting = false;
    }
    IEnumerator PopUp()
    {//combotext font = 24

        comboText.gameObject.SetActive(true);
        while (comboText.fontSize < 60)
        {
            comboText.fontSize += 4;
            yield return null;
        }

        while(comboText.fontSize > 24)
        {
            comboText.fontSize -= 4;
            yield return null;
        }
        //comboText.font
    }
    bool playerHack = false;
    public void StartPlayerHax()
    {
        StartCoroutine(PlayerHax());
    }
    IEnumerator PlayerHax()
    {
        if (playerHack) yield break;
        playerHack = true;
        yield return new WaitUntil(() => !pauseMenuUI.activeInHierarchy);
        yield return new WaitUntil(() => Input.anyKeyDown);


        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            currentPD.time += 2f;
            playerHack = false;
            StartCoroutine(PlayerHax());
        }



        else
            playerHack = false;
        
        Debug.Log("Out");        
    }
    #region Scene Related functions
    bool pause = false;

    public void PauseFunction()
    {
        
            if (!pause) Time.timeScale = 0f;
            else Time.timeScale = 1f;

            pause = !pause;
        
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
        bossHpSlider.gameObject.SetActive(false);
        sharkEventGO = GameObject.Find("NewSharkPrefab");
        grenade = GameObject.Find("Grenade");
        block = GameObject.Find("Block");
        yield return new WaitUntil(() => currentPD != null);
        //yield return new WaitUntil(() => currentPD.time > 5f);
        //StartCoroutine(PlayerHax());
        //yield return new WaitUntil(() => currentPD.time > 20f);

     
        Debug.Log("IN");
       
    }

    void StartsGameplay()
    {
        playerCanvas.SetActive(true);
        AttributeReset();
        StartCoroutine(OxyDropping());
        for (int i = 0; i < heartRates.Count; i++)
        {
            heartRates[i].Init();
        }
        StartCoroutine(UIUpdate());
        StartCoroutine(GameplayUpdate());
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

    bool ThreeCircle()
    {
        return true;
    }
    public GameObject sharkEventGO;
    public bool skip = true;
    IEnumerator DisgustingShit()
    {

        SwitchingTimeline(0);
        duration = (float)currentPD.duration;
        Cinemachine.CinemachineVirtualCamera dx = GameObject.Find("CM_DollyCam_Gameplay_00").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        dx.Priority = 13;
        yield return new WaitUntil(() => currentPD.time > 11f);
        currentPD.Pause();
        Interact_CompulCrates x = FindObjectOfType <Interact_CompulCrates>();
        yield return new WaitUntil(() => !x);
        StartsGameplay();
        dx.Priority = 11;
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 13);
        AudioManager.Instance.StopRainingVoice();
        yield return new WaitUntil(() => currentPD.time + 1 > duration);
        //yield return new WaitUntil(() => Input.anyKeyDown);

        if (!skip)
        {
            SwitchingTimeline(1);
            duration = (float)currentPD.duration;

            yield return new WaitUntil(() => currentPD.time > 41.2f);
            //currentPD.time
            SetCircle(sharkEventGO, true);
            Time.timeScale = 0.4f;

            yield return new WaitUntil(() => currentPD.time > 42.3f);
            Time.timeScale = 1.0f;

            //if (ThreeCircle())
            //    Shark.SetActive(false);
            //else
            //{
            //    PlayerDeath();
            //    yield break;
            //}

            //All Popped > move on


            // else Dead
        }

        yield return new WaitUntil(() => currentPD.time + 1 > duration);
        //yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(2);
        duration = (float)currentPD.duration;
        
        yield return new WaitUntil(() => currentPD.time + 1 > duration);
        dx.Priority = 11;
        //yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(3);
        duration = (float)currentPD.duration;
        dx = GameObject.Find("CM_AimGrenade_Gameplay_03").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        dx.Priority = 13;
        yield return new WaitUntil(() => currentPD.time > 57.6f);
        SetCircle(grenade);
        currentPD.Pause();
    
        yield return new WaitUntil(() => !grenade.activeInHierarchy);
  
        dx.Priority = 11;
        currentPD.Resume();



        yield return new WaitUntil(() => currentPD.time > 58f);

        //yield return new WaitUntil(() => Input.anyKeyDown);
        SwitchingTimeline(4);
        duration = (float)currentPD.duration;
        dx = GameObject.Find("CM_AimBlock_Gameplay_04").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        dx.Priority = 13;
        yield return new WaitUntil(() => currentPD.time > 14);

        SetCircle(GameObject.Find("Block"));
        currentPD.Pause();
        yield return new WaitUntil(() => !block.activeInHierarchy);
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 22);

        //yield return new WaitUntil(() => Input.anyKeyDown);
        currentPD = null;
        StartCoroutine(CheapFadeToNextScene());

    }
    public void PlayerTurnOnTitleOff()
    {
        titleCanvas.SetActive(false);
        //playerCanvas.SetActive(true);
        transform.localPosition = new Vector3(0, 0, 0);
        currentHighestComboCount = 0;
        Screen.SetResolution((int)originScreen.x, (int)originScreen.y, Screen.fullScreen);

        playableDirParent = GameObject.Find("PlaybleDirectors");
        //DontDestroyOnLoad(playableDirParent);
        playables = FindObjectsOfType<PlayableDirector>();
        AudioManager.Instance.WhenPlayBtnPress();
        StartCoroutine(PlayerHax());
        StartCoroutine(DisgustingShit());
        StartCoroutine(RedImageBlink());
    }
    IEnumerator CheapFadeToNextScene()
    {
        yield return new WaitUntil(() => currentPD == null);
        SceneChanger.Instance.Fading(1);
    }
    public void Init()
    {
    }
    List<Tentacle> tentacles;
    public void AssignTentacleList()
    {
        tentacles = new List<Tentacle>(GameObject.FindObjectsOfType<Tentacle>());
        StartCoroutine(TriggerTentacles());
    }
    bool uglyStop = false;

    #endregion
    Color originC;
    IEnumerator RedImageBlink()
    {
        yield return new WaitUntil(() => (currHealth / maxHealth < 0.25));
        bool b = false;
        lowHp.gameObject.SetActive(true);
        Color x = Color.red;
        x.a = 0;
        while (currHealth / maxHealth < 0.25) {
            if (b)
            {
                lowHp.color = Color.Lerp(lowHp.color, Color.red, Time.deltaTime);
                if (lowHp.color.a > 0.6) b = !b;
            }
            else
            {
                lowHp.color = Color.Lerp(lowHp.color, x, Time.deltaTime);
                if (lowHp.color.a < 0.1) b = !b;

            }

            yield return null;
        }
        lowHp.gameObject.SetActive(false);

        StartCoroutine(RedImageBlink());
   
        
    }
    IEnumerator GameplayUpdate()
    {
        yield return new WaitUntil(() => playerCanvas.activeInHierarchy);
        while (true)
        {
            
            if (currHealth < 0 || currOxygen < 0)
            {
                PlayerDeath();
                yield break;
            }
            
            if (SceneChanger.Instance.transitting && !playerCanvas.activeInHierarchy) yield break;
            Stats.Instance.timeTaken3 += 1 * Time.deltaTime;

            if (shootTimerNow < shootEvery) shootTimerNow += Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
               
                if (shootTimerNow > shootEvery && !reloading)
                {
                    RaycastHit hit;
                    Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Debug.DrawRay(point.origin,point.direction* 100f,Color.red,1f);
                    if (currBullet > 0)
                    {
                        Stats.Instance.TrackStats(0, 1);

                        bullets[(maxBullet - currBullet)].gameObject.SetActive (false);
                        shootTimerNow = 0;
                        GunAudioPlay(gunFire);
                        
                        if (Physics.Raycast(this.transform.position, point.direction, out hit))
                        {
                            targetHit = hit.transform.gameObject;
                            pointHit = hit.point;

                            if (hit.transform.GetComponent<AI>())
                                DamageShark(targetHit, pointHit);
                            else if (hit.transform.GetComponent<InteractableObj>())                                   //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                                hit.transform.GetComponent<InteractableObj>().Interact();

                            else if (hit.transform.name == "Block") // Tentacles
                                hit.transform.GetComponent<KeyObject>().DeductCircleHealth();
                            else if (hit.transform.name == "Blob") // Scene 0's blobs
                                                                   //Debug.Log("Blob Scene 01");
                            {
                                hit.transform.GetComponent<BulletScript>().DeductCircleHealth();
                                DamageProps(targetHit, pointHit);
                            }
                            else if (hit.transform.name == "Grenade")
                            {
                                hit.transform.GetComponent<KeyObject>().DeductCircleHealth();
                            }
                            else if (hit.transform.GetComponentInChildren<Shark>())// == "Shark")// Scene 
                            {
                                Debug.Log("Shark");
                                Stats.Instance.TrackStats(1, 1);
                                float rnd = Random.Range(50, 60);
                                Stats.Instance.TrackStats(10, rnd);
                                GainScore(rnd);
                                hit.transform.GetComponentInChildren<Shark>().DeductCircleHealth();
                            }

                            //else if (hit.transform.GetComponent<CircleAttached>())
                            else if (hit.transform.GetComponent<Boss>())
                                hit.transform.GetComponent<Boss>().OnHit();
                            else if (hit.transform.GetComponentInChildren<Destroyable>())
                                hit.transform.GetComponentInChildren<Destroyable>().OnHit();

                            else
                            {
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
            if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) StartCoroutine(Reload());
            yield return null;

        }
    }
  
  public void PlayerDeath()
    {
        //Fade back to scene 0
        Debug.Log("DEATH!");
        if (currentPD != null)
        {
            if (currentPD.time < duration)
                currentPD.Pause();
        }
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
            if (this.transform.localPosition != Vector3.zero)
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, Vector3.zero, 4f);

            }
            yield return null;
        }
    }
    IEnumerator OxyDropping()
    {
        while (true)
        {
            if (!SceneChanger.Instance.transitting && !pause)
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
                val = new Vector3(0,50f,0);
                break;
            default:
                val = new Vector3(-20,0,0);
                break;
        }
        Debug.Log(rndToGo); 

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
        Debug.Log("Damaged");
        Debug.Log(targetHitName.name);
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, pointHitPosition.normalized);
        GetGOWithPrefab(VFX_BulletSpark, pointHitPosition, targetHitName.transform.rotation);
        GetGOWithPrefab(VFX_BulletMark, pointHitPosition, targetHitName.transform.rotation);
    }
    GameObject GetGOWithPrefab(GameObject prefab, Vector3 pos, Quaternion quat)
    {
        GameObject x = PoolManager.Instance.ReturnGOFromList(prefab);
        x.transform.position = pos;
        x.transform.rotation = quat;
        x.SetActive(true);
        return x;
    }
    void GunAudioPlay(AudioClip clip)//, float volume = 1.0f)
    {
        gunASource.clip = clip;
        gunASource.PlayOneShot(clip, PlayerPrefs.GetFloat(AudioManager.masterVol) * PlayerPrefs.GetFloat(AudioManager.sfxVol));
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



    #region Obsolete
    
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
    
    #endregion
}
