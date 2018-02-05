using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Player : ISingleton<Player> {

    #region Attributes
    
    GameObject targetHit;
    [HideInInspector]
    public Vector3 pointHit;

    [Header("[None of this should be empty]")]
    public AudioSource audioSource;
    public Slider oxygenBar;
    public GameObject VFX_HitShark;
    public GameObject VFX_BulletMark;
    public GameObject VFX_BulletSpark;
    public AudioClip gunFire, reloadingClip, emptyGunFire;
    public GameObject ammoCounterBar;
    public Image spr_OxygenBar;

    public Text reloadText;
    public Text oxygenText;
    public Slider compassSlider;

    List<Image> bullets = new List<Image>();

    [Header("[Player's Attributes]")]

    [Header("[Things to Set]")]

    public float maxSuitHealth;
    public float maxOxygen, maxHealth,
        oxyDrop, healthDrop,
        reloadTime = 3, bulletDamage, shootEvery = 1;
    int maxBullet = 30;

    [Header("Dont+Touch+For+Debug+Purpose")]
    public int  currBullet;
    public float currSuitHealth, currOxygen, currHealth
        , shootTimerNow;
    
    float healthBarCount1, healthBarCount2;
    
    Cinemachine.CinemachineBrain CB;

    #endregion

    void Start () {
        Init();
        StartCoroutine(TriggerTentacles());
    }

    public void Init()
    {
        currHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        currSuitHealth = maxSuitHealth;
        currOxygen = maxOxygen;
        currBullet = maxBullet;
        reloadTime = reloadingClip.length;

        GameObject parentOf = new GameObject();
        parentOf.name = "VFX Container";

        PoolManager.RequestCreatePool(VFX_BulletMark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_BulletSpark, 60, parentOf.transform);
        PoolManager.RequestCreatePool(VFX_HitShark, 60, parentOf.transform);

        CB = this.GetComponent<Cinemachine.CinemachineBrain>();

        AssignTentacleList();
        bullets = new List<Image>(ammoCounterBar.GetComponentsInChildren<Image>());
        StartCoroutine(UIUpdate());
        StartCoroutine(OxyDropping());
        //StartCoroutine(SuittedUp());
    }
    void PlayAudioClip(AudioClip clip, float volume = 1.0f)
    {
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip, volume);
    }

    List<Tentacle> tentacles = new List<Tentacle>();

    public void AssignTentacleList()
    {
        tentacles = new List<Tentacle>(GameObject.FindObjectsOfType<Tentacle>());
        StartCoroutine(TriggerTentacles());        
    }
    
    void Update()
    {
        Stats.Instance.timeTaken3 += 1 * Time.deltaTime;
        if (currSuitHealth < 0)         Debug.Log("death");

        if (shootTimerNow < shootEvery) shootTimerNow += Time.deltaTime;
        
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (shootTimerNow > shootEvery && !reloading)
            {
                if (currBullet > 0)
                {
                    ImageUpdate(false);
                    shootTimerNow = 0;
                    PlayAudioClip(gunFire);


                    if (Physics.Raycast(this.transform.position, point.direction, out hit))
                    {
                        targetHit = hit.transform.gameObject;
                        pointHit = hit.point;
                        
                        if (hit.transform.GetComponent<AI>())
                            DamageShark(targetHit, pointHit);
                        else if (hit.transform.GetComponent<InteractableObj>())                                   //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                            hit.transform.GetComponent<InteractableObj>().Interact();
                        else if (hit.transform.GetComponent<Boss>())
                            hit.transform.GetComponent<Boss>().bossCurrHealth -= 15f;
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
                }else{
                    if (!audioSource.isPlaying)
                    {
                        audioSource.clip = emptyGunFire;
                        audioSource.Play();
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());

    }
  

    public IEnumerator OxyDropping()
    {
        while (true)
        {
            currOxygen -= oxyDrop;
            Stats.Instance.TrackStats(5, oxyDrop);
            yield return null;
        }
    }

    void ImageUpdate(bool x)
    {
        bullets[CurrImage()].gameObject.SetActive(x);
    }

    int CurrImage()
    {
        return  maxBullet - currBullet;
    }
    #region Public Functions

    public void HealthDropping(float _healthDrop)
    {
        Stats.Instance.TrackStats(2, _healthDrop);
        currHealth -= _healthDrop;
    }
    public void AddOxygen(float x)
    {
        StartCoroutine(AddOx(x));
    }

    public void ShakeCam(Vector3 pos)
    {
        StartCoroutine(ShakerShaker(pos));
    }

    #endregion

    #region PrivateCoroutines
    IEnumerator TriggerTentacles()
    {
        if (tentacles.Count < 0) yield break;
        while (true)
        {
            List<float> dist = new List<float>();

            for (int i = 0; i < tentacles.Count; i++)
            {
                float newV3 = Vector3.Distance(this.transform.position, tentacles[i].transform.position);
                dist.Add(newV3);
            }

            //Tentacle furthest = tentacles.Find(x=> (x.gameObject.transform.position )

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
                if (k != chosen) tentacles[k].rangeAttack = false;
                if (k != chosen2) tentacles[k].nearAttack = false;
            }

            yield return new WaitForSeconds(3.2f);
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
            compassSlider.value = (this.transform.localEulerAngles.y / 360f);
            oxygenBar.value = currOxygen / maxOxygen;
            yield return null;
        }
    }
    bool reloading = false;
    IEnumerator Reload()
    {
        if (reloading) yield break;
        reloading = true;
        PlayAudioClip(reloadingClip);
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

        //Debug.Log("finishAddingIn: " + finishAddingIn +  " seconds, perUpdatesAdd: " + perUpdateAdd + "numberOfUpdate: " + numberOfUpdate);
        //timer.Stop();
        //Debug.Log(timer.Elapsed + " | " + timer.ElapsedMilliseconds);
    }
    IEnumerator ShakerShaker(Vector3 tipPos)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(tipPos);
        Debug.Log(pos);
        Vector3 originValue = this.transform.localEulerAngles;
        CB.enabled = false;
        Vector3 val = (pos.x > Screen.width / 2) ?
            new Vector3(0, 30f, 0) : new Vector3(0, -30f, 0);
        
        this.transform.localEulerAngles += val/2;
        yield return null;
        this.transform.localEulerAngles += val/2;

        float timer = 0; //timerNow = 1f;
        
        while (timer < 1.3f)
        {
            timer += Time.deltaTime;
            this.transform.localEulerAngles = Vector3.Lerp(this.transform.localEulerAngles, originValue, Time.deltaTime * 2f);
            yield return null;
        }

        CB.enabled = true;

    }

    #endregion

    void DamageShark(GameObject targetHitName, Vector3 pointHitPosition)
    {
        targetHitName.GetComponent<AI>().currHealth -= bulletDamage;
        Stats.Instance.TrackStats(1, 1);
        Instantiate(VFX_HitShark, pointHitPosition, targetHitName.transform.rotation);
    }

    void DamageProps(GameObject targetHitName, Vector3 pointHitPosition)
    {
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, pointHitPosition.normalized);
        Instantiate(VFX_BulletSpark, pointHitPosition, targetHitName.transform.rotation);
        Instantiate(VFX_BulletMark, pointHitPosition, targetHitName.transform.rotation);
    }
    GameObject GetGOWithPrefab(GameObject prefab, Vector3 pos, Quaternion quat)
    {
        GameObject x = PoolManager.Instance.ReturnGOFromList(prefab);
        x.transform.position = pos;
        x.transform.rotation = quat;
        return x;
    }
}
