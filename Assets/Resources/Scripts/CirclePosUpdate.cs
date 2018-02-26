using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CirclePosUpdate : Button , PoolObject, 
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
        Debug.Log("END");
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
        while (true)
        {
            //if()
             healthNumber[0].text = health.ToString();
            
            if (_ref == null) break;

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

    protected override void Start()
    {
        base.Start();
        BootUp();
        this.onClick.AddListener(OnHit);
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
    public void Init_(GameObject obj , Collider col)
    { 
        this.gameObject.SetActive(true);
       
        _ref = obj;
        refCollider = col;

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
   
    public  void TurnOff()
    {
        BootUp();
        _ref = null;
        refCollider = null;

        bulletCheck = false;
        circleImage.transform.localScale = originScale;
        thisPos = Vector3.zero;
        afterPop = null;
        onHit = false;
        //this.onClick.RemoveAllListeners();
        this.gameObject.SetActive(false);

    }

    public void CheckUI()
    {
        Debug.Log(this.gameObject.name);
    }

  
    public override void OnPointerEnter(PointerEventData eventData)
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
        Debug.Log("PR");
        if (Player.Instance.currBullet > 0 && Input.GetMouseButton(0))
        {
            health--;
            if (health == 0)
            {
                if (!bulletCheck)
                    _ref.GetComponentInParent<Tentacle>().OnHit();
                else
                    _ref.GetComponent<BulletScript>().TurnOff();


                Debug.Log(bulletCheck);
                TurnOff();
            }

            Debug.Log("PR");
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        //eventData.p
        //Debug.Log(eventData);   
        //if (Player.Instance.fuckW)
        //Debug.Log(Player.Instance.fuckW);
        if (Player.Instance.currBullet> 0 && (Player.Instance.shootTimerNow > Player.Instance.shootEvery) && Input.GetMouseButton(0))
        {
            //OnHit();
        }
    }

    
    public bool onHit;
    public Action afterPop;
}

public interface CircleAttached
{
     void ToCircle();

    
}
