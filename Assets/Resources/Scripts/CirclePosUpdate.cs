﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CirclePosUpdate : PoolObject, 
    IPointerEnterHandler ,
    IPointerDownHandler  
 
{

    Vector3 originScale;
    Vector2 newScale;
    public Vector2 offSet;
    public GameObject _ref;
    Camera cam;

    void Start()
    {
     
        TurnOff();
    }
    
    IEnumerator RotateBoi()
    {
        Vector3 newRot = this.transform.localEulerAngles;

        while (this.transform.localEulerAngles.z < 180)
        {
            newRot.z += 15 * Time.deltaTime; ;
            this.transform.localEulerAngles = newRot;

            yield return null;
        }
        Debug.Log("END");
        this.transform.localEulerAngles = Vector3.zero;
    }
    IEnumerator ScaleDown()
    {
        Vector3 theScale = this.transform.localScale;
        int Rnd = UnityEngine.Random.Range(1, 5);
        while (this.transform.localScale.x > 0.013f)
        {

            theScale.x -= Rnd * Time.deltaTime / 100;
            theScale.y -= Rnd * Time.deltaTime / 100;
            this.transform.localScale = theScale;
            yield return new WaitForFixedUpdate();
        }
      
    }
    IEnumerator PosUpdate()
    {
        while (true)
        {
            Vector3 posOnScreen = cam.WorldToScreenPoint(_ref.transform.position);
            
            posOnScreen.x = (0 < posOnScreen.x && posOnScreen.x < Screen.width) ? posOnScreen.x :
               (posOnScreen.x < 0) ? 0 : Screen.width;

            posOnScreen.y = (0 < posOnScreen.y && posOnScreen.y < Screen.height) ? posOnScreen.y :
                (posOnScreen.y < 0) ? 0 : Screen.height;


            if((posOnScreen.x + offSet.x) < Screen.width && (posOnScreen.x + offSet.x)> 0)  posOnScreen.x += offSet.x;

            if((posOnScreen.y + offSet.y) < Screen.height&& (posOnScreen.y + offSet.y)> 0)  posOnScreen.y += offSet.y;

            this.transform.position = posOnScreen;
            yield return new WaitForFixedUpdate();
        }
    }

    void DestroySelf()
    {
        if (Player.Instance.currBullet != 0)
        {
            CircleManager.Instance.RemoveThisBut(this);
            TurnOff();
        }
    }

    public override void Init()
    {
        //Debug.Log(")
        cam = Camera.main;
        originScale = this.transform.localScale;
        newScale.x = originScale.x * 2;
        newScale.y = originScale.y / 2;
    }
    public void Init_(GameObject obj)
    { 
        this.gameObject.SetActive(true);
        _ref = obj;
        Init();
        StartCoroutine(PosUpdate());
        StartCoroutine(ScaleDown());
        StartCoroutine(RotateBoi());
    }

    public override void TurnOff()
    {
        _ref = null;
        PoolManager.Instance.EnqCircle(this);
        this.transform.localScale = originScale;
        this.gameObject.SetActive(false);
    }

    public void CheckUI()
    {
        Debug.Log(this.gameObject.name);
    }

  
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Player.Instance.currBullet != 0 && Input.GetMouseButton(0))
        {
            _ref.GetComponentInParent<Tentacle>().OnHit();
            TurnOff();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Player.Instance.currBullet != 0 && Input.GetMouseButton(0))
        {
            _ref.GetComponentInParent<Tentacle>().OnHit();
            TurnOff();
        }
    }

}
