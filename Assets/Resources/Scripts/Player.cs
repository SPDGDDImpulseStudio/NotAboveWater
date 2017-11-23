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
    
    public GameObject VFX_Hit;

    public float shootEvery = 1, shootTimerNow;
    void Start () {
        if (Instance.GetInstanceID() != this.GetInstanceID())
            Destroy(this.gameObject);

        currHealth = maxHealth;
        slider = FindObjectOfType<Canvas>().GetComponentInChildren<Slider>();
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

                    Instantiate(VFX_Hit, pointHit, targetHit.transform.rotation);
                    ParticleSystem parts = VFX_Hit.GetComponent<ParticleSystem>();
                    //Destroy(parts, .5f);

                    audioSource = GetComponent<AudioSource>();

                    audioSource.clip = Gunshot;
                    audioSource.Play();

                    if (hit.transform.GetComponent<Book>())
                    {
                        Debug.Log(hit.transform.GetComponent<Book>().bookSlotInfo.bookSlotPos + " " + hit.transform.GetComponent<Book>().ReturnSlot(hit.transform.position).bookSlotPos);
                    }
                    else
                    {
                        if (shootTimerNow > shootEvery)
                        {
                            shootTimerNow = 0;
                            if (hit.transform.GetComponent<AI>())
                                hit.transform.GetComponent<AI>().currHealth -= bulletDamage;
                        }
                        Debug.Log(hit.transform.name);
                    }
                    //bookShelf.GetSpecifyBook(hit.transform);
                }

            }
        }
    }
}
