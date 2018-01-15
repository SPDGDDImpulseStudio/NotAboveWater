using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour {

    #region Attributes
    public static Player Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Player>();
                if (_instance == null)
                    Debug.LogError("STOP");
                DontDestroyOnLoad(_instance.gameObject);
            }
            if (!_instance.gameObject.activeSelf) _instance.gameObject.SetActive(true);
            return _instance;
        }
    }
    static Player _instance;
    
    GameObject targetHit;
    [HideInInspector]
    public Vector3 pointHit;

    [Header("[None of this should be empty]")]
    public AudioSource audioSource;
    public Slider healthBar, oxygenBar, gunShootingBar;
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

    #endregion

    void Start () {
        if (Instance.GetInstanceID() != this.GetInstanceID())  Destroy(this.gameObject);

        Init(); AssignTentacleList();
        StartCoroutine(TriggerAI());
    }

    public void Init()
    {
        currHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        currSuitHealth = maxSuitHealth;
        currOxygen = maxOxygen;
        currBullet = maxBullet;
        reloadTime = reloadingClip.length;


        bullets = new List<Image>(ammoCounterBar.GetComponentsInChildren<UnityEngine.UI.Image>());
        StartCoroutine(UIUpdate());
        StartCoroutine(OxyDropping());
        //StartCoroutine(SuittedUp());
    }
    public void PlayAudioClip(AudioClip clip, float volume = 1.0f)
    {
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip, volume);
        //audioSource.
    }
    List<Tentacle> tentacles = new List<Tentacle>();
    public void AssignTentacleList()
    {
        tentacles = new List<Tentacle>(GameObject.FindObjectsOfType<Tentacle>());        
    }

    public IEnumerator TriggerAI()
    {
        if (tentacles.Count < 0) yield break;

        while (true)
        {
            List<float> dist = new List<float>();

            for (int i = 0; i < tentacles.Count; i ++)
            {
                float newV3 = Vector3.Distance(this.transform.position, tentacles[i].transform.position);

                dist.Add(newV3);
            }

            int chosen = 0;
            for (int j = chosen; j < tentacles.Count - 1; j++)
            {
                if (dist[chosen] < dist[j + 1]) chosen = j + 1;
            }

            tentacles[chosen].Trigger = true;
            for(int k = 0; k < tentacles.Count; k++)
            {
                if (k == chosen) continue;

                tentacles[k].Trigger = false;
            }

            yield return new WaitForSeconds(1.2f);
        }
    }
    

    void Update()
    {
        if (currSuitHealth < 0)
            Debug.Log("death");
        if (shootTimerNow < shootEvery)
            shootTimerNow += Time.deltaTime;


        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(point.origin, point.direction);
            //Debug.DrawRay(transform.position, point,Color.green);

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

                        //if (hit.transform.GetComponent<Book>())
                        //{
                        //    Debug.Log(hit.transform.GetComponent<Book>().bookSlotInfo.bookSlotPos + " " + hit.transform.GetComponent<Book>().ReturnSlot(hit.transform.position).bookSlotPos);
                        //}
                        if (hit.transform.GetComponent<AI>())
                            DamageShark(targetHit, pointHit);
                        else if (hit.transform.GetComponent<InteractableObj>())                                   //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                            hit.transform.GetComponent<InteractableObj>().Interact();
                        else if (hit.transform.GetComponent<Boss>())
                            hit.transform.GetComponent<Boss>().bossCurrHealth -= 15f;
                        else
                        {
                            if (hit.transform.name == "Bone023")                          //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                                hit.transform.GetComponentInParent<Tentacle>().OnHit();
                            else
                                DamageProps(targetHit, pointHit);
                        }
                    }
                    if (currBullet - 1 == 0)
                        StartCoroutine(WorkAroundButton());
                    else
                        currBullet--;
                }
                else
                {
                    if(!audioSource.isPlaying)
                    audioSource.PlayOneShot(emptyGunFire);
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.R)) StartCoroutine(Reload());

    }
    public IEnumerator WorkAroundButton()
    {
        yield return new WaitForSeconds(shootEvery);
        currBullet--;
    }
    bool reloading = false;//, suitUp = true;

    public IEnumerator Reload()
    {
        if (reloading) yield break; 
        reloading = true;
        PlayAudioClip(reloadingClip);
        yield return new WaitForSeconds(reloadingClip.length);

        for(int i = 0; 0 < maxBullet - 1; i++)
        {
            int b = 29 - i;
            
            if (b < 0) break;
            
            if (!bullets[b].gameObject.activeSelf)
            {
                bullets[b].gameObject.SetActive(true);
            }
        }
        //while (!bullets[0].gameObject.activeSelf)
        //{
        //        Debug.Log(CurrImage());
        //    if(!bullets[CurrImage()].gameObject.activeSelf)
        //    bullets[CurrImage()].gameObject.SetActive(true);
        //    yield return null;
        //}
        currBullet = maxBullet;

        reloading = false;
        gunShootingBar.maxValue = 1;

    }

    public IEnumerator OxyDropping()
    {
        Image[] x = oxygenBar.GetComponentsInChildren<Image>(); // = Color.blue + new Color(0,0.5f, 0);
        for(int i = 0; i < x.Length; i++)
        {
            //x[i].color = Color.blue + new Color(0, 0.5f, 0);
        }
        while (true)
        {
            healthBarCount1 = currOxygen;
            healthBarCount2 = maxOxygen;
            if (currOxygen < 0) {
                StartCoroutine(HealthDropping());
                yield break;
            }
            currOxygen -= oxyDrop;
            yield return null;
        }
    }
    bool healthDrop_ = false;
    public IEnumerator HealthDropping()
    {
        float newNum =96 / 225;
        Image[] x = oxygenBar.GetComponentsInChildren<Image>(); // = Color.blue + new Color(0,0.5f, 0);
        for (int i = 0; i < x.Length; i++)
        {
            x[i].color = Color.red + new Color(0, 0, newNum);
        }
        //oxygenBar.GetComponentsInChildren<Image>().color = Color.red + new Color(0, 0, newNum);
        if (healthDrop_) yield break;
        healthDrop_ = true;
        while (true)
        {
            healthBarCount1 = currHealth;
            healthBarCount2 = maxHealth;
            if (currOxygen > 0) { healthDrop_ = false; StartCoroutine(OxyDropping()); yield break; }
            currHealth -= healthDrop;
            yield return null;
        }
    }

    //public IEnumerator SuittedUp()
    //{
    //    yield return new WaitUntil(() => currSuitHealth >= 0);
    //    suitUp = true;
    //    healthBarCount1 = currSuitHealth;
    //    healthBarCount2 = maxSuitHealth;
    //    //Here i can set the image for UI too
    //    StartCoroutine(SuitDown());
    //}
    //public IEnumerator SuitDown()
    //{
    //    yield return new WaitUntil(() => currSuitHealth <= 0);

    //    suitUp = false;
    //    //StartCoroutine(HealthDropping());
    //    //healthBarCount1 = currHealth;
    //    //healthBarCount2 = maxHealth;
    //    //Here i can set the image for UI too
    //    StartCoroutine(SuittedUp());
    //}
    void ImageUpdate(bool x)
    {
        bullets[CurrImage()].gameObject.SetActive(x);
    }

    int CurrImage()
    {
        return  30 - currBullet;
    }
  
    IEnumerator UIUpdate()
    {
        while (true)
        {
          
            compassSlider.value = (this.transform.localEulerAngles.y/360f);
            //healthBar.value = currSuitHealth / maxSuitHealth;
            oxygenBar.value = healthBarCount1 / healthBarCount2;
            if (currBullet != 0)
            {
                gunShootingBar.value = shootTimerNow / shootEvery;
            }
            else if (reloading)
            {
                //if(gunShootingBar.maxValue == 1)
                gunShootingBar.maxValue = reloadTime;
                gunShootingBar.value += Time.deltaTime;
                //Debug.Log((gunShootingBar.value + Time.deltaTime) + " " + gunShootingBar.maxValue);
                //if ((gunShootingBar.value +Time.deltaTime) >= gunShootingBar.maxValue)
                //{
                //    gunShootingBar.maxValue = 1;
                //    Debug.Log("SET");
                //}
            }
            else
                gunShootingBar.value = 0;

            yield return null;
        }
    }

    public void AddOxygen(float x)
    {
        //currOxygen = ((x + currOxygen) < maxOxygen) ? currOxygen + x : maxOxygen;
        //if ((x + currOxygen) < maxOxygen)
        //    currOxygen += x;
        //else
        //    currOxygen = maxOxygen;
        StartCoroutine(AddOx(x));
    }

    IEnumerator AddOx(float x)
    {
        System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        float amtToAdd = x,
            finishAddingIn = 4,
            perUpdateAdd = amtToAdd / (finishAddingIn * 60);
        int numberOfUpdate = 0;

        //Debug.Log("finishAddingIn: " + finishAddingIn +  " perUpdatesAdd: " + perUpdateAdd);
        timer.Start();
        while(amtToAdd > 0)
        {
            amtToAdd -= perUpdateAdd;
            currOxygen = ((perUpdateAdd + currOxygen) < maxOxygen) ? currOxygen + perUpdateAdd : maxOxygen;
            //Debug.Log(Time.deltaTime);
            numberOfUpdate++;
            //yield return null;
            yield return new WaitForFixedUpdate();
        }

        //Debug.Log("finishAddingIn: " + finishAddingIn +  " seconds, perUpdatesAdd: " + perUpdateAdd + "numberOfUpdate: " + numberOfUpdate);
        timer.Stop();
        //Debug.Log(timer.Elapsed + " | " + timer.ElapsedMilliseconds);
    }

    void DamageShark(GameObject targetHitName, Vector3 pointHitPosition)
    {
        targetHitName.GetComponent<AI>().currHealth -= bulletDamage;
        Instantiate(VFX_HitShark, pointHitPosition, targetHitName.transform.rotation);
    }

    void DamageProps(GameObject targetHitName, Vector3 pointHitPosition)
    {
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, pointHitPosition.normalized);
        Instantiate(VFX_BulletSpark, pointHitPosition, targetHitName.transform.rotation);
        Instantiate(VFX_BulletMark, pointHitPosition, targetHitName.transform.rotation);
    }
}
