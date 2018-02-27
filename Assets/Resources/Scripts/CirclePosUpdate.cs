﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CirclePosUpdate : MonoBehaviour , PoolObject, 
    IPointerEnterHandler ,
    IPointerDownHandler  
{

    Vector3 originScale;
    Vector2 newScale;
    public Vector2 offSet;
    public GameObject _ref;
    Camera cam;
    bool bootUp = false;
    public bool bulletCheck = false;
    public List <Text> healthNumber = new List<Text>();

    public Image circleImage;
    Collider refCollider;
    public int health = 0;
    #region Coroutines

    IEnumerator RotateBoi()
    {
        Vector3 newRot = circleImage.transform.localEulerAngles;

        while (circleImage.transform.localEulerAngles.z < 180)
        {
            if (_ref == null) break;
            newRot.z += 15 * Time.deltaTime; ;
            circleImage.transform.localEulerAngles = newRot;

            yield return null;
        }
        circleImage.transform.localEulerAngles = Vector3.zero;
    }
    IEnumerator ScaleDown()
    {
        Vector3 theScale = circleImage.transform.localScale;
        int Rnd = UnityEngine.Random.Range(100,200);

        while (true)
        {
            if (_ref == null) break;
            if (circleImage.transform.localScale.x < 0.75f)
            {
                theScale.x -= Rnd * Time.deltaTime;
                theScale.y -= Rnd * Time.deltaTime;

            }else if (circleImage.transform.localScale.x > 1.25f)
            {
                theScale.x += Rnd * Time.deltaTime;
                theScale.y += Rnd * Time.deltaTime;
            }
            circleImage.transform.localScale = theScale;

            yield return new WaitForFixedUpdate();
        }

    }

    IEnumerator PosUpdate()
    {
        float timerNow =0f, timer =3f;
        Vector3 posOnScreen;
        yield return new WaitUntil(() => _ref.activeInHierarchy);
        while (true)
        {
            //if()
             healthNumber[0].text = health.ToString();

            if (_ref == null ||!_ref.activeInHierarchy)
            {
                TurnOff();
          
                break;
            }
            posOnScreen = cam.WorldToScreenPoint(_ref.transform.position);


            //posOnScreen.x = (0 < posOnScreen.x && posOnScreen.x < Screen.width) ? posOnScreen.x :
            //   (posOnScreen.x < 0) ? 0 : Screen.width;

            //posOnScreen.y = (0 < posOnScreen.y && posOnScreen.y < Screen.height) ? posOnScreen.y :
            //    (posOnScreen.y < 0) ? 0 : Screen.height;


            if (0 > posOnScreen.x || posOnScreen.x > Screen.width || 0 > posOnScreen.y || posOnScreen.y > Screen.height)
            {
                timerNow += Time.deltaTime;
                if (timerNow > timer)
                {
                    TurnOff();
                    break;
                }
            }
            else timerNow = 0f;

            
            if ((posOnScreen.x + offSet.x) < Screen.width && (posOnScreen.x + offSet.x) > 0) posOnScreen.x += offSet.x;

            if ((posOnScreen.y + offSet.y) < Screen.height && (posOnScreen.y + offSet.y) > 0) posOnScreen.y += offSet.y;

            this.transform.position = posOnScreen;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator DestroyAft90()
    {
        while ((Vector3.Distance(thisPos, _ref.transform.position) < 130f)&& _ref)
        {
            if (_ref == null) break;
            yield return null;
        }
        Debug.Log("OUT");
        TurnOff();
    } 
    #endregion

     void Start()
    {
        BootUp();
    }
    public void HealthDown()
    {
        health--;
    }
    void BootUp()
    {
        if (bootUp) return;
        bootUp = true;
        cam = Camera.main;
        circleImage = GetComponentInChildren<Image>();
        originScale = circleImage.transform.localScale;
        
        newScale.x = originScale.x * 2;
        newScale.y = originScale.y / 2;
    }
  
    public void Init()
    {
        //Debug.Log(")
        BootUp();
        cam = Camera.main;
    }

    bool useCollider;
    public void Init_(GameObject obj , bool _useCollider)
    { 
        this.gameObject.SetActive(true);
       
        _ref = obj;
        useCollider = _useCollider;

        health = UnityEngine.Random.Range(2, 3);
        thisPos = _ref.transform.position;
        Init();
        StartCoroutine(PosUpdate());
        StartCoroutine(ScaleDown());
        StartCoroutine(RotateBoi());
        StartCoroutine(DestroyAft90());
        StartCoroutine(Pop());
    }
    IEnumerator Pop()
    {
        yield return new WaitUntil(() =>  onHit);
        //Debug.Log("Pop");
        //afterPop();
        //TurnOff();
    }
    Vector3 thisPos;
   
    public void TurnOff()
    {
        BootUp();
        if(_ref != null)
        Debug.Log("TurnOff "+ _ref.name + " of circle "+ gameObject.name);
        _ref = null;
        useCollider = false;

        bulletCheck = false;
        circleImage.transform.localScale = originScale;
        thisPos = Vector3.zero; 
        onHit = false;
        Debug.Log("TURNOFF");
        //this.onClick.RemoveAllListeners(); 
        if (afterPop != null)
        afterPop();

        afterPop = null;
        this.gameObject.SetActive(false);

       
    }
  
    public  void OnPointerEnter(PointerEventData eventData)
    {
        //if(Player.Instance.fuckW)
        //Debug.Log(Player.Instance.fuckW);
        //Debug.Log(eventData);
        if (Player.Instance.currBullet> 0 && (Player.Instance.shootTimerNow> Player.Instance.shootEvery) && Input.GetMouseButton(0))
        {
            //OnHit();
        }
    }
    void OnHit()
    {
        if (!useCollider) {
            if (Player.Instance.currBullet > 0 && Input.GetMouseButtonDown(0))
            {
                health--;
                if (health == 0)
                {
                    if (!bulletCheck)
                        _ref.GetComponentInParent<Tentacle>().OnHit();
                    else
                        _ref.GetComponent<BulletScript>().TurnOff();

                    
                    TurnOff();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Player.Instance.currBullet> 0 && (Player.Instance.shootTimerNow > Player.Instance.shootEvery) && Input.GetMouseButton(0))
        {
            OnHit();
        }
    }

    
    public bool onHit;
    public Action afterPop;
}
