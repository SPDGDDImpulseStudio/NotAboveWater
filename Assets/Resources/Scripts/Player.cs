using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using EZCameraShake;
[RequireComponent(typeof(AudioSource))]
public class Player : ISingleton<Player>
{
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
    public Image redImageForLowHP;
    public Image redImageForDamaged;
    public Image reloadImage;
    public Image oxygenLow;

    [Header("[AudioClips]")]
    public AudioClip gunFire;
    public AudioClip reloadingClip, emptyGunFire;
    public List<AudioClip> characterDamagedClips = new List<AudioClip>();

    [Header("[Various UI]")]
    public GameObject ammoCounterBar;
    public Slider compassSlider, oxygenBar, bossHpSlider;
    public GameObject playerCanvas, titleCanvas, endGameGO, pauseMenuUI;
    public Text reloadText, oxygenText, comboText;
    public GameObject circlePrefab;
    public Text totalScoreText;
    public Text animatedText;


    public Text scoreText, nameText, timeText, accuracyText, combo_Text, treasureFoundText;

    [Header("[Player's Attributes]")]

    [Header("[Things to Set]")]
    public float maxHealth;
    [Tooltip("Oxygen drops every frame, beware to set properly")]
    public float maxOxygen,
        oxyDrop,
        reloadTime = 3, shootEvery = 0.3f;

    [Header("Dont+Touch+For+Debug+Purpose")]
    public int currBullet;
    public float currHealth, currOxygen, shootTimerNow;
    public Cinemachine.CinemachineBrain CB;
    public PlayableDirector currentPD;

    PlayableDirector[] playables;
    float currentPDDuration;
    List<Image> bullets = new List<Image>();
    List<heartrateScript> heartRates = new List<heartrateScript>();
    int maxBullet = 30;

    [Header("[Assign from Caller In Scene 0]")]
    public List<GameObject> blobs = new List<GameObject>();
    public GameObject grenade, block, parentCam, sharkEventGO, boxSuitGO;
    public Cinemachine.CinemachineVirtualCamera cam01, cam02, cam03;

    



    #endregion


    #region OnlyCallOnce

    Vector2 originScreen;
    void Start()
    {
        CB = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        bullets = new List<Image>(ammoCounterBar.GetComponentsInChildren<Image>());
        heartRates = new List<heartrateScript>(playerCanvas.GetComponentsInChildren<heartrateScript>());
        reloadTime = reloadingClip.length;

        #region VFX POOL

        GameObject parentOf = new GameObject();
        parentOf.transform.SetParent(this.transform);
        parentOf.name = "VFX Container";
        PoolManager.RequestCreatePool(VFX_BulletMark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_BulletSpark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_HitShark, 60, parentOf.transform);

        #endregion

        StartCoroutine(MakeSureThisThing());
        originScreen = new Vector2(1920f, 1080f);
        //sharkEventGO =  GameObject.Find("NewSharkPrefab");
        originC = redImageForLowHP.color;
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
    public override void RegisterSelf()
    {
        base.RegisterSelf();
        GetFunctionWithSceneIndex
            (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void GetFunctionWithSceneIndex(int sceneBuildIndex)
    {
        switch (sceneBuildIndex)
        {
            case 0: StartCoroutine(SceneZeroFunction()); break;
            //case 1: StartCoroutine(SceneOneFunction()); break;
        }
    }

    #endregion


    int nextNum = 0;
    void SetCircle(GameObject x, bool b = false)
    {
        CirclePosUpdate circle = PoolManager.Instance.ReturnGOFromList(circlePrefab).GetComponent<CirclePosUpdate>();

        if (x.name == "Grenade" || x.name == "Block")
        {
            circle.Init_(x, true);
            x.GetComponent<KeyObject>().circle = circle;
        }
        else
        {
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


    public void Reset()
    {
        if (pause) PauseFunction();
        SceneChanger.Instance.Fading(0);
    }
    public void GainScore(float toShow)
    {
        float newScore = toShow;

        if(currentComboCount > 0)
        {
            newScore *= (float)((currentComboCount * 0.1) + 1);

            newScore = Mathf.RoundToInt(newScore);
        }
       
        Stats.Instance.TrackStats(10, newScore);
        Vector3 pos = Input.mousePosition;
        pos.z = 0;
        string toShowString = "+" + newScore.ToString();
        totalScoreText.text =  Stats.Instance.gameScores.ToString();
        StartCoroutine(ScoreText(pos, toShowString));
        StartCoroutine(AnimateText(newScore));
    }
    bool scoring = false;

    IEnumerator ScoreText(Vector3 pos, string textToShow)
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
    #region ComboFN
    bool comboCounting;
    int currentComboCount;
    public void GainCombo()
    {
        StartCoroutine(ComboText());
    }
    IEnumerator ComboText()
    {
        currentComboCount++;
        Stats.Instance.chainComboCURRENT8 = currentComboCount;
        if (currentComboCount > Stats.Instance.chainComboMAX9)
            Stats.Instance.chainComboMAX9 = currentComboCount;

        Debug.Log(Stats.Instance.chainComboMAX9);
        comboText.color = Color.white;
        comboText.text = currentComboCount.ToString();
        StartCoroutine(PopUp());
        if (comboCounting) yield break;

        comboCounting = true;
        while (comboText.color.a > 0.5)
        {
            comboText.color = Color.Lerp(comboText.color, Color.clear, Time.deltaTime / 5);
            yield return null;
        }
        comboText.gameObject.SetActive(false);
        
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

        while (comboText.fontSize > 24)
        {
            comboText.fontSize -= 4;
            yield return null;
        }
    }
    float recordScore = 0;
    bool animated = false;
    float posYForText = 351.6f;
    IEnumerator AnimateText(float score)
    {
        recordScore += score;
        animatedText.text = "+" + recordScore.ToString();
        animatedText.color = Color.white;
        animatedText.transform.position = new Vector2(animatedText.transform.position.x, posYForText);
        GainCombo();
        if (animated) yield break;
        animated = true;
        Vector3 target = animatedText.transform.position + new Vector3(0, 30, 0);
        while (animatedText.color.a > 0.5)
        {
            animatedText.transform.position = Vector3.MoveTowards(animatedText.transform.position, target, 10f * Time.deltaTime);
            animatedText.color = Color.Lerp(animatedText.color, Color.clear, Time.deltaTime / 5);
            yield return null;
        }
        animatedText.color = Color.clear;
        animated = false;
        //totalScoreText.text
        recordScore = 0;

        animatedText.transform.position = new Vector2(animatedText.transform.position.x, posYForText);

    }
    #endregion
    #region Miscellaneous

    bool playerHack = false;
    public void StartPlayerHax()
    {//Called from pressing that hidden button
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
        else playerHack = false;
    }

    #endregion

    bool pause = false;

    public void PauseFunction()
    {
        if (!pause) Time.timeScale = 0f;
        else Time.timeScale = 1f;

        pause = !pause;
    }

    #region SceneZero
    bool setPos = false;
    IEnumerator SceneZeroFunction()
    {
        if (!setPos) setPos = true;
        else
        {
            playerCanvas.SetActive(false);
            titleCanvas.SetActive(true);
            endGameGO.SetActive(true);
            totalScoreText.text = "00000000";
        }
        bossHpSlider.gameObject.SetActive(false);
 
        yield return new WaitUntil(() => currentPD != null);
    }
    public void StartButtonPressed()
    {
        titleCanvas.SetActive(false);
        transform.localPosition = new Vector3(0, 0, 0);
        Screen.SetResolution((int)originScreen.x, (int)originScreen.y, Screen.fullScreen);

        playables = FindObjectsOfType<PlayableDirector>();
        AudioManager.Instance.WhenPlayBtnPress();
        StartCoroutine(PlayerHax());
        StartCoroutine(SceneZeroEvents());
        StartCoroutine(RedImageBlink());
        StartCoroutine(ReloadImage());
        StartCoroutine(LowOxygen());
    }
    public bool skip;
    IEnumerator SceneZeroEvents()
    {
        SwitchingTimeline(0);
        currentPDDuration = (float)currentPD.duration;
        Cinemachine.CinemachineVirtualCamera dx = cam01; //GameObject.Find("CM_DollyCam_Gameplay_00").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        dx.Priority = 13;
        yield return new WaitUntil(() => currentPD.time > 11f);
         if (boxSuitGO)
        {
            currentPD.Pause();
       
            Interact_CompulCrates x = boxSuitGO.GetComponent<Interact_CompulCrates>();
            yield return new WaitUntil(() => !x);
        }
        StartsGameplay();
        dx.Priority = 11;
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 13);
        AudioManager.Instance.StopRainingVoice();
        yield return new WaitUntil(() => currentPD.time + 1 > currentPDDuration);

        if (!skip)
        {
            SwitchingTimeline(1);
            currentPDDuration = (float)currentPD.duration;
            yield return new WaitUntil(() => currentPD.time > 41.2f);
            SetCircle(sharkEventGO, true);
            Time.timeScale = 0.4f;
            yield return new WaitUntil(() => currentPD.time > 42.3f);
            Time.timeScale = 1.0f;
            yield return new WaitUntil(() => currentPD.time + 1 > currentPDDuration);
        }


        SwitchingTimeline(2);
        currentPDDuration = (float)currentPD.duration;
        yield return new WaitUntil(() => currentPD.time + 1 > currentPDDuration);
        dx.Priority = 11;


        SwitchingTimeline(3);
        currentPDDuration = (float)currentPD.duration;
        dx = cam02;// GameObject.Find("CM_AimGrenade_Gameplay_03").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        dx.Priority = 13;
        yield return new WaitUntil(() => currentPD.time > 57.6f);
        SetCircle(grenade);
        currentPD.Pause();
        yield return new WaitUntil(() => !grenade.activeInHierarchy);
        dx.Priority = 11;
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 58f);
        SwitchingTimeline(4);
        currentPDDuration = (float)currentPD.duration;
        dx = cam03;//GameObject.Find("CM_AimBlock_Gameplay_04").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        dx.Priority = 13;
        yield return new WaitUntil(() => currentPD.time > 14);
        SetCircle(block);// GameObject.Find("Block"));
        currentPD.Pause();
        yield return new WaitUntil(() => !block.activeInHierarchy);
        currentPD.Resume();
        yield return new WaitUntil(() => currentPD.time > 22);
        currentPD = null;
        StartCoroutine(CheapFadeToNextScene());
    }

  
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
    void StartsGameplay()
    {
        playerCanvas.SetActive(true);
        AttributeReset();
        StartCoroutine(OxyDropping());
        for (int i = 0; i < heartRates.Count; i++)
        {
            heartRates[i].Init();
            if (i == 0)
                heartRates[0].gameObject.GetComponent<Slider>().value = 0.5f;
            else

                heartRates[1].gameObject.GetComponent<Slider>().value = 1f;
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
    IEnumerator Timeline3Event()
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

    public void CallFromStats(string _name, float scores, float accuracy, float treasures, float time, float combo)
    {
        nameText.text = _name;

        scoreText.text = System.Math.Round(scores).ToString();
        accuracyText.text = (System.Math.Round(accuracy, 2)* 100).ToString() + "%";
        treasureFoundText.text = treasures.ToString();
        float seconds, minutes;
        seconds = Mathf.FloorToInt(time) % 60;
        minutes = Mathf.FloorToInt(time) / 60;

        if (seconds < 1) timeText.text = minutes.ToString() + ":00";
        else if (seconds < 10) timeText.text = minutes.ToString() + ":0" + seconds;
        else timeText.text = minutes.ToString() + ":" + seconds;
        
        combo_Text.text = combo.ToString();
    }

    public void CallStats(int i)
    {
        StartCoroutine(CheckForLeaderboard(i));
    }
    IEnumerator CheckForLeaderboard(int i)
    {
        yield return new WaitUntil(() => GetComponentInChildren<LeaderboardDesu>());
        Debug.Log(i);
        GetComponentInChildren<LeaderboardDesu>().currentInt = i;
        GetComponentInChildren<LeaderboardDesu>().Display();
    }
    public void CallCircleBlobEvent()
    {
        StartCoroutine(Timeline3Event());
        PoolCircle();
        nextNum = 0;
    }

    void PoolCircle()
    {
        GameObject go = new GameObject();
        go.name = "Circle parent";
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        for (int i = 0; i < canvases.Length; i++)
        {
            if (!canvases[i].GetComponent<SceneChanger>())
            {
                go.transform.SetParent(canvases[i].transform.GetChild(0).transform);
                go.transform.SetSiblingIndex(10);
                break;
            }
        }
        PoolManager.RequestCreatePool(circlePrefab, 10, go.transform);
    }

    #endregion
    #region BossFight

    public void CallSceneOneFn(PlayableDirector x, PlayableDirector y)
    {
        StartCoroutine(SceneOneFunction(x,y));
    }

    IEnumerator SceneOneFunction(PlayableDirector x, PlayableDirector y)
    {
        currentPD = x;
        playerCanvas.gameObject.SetActive(false);
        yield return new WaitUntil(() => !SceneChanger.Instance.transitting);
        currentPD.Play();
        float timex = (float) currentPD.duration;
        AssignTentacleList();
        yield return new WaitUntil(() => currentPD.time + 1 > timex);
        currentPD = y;
        FindObjectOfType<Boss>().Init();
        currentPD.Play();
        playerCanvas.gameObject.SetActive(true);
        StartCoroutine(GameplayUpdate());

    }
    IEnumerator CheapFadeToNextScene()
    {
        yield return new WaitUntil(() => currentPD == null);
        SceneChanger.Instance.Fading(1);
    }

    List<Tentacle> tentacles;
    public void AssignTentacleList()
    {
        tentacles = new List<Tentacle>(FindObjectsOfType<Tentacle>());
        Debug.Log(tentacles.Count);
        StartCoroutine(TriggerTentacles());
    }

    public float triggerTenTime = 3.2f;
    IEnumerator TriggerTentacles()
    {
        if (tentacles.Count < 1) yield break;
        List<float> dist;
        while (true)
        {
            dist = new List<float>();

            for (int i = 0; i < tentacles.Count; i++)
            {
                if (tentacles[i] == null) yield break;
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

    bool uglyStop = false;
    #endregion

    #region VFX

    Color originC;
    IEnumerator RedImageBlink()
    {
        yield return new WaitUntil(() => (currHealth / maxHealth < 0.25));
        bool b = false;
        redImageForLowHP.gameObject.SetActive(true);
        Color x = Color.red;
        x.a = 0;
        while (currHealth / maxHealth < 0.25)
        {
            if (b)
            {
                redImageForLowHP.color = Color.Lerp(redImageForLowHP.color, Color.red, Time.deltaTime);
                if (redImageForLowHP.color.a > 0.6) b = !b;
            }
            else
            {
                redImageForLowHP.color = Color.Lerp(redImageForLowHP.color, x, Time.deltaTime);
                if (redImageForLowHP.color.a < 0.1) b = !b;
            }
            yield return null;
        }
        redImageForLowHP.gameObject.SetActive(false);
        StartCoroutine(RedImageBlink());
    }

    IEnumerator LowOxygen()
    {
        yield return new WaitUntil(() => currOxygen / maxOxygen < 0.5);
        //Lerp between Color.white <==> Color.red
        bool b = false;
        float speed =3f;
        while (currOxygen / maxOxygen < 0.5)
        {
            speed = 1 + maxOxygen / currOxygen;
            if (b)
            {
                oxygenLow.color = Color.Lerp(oxygenLow.color, Color.white, Time.deltaTime*speed);
                if (oxygenLow.color.b > 0.9f) b = !b;
            }
            else
            {
                oxygenLow.color = Color.Lerp(oxygenLow.color, Color.red, Time.deltaTime*speed);
                if (oxygenLow.color.b < 0.1f) b = !b;
            }
            yield return null;
        }
        oxygenLow.color = Color.white;
        StartCoroutine(LowOxygen());

    }
    IEnumerator ReloadImage()
    {
        yield return new WaitUntil(() => currBullet < 1);
        bool b = false;
        reloadImage.gameObject.SetActive(true);
        reloadImage.color = Color.white;
        Color x = Color.white;
        x.a = 0;
        while (currBullet < 1)
        {
            if (b)
            {
                reloadImage.color = Color.Lerp(reloadImage.color, Color.white, Time.deltaTime);
                if (reloadImage.color.a > 0.8f) b = !b;
            }
            else
            {
                reloadImage.color = Color.Lerp(reloadImage.color,Color.clear, Time.deltaTime);
                if (reloadImage.color.a < 0.4f) b = !b;
            }

            yield return null;
        }

        reloadImage.gameObject.SetActive(false);
        StartCoroutine(ReloadImage());
    }

    public void ShakeCam()
    {
        StartCoroutine(ShakerShaker());
        int rnd = Random.Range(0, characterDamagedClips.Count);
        ForcedPlayerAudioPlay(characterDamagedClips[rnd]);
        //playerASource.Play();
    }
    IEnumerator ShakerShaker()
    {
        Vector3 originValue = parentCam.transform.localEulerAngles;
        if (!CB)
            CB = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        CB.enabled = false;

        //CameraShaker.Instance.Shake
        int rndToGo = Random.Range(0, 3);
        Vector3 val;
        switch (rndToGo)
        {
            case 0:
                val =// (pos.x > Screen.width / 2) ?
                    new Vector3(0, 30f, 0);
                break;
            case 1:
                val = new Vector3(30, 0, 0);
                break;

            case 2:
                val = new Vector3(0, 50f, 0);
                break;
            default:
                val = new Vector3(-20, 0, 0);
                break;
        }

        StartCoroutine(GotHit());
        //parentCam.transform.localEulerAngles += val / 2;
        yield return null;
        CameraShaker.Instance.ShakeOnce(4.0f, 4.0f, 1f, 1f);
        //parentCam.transform.localEulerAngles += val / 2;
        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            parentCam.transform.localEulerAngles = Vector3.Lerp(parentCam.transform.localEulerAngles, originValue, Time.deltaTime * 2f);
            yield return null;
        }
        CB.enabled = true;
    }
    IEnumerator GotHit()
    {
        //put alpha up to 25 then cut down in a second
        redImageForDamaged.color = new Color(1, 0, 0, 0.25f);
        //Should go Up to a number then go down back to zero again
        //yield return new WaitUntil(() => currHealth < maxHealth / 2);

        float timer = 1f, timerNow = 0f;
        while (timerNow < timer)
        {
            timerNow += Time.deltaTime;
            redImageForDamaged.color = Color.Lerp(redImageForDamaged.color, Color.clear, Time.deltaTime);
            yield return null;
        }
        redImageForDamaged.color = Color.clear;
    }
    #endregion


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
            if (playerCanvas.activeInHierarchy)
            {
                if (shootTimerNow < shootEvery) shootTimerNow += Time.deltaTime;
                if (Input.GetMouseButton(0))
                {
                    if (shootTimerNow > shootEvery && !reloading)
                    {
                        RaycastHit hit;
                        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Debug.DrawRay(point.origin, point.direction * 100f, Color.red, 1f);
                        if (currBullet > 0)
                        {
                            Stats.Instance.TrackStats(0, 1);

                            bullets[(maxBullet - currBullet)].gameObject.SetActive(false);
                            shootTimerNow = 0;
                            GunAudioPlay(gunFire);

                            if (Physics.Raycast(this.transform.position, point.direction, out hit))
                            {
                                targetHit = hit.transform.gameObject;
                                pointHit = hit.point;
                                Debug.Log(targetHit.name);
                                if (hit.transform.GetComponent<AI>())
                                    DamageShark(targetHit, pointHit);
                                else if (hit.transform.GetComponent<InteractableObj>())
                                {
                                    Stats.Instance.TrackStats(1, 1);
                                    //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                                    hit.transform.GetComponent<InteractableObj>().Interact();
                                }
                                else if (hit.transform.name == "Block" || hit.transform.name == "Grenade")
                                {
                                    Debug.Log(hit.transform.GetComponent<MeshCollider>() + " " + hit.transform.GetComponent<BoxCollider>());
                                    hit.transform.GetComponent<KeyObject>().DeductCircleHealth();
                                    Stats.Instance.TrackStats(1, 1);
                                }
                                else if (hit.transform.GetComponent<BulletScript>())
                                {
                                    Stats.Instance.TrackStats(1, 1);
                                    hit.transform.GetComponent<BulletScript>().DeductCircleHealth();
                                    DamageProps(targetHit, pointHit);
                                }
                                else if (hit.transform.GetComponent<Tentacle>())
                                {
                                    Stats.Instance.TrackStats(1, 1);

                                    hit.transform.GetComponentInChildren<Tentacle>().OnHit();
                                }
                                else if (hit.transform.GetComponent<Shark>())
                                {
                                    Stats.Instance.TrackStats(1, 1);
                                    float rnd = Random.Range(50, 60);
                                    GainScore(rnd);
                                    hit.transform.GetComponentInChildren<Shark>().DeductCircleHealth();
                                }
                                else if (hit.transform.GetComponentInParent<Shark>())
                                {
                                    Debug.Log("Weak point");
                                    Stats.Instance.TrackStats(1, 1);
                                    float rnd = Random.Range(50, 60);
                                    GainScore(rnd);
                                    hit.transform.GetComponentInParent<Shark>().DeductCircleHealth();
                                }
                                else if (hit.transform.GetComponent<TreasureChest>())
                                {
                                    Stats.Instance.TrackStats(1, 1);
                                    hit.transform.GetComponent<TreasureChest>().OnHit();
                                }
                                else if (hit.transform.GetComponent<Boss>())
                                {
                                    hit.transform.GetComponent<Boss>().OnHit();
                                }
                                else if (hit.transform.GetComponentInChildren<Destroyable>())
                                {
                                    Stats.Instance.TrackStats(1, 1);
                                    hit.transform.GetComponentInChildren<Destroyable>().OnHit();
                                }
                                else DamageProps(targetHit, pointHit);
                            }
                            currBullet--;
                            Stats.Instance.TrackStats(0, 1);
                        }
                        else
                        {
                            if (!gunASource.isPlaying) GunAudioPlay(emptyGunFire);
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) StartCoroutine(Reload());
            }
            yield return null;
        }
    }

    public void PlayerDeath()
    {
        if (currentPD != null)
        {
            if (currentPD.time < currentPDDuration)
                currentPD.Pause();
        }
        SceneChanger.Instance.Fading(0);

        uglyStop = true;
    }


    #region Gameplay Functions

    public void HealthDropping(float _healthDrop)
    {
        Stats.Instance.TrackStats(2, _healthDrop);
        currHealth -= _healthDrop;
    }
    public void AddOxygen(float x)
    {   //Called from Interact_OxygenTank
        StartCoroutine(AddOx(x));
    }

    #endregion

    #region PrivateCoroutines under active gameplay
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
            if (parentCam)
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
        float amtToAdd = x,
            finishAddingIn = 4,
            perUpdateAdd = amtToAdd / (finishAddingIn * 60);
        int numberOfUpdate = 0;

        while (amtToAdd > 0)
        {
            amtToAdd -= perUpdateAdd;
            currOxygen = ((perUpdateAdd + currOxygen) < maxOxygen) ? currOxygen + perUpdateAdd : maxOxygen;
            numberOfUpdate++;
            yield return new WaitForFixedUpdate();
        }
    }

 

    #endregion

    #region Shooting

    void DamageShark(GameObject targetHitName, Vector3 pointHitPosition)
    {
        //targetHitName.GetComponent<AI>().currHealth -= bulletDamage;
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


}
