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

    public List<GameObject> AIWaypoints = new List<GameObject>();

    public List<EventsInterface> eventsList = new List<EventsInterface>();

    public EventsInterface currEvent;
        
    public Action UpdateGame;

    public IEnumerator<Action> IAction;

    delegate void EventUpdate(string x);

    EventUpdate _eventUpdate;

    public Action AIEnum;

    public bool forceStop = false;

    //Leaderboard's 

    public InputField saveName;

    public const string leaderboard = "LEADERBOARD";

    public static float playerScore = 0;

    //Editor's
    [Header("[Editor]")]
    public float sizeOfSphere =3;
    public Color colorOfLines = Color.black;
    public bool showTrack = false;


    string TimeScaleNow;    
    int TimeScaleInt = 1;
    public Rect ButPos;

    public void RecordScore(string _name)
    {
        Debug.Log("Name= " + _name);
        for(int i = 0; i < 5; i ++)
        {
            if (PlayerPrefs.HasKey(leaderboard + i.ToString()))
            {
                if (PlayerPrefs.GetFloat(leaderboard + i.ToString()) > playerScore)   continue;  

                else if (PlayerPrefs.GetFloat(leaderboard + i.ToString()) == playerScore)
                {

                }
                else
                {
                    //my new scores is higher than i's stored scores

                    string tempName = PlayerPrefs.GetString(leaderboard + i.ToString());
                    float tempScore = PlayerPrefs.GetFloat(leaderboard + i.ToString());
                    Debug.Log("tempName =" + tempName);
                    PlayerPrefs.SetFloat(leaderboard + i.ToString(), playerScore);
                    PlayerPrefs.SetString(leaderboard + i.ToString(), _name);
                    PlayerPrefs.Save();

                    //Now i proceed to shift all the others by 1

                    if (i== 4) break;
                    //this is the last one
                  

                    else
                    {
                        int nextSpot = i+ 1;

                        string _tempName = PlayerPrefs.GetString(leaderboard + nextSpot);
                        float _tempScore = PlayerPrefs.GetFloat(leaderboard + nextSpot);
                        for (int j = nextSpot; j < 5; j++)
                        {
                            
                            PlayerPrefs.SetString(leaderboard + j.ToString(), tempName);
                            PlayerPrefs.SetFloat(leaderboard + j.ToString(), tempScore);

                            tempScore = _tempScore;
                            tempName = _tempName;
                            _tempName = PlayerPrefs.GetString(leaderboard + (j.ToString() + 1));
                            _tempScore = PlayerPrefs.GetFloat(leaderboard + (j.ToString() + 1));
                            PlayerPrefs.Save();

                        }
                    }
                    break;
                }
            }else{

                //byte[] bytesArray = new byte[256];
                //for (byte b = 0; b < 255; b++)
                //{
                //    bytesArray[b] = b;
                //}

                //System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
                //byte[] bytesNew = ue.GetBytes(_name);
                //string strNew = System.Convert.ToBase64String(bytesNew);

                //Debug.Log("bytes= " + bytesNew.ToString()+ " Length= " +  bytesNew.Length + " " + "string= " + strNew);
                nameToSave = _name;
                PlayerPrefs.SetString(leaderboard + i.ToString(), nameToSave);


                //string jsonName = JsonUtility.ToJson(_name);

                 //I CAN TRY USING XML INSTEAD OF THIS SHIT CAUSE FUCK THIS SHIT NOW
                Debug.Log(leaderboard + i.ToString() + " | FUCK THIS USELESS PIECE OF SHIT 1 | " + nameToSave);

                //PlayerPrefs.SetString(leaderboard + i.ToString(), _name);
                Debug.Log(leaderboard + i.ToString() + " | FUCK THIS USELESS PIECE OF SHIT 2 | " + nameToSave);
                PlayerPrefs.SetFloat(leaderboard + i.ToString(), playerScore);
                PlayerPrefs.Save();
                Debug.Log("THIS BETTER BBE FUCKED UP HERE "+PlayerPrefs.GetString(leaderboard + i.ToString()));

                break;
            }
        }
        playerScore = 0;

    }
    static string nameToSave = "";  

    void Start()
    {
        if (Instance.GetInstanceID() != this.GetInstanceID())
        {
            Destroy(gameObject);
            return;
        }
        playerCam = Camera.main;

        StartCoroutine(NextEvent());
        _eventUpdate = new EventUpdate(eventsList[currInt].Test);
        //Debug.Log(_eventUpdate);
        //Debug.Log(eventsList.Count);
        //_eventUpdate();
        //_eventUpdate += eventsList[currInt].EnumEvent;
        AIWaypoints = new List<GameObject>();
        AI ai = FindObjectOfType<AI>();
        for (int x = 0; x < ai.waypoints.Count; x++)
        {
            AIWaypoints.Add(ai.waypoints[x]);
        }

        

    }
    public void NameRecorded(string written)
    {
        Debug.Log(written);
        RecordScore(written);
        //Debug.Log(saveName.text);

        //Debug.Log(" | " + (saveName.onEndEdit.ToString()) + " | ");
        saveName.gameObject.SetActive(false);
        //Destroy(saveName);
        //Fade to next screen
    }
    public IEnumerator NextEvent()
    {
        yield return new WaitUntil(() => AI.Instance != null);
        //yield return new WaitUntil(() => AI.Instance.nav != null);
        //if (currEvent == eventsList[eventsList.Count - 1])
        if(currInt == eventsList.Count || forceStop)
        {
            Debug.Log("End of All events ");
            currEvent = null;
            //Canvas canvas = FindObjectOfType<Canvas>();
            saveName.gameObject.SetActive(true);
            saveName.onEndEdit.AddListener(NameRecorded);


            //Instantiate(saveName,canvas.transform);
            //yield return new WaitUntil(() => SaveName.text != "");
            //Debug.Log(saveName.text+ " 1 " + (saveName.onEndEdit.ToString()));
            //saveName.onEndEdit.AddListener(NameRecorded);
            //RecordScore(SaveName.onEndEdit.ToString());
            //yield return new WaitUntil(() => SaveName.onEndEdit);
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
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < 5; i++)
            {
                if (PlayerPrefs.HasKey(leaderboard + i.ToString()))
                    Debug.Log("CurrentKey: " + leaderboard + i.ToString()

                         + " Name: " + PlayerPrefs.GetString(leaderboard + i.ToString())

                        + " Score: " + PlayerPrefs.GetFloat(leaderboard + i.ToString())

                        //+" Jsoned " + JsonUtility.FromJson<string>(PlayerPrefs.GetString(leaderboard + i.ToString()))

                        );
            }
            Debug.Log(PlayerPrefs.GetString("FUCK", "XD"));
            playerScore += 50;
        }else if (Input.GetMouseButtonDown(0))
        {
            PlayerPrefs.SetString("FUCK", "FUUCK");
            PlayerPrefs.Save();
        }

        
    }
    void OnGUI()
    {
        if (GUI.Button(ButPos,TimeScaleNow))
        {
            if (TimeScaleInt != 4)
                TimeScaleInt *= 2;
            else
                TimeScaleInt = 1;

            Time.timeScale = TimeScaleInt;
            TimeScaleNow = TimeScaleInt.ToString();
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!showTrack)
            return;

        AIWaypoints = new List<GameObject>();
        AI ai = FindObjectOfType<AI>();
        for (int x = 0; x < ai.waypoints. Count; x++)
        {
            AIWaypoints.Add(ai.waypoints[x]);
        }
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.color = colorOfLines;

            Gizmos.DrawWireSphere(waypoints[i].transform.position, sizeOfSphere);
            if (i != waypoints.Count && i != waypoints.Count-1) Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position);
                
            Gizmos.color = Color.red;
            Ray ray = new Ray(waypoints[i].transform.position, (transform.forward - waypoints[i].transform.position));
            Gizmos.DrawRay(ray);
        }

        for (int j= 0; j < AIWaypoints.Count; j++)
        {
            Gizmos.color = Color.black;

            Gizmos.DrawWireSphere(AIWaypoints[j].transform.position, sizeOfSphere);
            if (j != AIWaypoints.Count && j != AIWaypoints.Count - 1)

                Gizmos.DrawLine(AIWaypoints[j].transform.position, AIWaypoints[j + 1].transform.position);

        }
    }
#endif 

}
