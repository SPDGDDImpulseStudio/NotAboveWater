using System.Collections;
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
    bool bootUp = false;
    public bool bulletCheck = false;
    #region Coroutines

    IEnumerator RotateBoi()
    {
        Vector3 newRot = this.transform.localEulerAngles;

        while (this.transform.localEulerAngles.z < 180)
        {
            if (_ref == null) break;
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

            if (_ref == null) break;
            theScale.x -= Rnd * Time.deltaTime / 100;
            theScale.y -= Rnd * Time.deltaTime / 100;

            this.transform.localScale = theScale;
            yield return new WaitForFixedUpdate();
        }

    }
    IEnumerator PosUpdate()
    {
        Vector3 posOnScreen;
        while (true)
        {
            //if()
            if (_ref == null) break;

            posOnScreen = cam.WorldToScreenPoint(_ref.transform.position);


            //posOnScreen.x = (0 < posOnScreen.x && posOnScreen.x < Screen.width) ? posOnScreen.x :
            //   (posOnScreen.x < 0) ? 0 : Screen.width;

            //posOnScreen.y = (0 < posOnScreen.y && posOnScreen.y < Screen.height) ? posOnScreen.y :
            //    (posOnScreen.y < 0) ? 0 : Screen.height;


            if (0 > posOnScreen.x || posOnScreen.x > Screen.width || 0> posOnScreen.y || posOnScreen.y > Screen.height)
            {
                Debug.Log("OUT");
                TurnOff();
                break;
            }
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

    void BootUp()
    {
        if (bootUp) return;
        bootUp = true;
        cam = Camera.main;
        originScale = this.transform.localScale;
        newScale.x = originScale.x * 2;
        newScale.y = originScale.y / 2;
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
        BootUp();
        cam = Camera.main;
    }
    public void Init_(GameObject obj)
    { 
        this.gameObject.SetActive(true);
        _ref = obj;
        thisPos = _ref.transform.position;
        Init();
        StartCoroutine(PosUpdate());
        StartCoroutine(ScaleDown());
        StartCoroutine(RotateBoi());
        StartCoroutine(DestroyAft90());
    }
    Vector3 thisPos;
   
    public override void TurnOff()
    {
        BootUp();
        _ref = null;
        bulletCheck = false;
        this.transform.localScale = originScale;
        thisPos = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    public void CheckUI()
    {
        Debug.Log(this.gameObject.name);
    }

  
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Player.Instance.currBullet> 0 && Input.GetMouseButton(0))
        {
            if (!bulletCheck)
                _ref.GetComponentInParent<Tentacle>().OnHit();
            else
                _ref.GetComponent<BulletScript>().TurnOff();
            TurnOff();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Player.Instance.currBullet> 0 && Input.GetMouseButton(0))
        {

            if (!bulletCheck)

                _ref.GetComponentInParent<Tentacle>().OnHit();
            else
                _ref.GetComponent<BulletScript>().TurnOff();
            TurnOff();
        }
    }

}
