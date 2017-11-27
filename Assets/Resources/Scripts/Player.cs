using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public AudioSource audioSource;
    public AudioClip Gunshot;
    public Slider slider;
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

    public float bulletDamage, currHealth, maxHealth;

    public bool allowToShoot = false;

    GameObject targetHit;
    public Vector3 pointHit;

    public GameObject VFX_HitShark;
    public GameObject VFX_BulletMark;
    public GameObject VFX_BulletSpark;

    public float maxSuitHealth, currSuitHealth, maxOxygen, currOxygen;

    public float shootEvery = 1, shootTimerNow;

    void Start () {
        if (Instance.GetInstanceID() != this.GetInstanceID())
            Destroy(this.gameObject);

        currHealth = maxHealth;
        slider = FindObjectOfType<Canvas>().GetComponentInChildren<Slider>();
        audioSource = GetComponent<AudioSource>() ? GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();

        currSuitHealth = maxSuitHealth;
        currOxygen = maxOxygen;

    }

    void Update()
    {
        slider.value = currHealth / maxHealth;
        if (shootTimerNow < shootEvery)
            shootTimerNow += Time.deltaTime;
        if (allowToShoot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(point.origin, point.direction);
                //Debug.DrawRay(transform.position, point,Color.green);
                if (Physics.Raycast(this.transform.position, point.direction, out hit))
                {

                    targetHit = hit.transform.gameObject;
                    pointHit = hit.point;

                    if (hit.transform.GetComponent<Book>())
                    {
                        Debug.Log(hit.transform.GetComponent<Book>().bookSlotInfo.bookSlotPos + " " + hit.transform.GetComponent<Book>().ReturnSlot(hit.transform.position).bookSlotPos);
                    }
                    else
                    {
                        if (shootTimerNow > shootEvery)
                        {
                            shootTimerNow = 0;

                            audioSource.clip = Gunshot;
                            audioSource.Play();

                            if (targetHit.GetComponent<AI>())
                                DamageShark(targetHit, pointHit);
                            else if (targetHit.GetComponent<InteractableObj>())                                   //Temporarily for detecting walls and etc (not shark). will update for detecting more precise name e.g tags 
                                targetHit.GetComponent<InteractableObj>().Interact();
                            else
                                DamageProps(targetHit, pointHit);
                        }
                        Debug.Log(hit.transform.name);
                    }
                    //bookShelf.GetSpecifyBook(hit.transform);
                }

            }
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
        float toAdd = x;

        while(toAdd > 0)
        {
            toAdd -= Time.deltaTime;
            currOxygen = ((Time.deltaTime + currOxygen) < maxOxygen) ? currOxygen + Time.deltaTime : maxOxygen;
            yield return null;
        }
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
