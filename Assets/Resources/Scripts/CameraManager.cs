using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraManager : MonoBehaviour {


    public float speed = 5;

    [ContextMenu("Do Something")]
    void DoSomething()
    {
        Debug.Log("Perform operation");
    }

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
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
            ShakeCamera();
#endif
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
        while (this.transform.position != walkTo)
        {
            this.transform.position = Vector3.Lerp(transform.position, walkTo, speed * Time.deltaTime);
            yield return null;
        }
    } 

    public void Wait(float duration)
    {
        StartCoroutine(WaitFor(duration));
    }

    public IEnumerator WaitFor(float _duration)
    {
        yield return new WaitForSeconds(_duration);
    }
    #endregion


    Vector3 originPos;

    public void ShakeCamera()
    {
        StartCoroutine(Shakey());
    }
    bool shakeyBoi = false;

    IEnumerator Shakey()
    {
        if (shakeyBoi) yield break;
        shakeyBoi = true;
        float currTime = 0, timer = 5;
        originPos = this.transform.localPosition;
        float shakeAmount = 0.7f;
        while (currTime < timer)
        {
            currTime += Time.deltaTime;
            this.transform.localPosition = originPos + UnityEngine.Random.insideUnitSphere * shakeAmount;

            yield return null;
        }
        shakeyBoi = false;
    }
    public void LookAtWayp(Transform x, out bool z)
    {
        z = waitRotate;
        StartCoroutine(LookAtTarget(x));
    }
    bool waitRotate = true;
    IEnumerator LookAtTarget(Transform target)
    {
        //if (waitRotate) yield break;
        waitRotate = true;
        float angle = 15;
        while (!(Vector3.Angle(transform.forward, (target.position - transform.position).normalized) < angle))
        {
            Vector3 dir = (target.position - transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(dir);

            transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, 14 * Time.deltaTime);
            Debug.Log(angle);
            Debug.DrawRay(transform.position, target.position - transform.position);
            yield return null;
        }
        waitRotate = false;
    }

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
    public Transform target;

    [Tooltip("Lower The Value, the faster.")]
    public float maxDistDel = 3;
    Vector3 velocity = Vector3.zero;
    void LateUpdate()
    {
        if (!target) return;
        //transform.LookAt(target);
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, maxDistDel);
    }

    public void RotationCam(Transform target, float rotationSpeed =14)
    {
        Vector3 dir = (target.position - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(dir);

       transform.localRotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
       Debug.DrawRay(transform.position, (target.position - transform.position));

    }
    //public Transform target; // Target following
    //public float distance = 3.0f; // Target distance along x-z plane
    //public float height = 1.0f; // Camera height above target
    //public float heightDamping = 2.0f;
    //public float rotationDamping = 3.0f;

    //void LateUpdate()
    //{
    //    if (!target)
    //        return;

    //    // Calculate current rotation angles
    //    float wantedRotationAngle = target.eulerAngles.y;
    //    float wantedHeight = target.position.y + height;

    //    float currentRotationAngle = transform.eulerAngles.y;
    //    float currentHeight = transform.position.y;

    //    // Dampen rotation about the y-axis
    //    currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

    //    // Dampen height
    //    currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

    //    // Convert angle into rotation
    //    Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

    //    // Set position of camera on x-z plane to distance behind target
    //    transform.position = target.position;
    //    transform.position -= currentRotation * Vector3.forward * distance;

    //    // Set camera height
    //    Vector3 temp = transform.position;
    //    temp.y = currentHeight;
    //    transform.position = temp;

    //    // Look at target
    //    transform.LookAt(target);
    //}

}





