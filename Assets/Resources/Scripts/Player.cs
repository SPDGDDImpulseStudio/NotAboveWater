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
    public AudioClip gunFire;

    [Header("[Player's Attributes]")]
    [Header("Things to Set")]
    public int maxBullet = 10;
    public float maxSuitHealth, maxOxygen, maxHealth,
        oxyDrop, healthDrop,
        reloadTime = 3, bulletDamage, shootEvery = 1;

    [Header("Dont+Touch+For+Debug+Purpose")]
    public int  currBullet;
    public float currSuitHealth, currOxygen, currHealth, shootTimerNow;
    
    float healthBarCount1, healthBarCount2;
    public bool allowToShoot = false;

    #endregion

    void Start () {
        if (Instance.GetInstanceID() != this.GetInstanceID())  Destroy(this.gameObject);

        Init();
    }

    public void Init()
    {
        currHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        currSuitHealth = maxSuitHealth;
        currOxygen = maxOxygen;
        currBullet = maxBullet;

        StartCoroutine(UIUpdate());
        StartCoroutine(OxyDropping());
        StartCoroutine(SuittedUp());
    }

    void Update()
    {
        if (shootTimerNow < shootEvery)
            shootTimerNow += Time.deltaTime;


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(point.origin, point.direction);
            //Debug.DrawRay(transform.position, point,Color.green);
            if (allowToShoot)
            {
                if (shootTimerNow > shootEvery && currBullet != 0)
                {
                    shootTimerNow = 0;
                   
                    audioSource.clip = gunFire;
                    audioSource.Play();
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
                        else
                            DamageProps(targetHit, pointHit);

                    }
                    if (currBullet - 1 == 0)
                        StartCoroutine(WorkAroundButton());
                    else
                    currBullet--;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        
            StartCoroutine(Reload());
        
    }
    public IEnumerator WorkAroundButton()
    {
        yield return new WaitForSeconds(shootEvery);
        currBullet--;
    }
    bool reloading = false, suitUp = true;

    public IEnumerator Reload()
    {
        if (reloading) yield break; 
        reloading = true; 
        yield return new WaitForSeconds(reloadTime);
        currBullet = maxBullet;
       
        reloading = false;
        gunShootingBar.maxValue = 1;

    }

    public IEnumerator OxyDropping()
    {
        while (true)
        {
            if (currOxygen < 0) {StartCoroutine(HealthDropping()); yield break; }
            currOxygen -= oxyDrop;
            yield return null;
        }
    }
    bool healthDrop_ = false;
    public IEnumerator HealthDropping()
    {
        if (healthDrop_) yield break;
        healthDrop_ = true;
        while (true)
        {
            if (suitUp && currOxygen > 0) { healthDrop_ = false; yield break; }
            currHealth -= healthDrop;
            yield return null;
        }
    }
    public IEnumerator SuittedUp()
    {
        yield return new WaitUntil(() => currSuitHealth >= 0);
        suitUp = true;
        healthBarCount1 = currSuitHealth;
        healthBarCount2 = maxSuitHealth;
        //Here i can set the image for UI too
        StartCoroutine(SuitDown());
    }
    public IEnumerator SuitDown()
    {
        yield return new WaitUntil(() => currSuitHealth <= 0);

        suitUp = false;
        StartCoroutine(HealthDropping());
        healthBarCount1 = currHealth;
        healthBarCount2 = maxHealth;
        //Here i can set the image for UI too
        StartCoroutine(SuittedUp());
    }
    IEnumerator UIUpdate()
    {
        while (true)
        {
            healthBar.value = healthBarCount1 / healthBarCount2;
            oxygenBar.value = currOxygen / maxOxygen;
            if (currBullet != 0)
                gunShootingBar.value = shootTimerNow / shootEvery;
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
