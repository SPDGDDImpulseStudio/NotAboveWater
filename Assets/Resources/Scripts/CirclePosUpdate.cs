using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CirclePosUpdate : Button
{
    protected override void Start()
    {
        base.Start();
        cam = Camera.main;

        if (Application.isPlaying)
        {
            originScale = this.transform.localScale;
            newScale.x = originScale.x * 2;
            newScale.y = originScale.y / 2;
            TurnOff();
        }
    }
    Vector3 originScale;
    Vector2 newScale;
    public Vector2 offSet;
    public GameObject _ref;
    Camera cam;

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
        int Rnd = Random.Range(1, 5);
        while (this.transform.localScale.x > 0.01f)
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

    public void Init(GameObject obj)
    {
        this.gameObject.SetActive(true);
        onClick.AddListener(DestroySelf);
        _ref = obj;
        StartCoroutine(PosUpdate());
        StartCoroutine(ScaleDown());
        StartCoroutine(RotateBoi());
    }

    public void TurnOff()
    {
        onClick.RemoveListener(DestroySelf);
        _ref = null;
        PoolManager.Instance.EnqCircle(this);
        this.transform.localScale = originScale;
        this.gameObject.SetActive(false);

    }

    public void CheckUI()
    {
        Debug.Log(this.gameObject.name);
    }
}
