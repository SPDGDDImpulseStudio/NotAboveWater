using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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


    public float shootEvery = 1, shootTimerNow;
    void Start () {
        if (Instance.GetInstanceID() != this.GetInstanceID())
            Destroy(this.gameObject);

        currHealth = maxHealth;
	}

    void Update()
    {
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
                        } Debug.Log(hit.transform.name);
                    }
                    //bookShelf.GetSpecifyBook(hit.transform);
                }

            }
        }
    }
}
