using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraManager : MonoBehaviour {


    public float speed = 5;

    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraManager>();
                if (_instance == null)
                {
                    GameObject newGO = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/CameraManager"));
                    _instance = newGO.GetComponent<CameraManager>();
                }
                if (_instance == null)
                    Debug.LogError("STOP");
                DontDestroyOnLoad(_instance.gameObject);
            }
            if (!_instance.gameObject.activeSelf) _instance.gameObject.SetActive(true);
            return _instance;
        }
    }

    static CameraManager _instance;

    public Camera playerCam;

    BookShelf bookShelf;

    public bool movementBool = false;
    // Use this for initialization
    void Start () {
        playerCam = Camera.main;
        bookShelf = FindObjectOfType<BookShelf>();
	}

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        //Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(point.origin, point.direction);
        ////Debug.DrawRay(transform.position, point,Color.green);
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Physics.Raycast(this.transform.position, point.direction, out hit))
        //    {
        //        if (hit.transform.GetComponent<Book>())
        //            Debug.Log(hit.transform.GetComponent<Book>().bookSlotInfo.bookSlotPos + " "+ hit.transform.GetComponent<Book>().ReturnSlot(hit.transform.position).bookSlotPos);
        //        else
        //            Debug.Log(hit.transform.name);
        //        //bookShelf.GetSpecifyBook(hit.transform);
        //    }

        //}
        

    }
    public void UpdateCameraPos()
    {

        //StartCoroutine(UpdateCamera());
    }

    public  IEnumerator UpdateCamera()
    {
        if (GameManager.Instance.updating) yield break;
        if (GameManager.Instance.currInt == GameManager.Instance.waypoints.Count - 1) { Debug.Log("END"); yield break; }

        GameManager.Instance.currInt++;

        Transform currTarget = GameManager.Instance.waypoints[GameManager.Instance.currInt].transform;
        StartCoroutine(UpdateBool(currTarget));

        while (true)
        //while (currTarget.gameObject.activeInHierarchy)
        {
            playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, currTarget.position, speed * Time.deltaTime);
            playerCam.transform.rotation = Quaternion.Lerp(playerCam.transform.rotation, currTarget.rotation, speed * Time.deltaTime);
            yield return null;
        }
    }




    #region Predefined Functions

    public void WalkTo(Vector3 _walkTo, float speed)
    {
        StartCoroutine(Walk(_walkTo, speed));
    }

    public IEnumerator Walk(Vector3 walkTo, float speed)
    {
        movementBool = true;
        while (this.transform.position != walkTo)
        {
            this.transform.position = Vector3.Lerp(transform.position, walkTo, speed * Time.deltaTime);
            yield return null;
        }
        movementBool = false;
    } 

    public void Wait(float duration)
    {
        StartCoroutine(WaitFor(duration));
    }

    public IEnumerator WaitFor(float _duration)
    {
        Debug.Log("Wait for" + _duration + " seconds.");
        yield return new WaitForSeconds(_duration);
    }
    #endregion

    IEnumerator UpdateBool(Transform _target)
    {
        GameManager.Instance.updating = true;
        //while (_target.gameObject.activeInHierarchy)
        //{

        //    yield return null;
        //}
        yield return new WaitUntil(() => !_target.gameObject.activeInHierarchy);
        GameManager.Instance.updating = false;

    }
}
