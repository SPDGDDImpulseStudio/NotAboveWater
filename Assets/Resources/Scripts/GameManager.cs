using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    #region Working Variables

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject newGO = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameManager"));
                    _instance = newGO.GetComponent<GameManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            if (!_instance.gameObject.activeSelf) _instance.gameObject.SetActive(true);
            return _instance;
        }
    }

    static GameManager _instance;

    public Camera playerCam;
    public List<GameObject> waypoints = new List<GameObject>();
    public int currInt = 0;
    public float speed = 5;
    public bool updating = false;


    #endregion

    //public delegate void Process();
    //public delegate string MsgToTypeIn(string msg);
    //List<MsgToTypeIn> msgs = new List<MsgToTypeIn>();

    public List<EventsInterface> eventsList = new List<EventsInterface>();

    public EventsInterface currEvent;
    
    public InputField field;

    [Header("[Editor]")]
    public float sizeOfSphere =3;
    public Color colorOfLines = Color.black;
    public bool showTrack = false;
    
    public Action UpdateGame;

    public IEnumerator<Action> IAction;

    delegate void EventUpdate(string x);

    EventUpdate _eventUpdate;

    public Action AIEnum;

    void Start() {
        if (Instance.GetInstanceID() != this.GetInstanceID())
        {
            Destroy(gameObject);
            return;
        }
        playerCam = Camera.main;
        field = FindObjectOfType<InputField>();
        //StartCoroutine(NextEvent());
        _eventUpdate = new EventUpdate(eventsList[currInt].Test);
        Debug.Log(_eventUpdate);
        Debug.Log(eventsList.Count);
        //_eventUpdate();
        //_eventUpdate += eventsList[currInt].EnumEvent;
    }


    public IEnumerator NextEvent()
    {
        //if (currEvent == eventsList[eventsList.Count - 1])
        if(currInt == eventsList.Count)
        {
            Debug.Log("END");
            currEvent = null;
            yield break;
        }
        Debug.Log("currInt =" + currInt);
        //UpdateGame += eventsList[currInt].CallEvent;
        //Adding the method into Anonymous Method
        StartCoroutine(eventsList[currInt].EnumEvent());

        currEvent = eventsList[currInt];

        eventsList[currInt].runningOfEvent = true;
        //Turns the bool i check into true;

        //UpdateGame();
        //Calls the anonymous Method

        yield return new WaitUntil(() => !eventsList[currInt].runningOfEvent);

        //UpdateGame -= eventsList[currInt].CallEvent;
        currInt++;

        //Wait till it finish running 
        StartCoroutine(NextEvent());
            
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !showTrack)
        {
            StartCoroutine(NextEvent());
            showTrack = true;
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!showTrack)
            return;
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.color = colorOfLines;

            Gizmos.DrawWireSphere(waypoints[i].transform.position, sizeOfSphere);
            if (i != waypoints.Count && i != waypoints.Count-1) Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                
            Gizmos.color = Color.red;
            Ray ray = new Ray(waypoints[i].transform.position, (transform.forward - waypoints[i].transform.position));
            Gizmos.DrawRay(ray);
        }
    }
}
#endif 

