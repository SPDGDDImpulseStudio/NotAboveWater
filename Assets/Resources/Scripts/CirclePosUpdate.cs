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
        TurnOff();
    }

    public Vector2 offSet;
    public GameObject _ref;
    Camera cam;


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
            yield return null;
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
    }

    public void TurnOff()
    {
        onClick.RemoveListener(DestroySelf);
        _ref = null;
        PoolManager.Instance.EnqCircle(this);
        this.gameObject.SetActive(false);
    }

    public void CheckUI()
    {
        Debug.Log(this.gameObject.name);
    }
}
