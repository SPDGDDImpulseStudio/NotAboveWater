using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePosUpdate : MonoBehaviour
{
    void Start()
    {
        cam = Camera.main;
        this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(DestroySelf);
    }

    public GameObject _ref;
    Camera cam;

    public Vector2 offSet;
    
    // Update is called once per frame
    void Update()
    {
        if (_ref)
        {
            Vector3 posOnScreen = cam.WorldToScreenPoint(_ref.transform.position);
        
            //Debug.Log(posOnScreen);
            posOnScreen.x = (0 < posOnScreen.x && posOnScreen.x < Screen.width) ? posOnScreen.x :
               (posOnScreen.x < 0) ? 0 : Screen.width;

            posOnScreen.y = (0 < posOnScreen.y && posOnScreen.y < Screen.height) ? posOnScreen.y :
                (posOnScreen.y < 0) ? 0 : Screen.height;

            //Debug.Log(posOnScreen);
            //if (posOnScreen.x > Screen.width)
            //    posOnScreen.x = Screen.width;
            //else if (posOnScreen.x < 0)
            //    posOnScreen.x = 0;
            //if (posOnScreen.y > Screen.height)
            //    posOnScreen.y = Screen.height;
            //else if (posOnScreen.y < 0)
            //    posOnScreen.y = 0;

            posOnScreen.x += offSet.x;
            posOnScreen.y += offSet.y;

            this.transform.position = posOnScreen;

        }
    }

    void DestroySelf()
    {   CircleManager.Instance.RemoveThisBut(this.GetComponent<UnityEngine.UI.Button>());
        Destroy(this.gameObject);
    }
    
}
