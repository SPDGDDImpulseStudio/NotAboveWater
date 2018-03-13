using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : ISingleton<GameManager> {

    #region Working Variables

    //public static GameManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType<GameManager>();
    //            if (_instance == null)
    //            {
    //                GameObject newGO = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameManager"));
    //                _instance = newGO.GetComponent<GameManager>();
    //            }
    //            DontDestroyOnLoad(_instance.gameObject);
    //        }
    //        if (!_instance.gameObject.activeSelf) _instance.gameObject.SetActive(true);
    //        return _instance;
    //    }
    //}

    //static GameManager _instance;

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

    public const string leaderboardName = leaderboard + "NAME";

    public const string leaderboardScore = leaderboard + "SCORE";

    public const string leaderboardAccuracy = leaderboard + "ACCURACY";

    public const string leaderboardTreasures = leaderboard + "TREASURES";

    public const string leaderboardTime = leaderboard + "TIME";

    public const string leaderboardCombo = leaderboard + "COMBO";

    public float playerScore = 0;

    //Editor's
    [Header("[Editor]")]
    public float sizeOfSphere =3;
    public Color colorOfLines = Color.black;
    public bool showTrack = false;


    string TimeScaleNow;    
    int TimeScaleInt = 1;
    public Rect ButPos;

    #region Leaderboard

    float GetPlayerPrefScore(int i)
    {
        return PlayerPrefs.GetFloat(leaderboardScore + i.ToString());
    }

    string GetPlayerPrefName(int i)
    {
        return PlayerPrefs.GetString(leaderboardName + i.ToString());
    }

    public void RecordScore(string _name)
    {
        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey(leaderboard + i.ToString()))
            {
                if (GetPlayerPrefScore(i) > playerScore) continue;

                else
                {
                    //my new scores is higher than i's stored scores
                    string tempName = GetPlayerPrefName(i);
                    float tempScore = GetPlayerPrefScore(i);

                    SavePlayerPrefs(i, playerScore, _name);

                    //Now i proceed to shift all the others by 1

                    if (i == 4) break;
                    //this is the last one

                    else
                    {
                        int nextSpot = i + 1;

                        string _tempName = GetPlayerPrefName(nextSpot);
                        float _tempScore = GetPlayerPrefScore(nextSpot);
                        for (int j = nextSpot; j < 5; j++)
                        {

                            if (!PlayerPrefs.HasKey(leaderboard + nextSpot.ToString()))  break;
                          
                            SavePlayerPrefs(j, tempScore, tempName);
                            tempScore = _tempScore;
                            tempName = _tempName;
                            _tempScore = GetPlayerPrefScore(j);
                            _tempName = GetPlayerPrefName(j);

                        }
                    }
                    break;
                }
            }
            else
            {
                SavePlayerPrefs(i, playerScore, _name);
                break;
                #region hide
                //byte[] bytesArray = new byte[256];
                //for (byte b = 0; b < 255; b++)
                //{
                //    bytesArray[b] = b;
                //}

                //System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
                //byte[] bytesNew = ue.GetBytes(_name);
                //string strNew = System.Convert.ToBase64String(bytesNew);

                //Debug.Log("bytes= " + bytesNew.ToString()+ " Length= " +  bytesNew.Length + " " + "string= " + strNew);

                //string jsonName = JsonUtility.ToJson(_name);

                //I CAN TRY USING XML INSTEAD OF THIS SHIT CAUSE FUCK THIS SHIT NOW

                //PlayerPrefs.SetString(leaderboard + i.ToString(), _name);

                #endregion

            }
        }
        playerScore = 0;
    }

    void SavePlayerPrefs(int x, float _score, string name_)
    {
        PlayerPrefs.SetFloat(leaderboardScore + x.ToString(), _score);
        PlayerPrefs.SetString(leaderboardName + x.ToString(), name_);
        PlayerPrefs.SetInt(leaderboard + x.ToString(), 1);
        PlayerPrefs.Save();
    }

    #endregion
    void Start()
    {
        playerCam = Camera.main;
        StartCoroutine(NextEvent());
        _eventUpdate = new EventUpdate(eventsList[currInt].Test);
        AIWaypoints = new List<GameObject>();
        AI ai = FindObjectOfType<AI>();
        for (int x = 0; x < ai.waypoints.Count; x++) AIWaypoints.Add(ai.waypoints[x]);
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

                         + " Name: " + PlayerPrefs.GetString(leaderboardName + i.ToString())

                        + " Score: " + PlayerPrefs.GetFloat(leaderboardScore + i.ToString())

                        //+" Jsoned " + JsonUtility.FromJson<string>(PlayerPrefs.GetString(leaderboard + i.ToString()))

                        + " Accuracy: " + PlayerPrefs.GetFloat(leaderboardAccuracy + i.ToString())

                        + " Treasures: " + PlayerPrefs.GetFloat(leaderboardTreasures + i.ToString())

                        + " Time Taken: " + PlayerPrefs.GetFloat(leaderboardTime + i.ToString())

                        );
            }
            Debug.Log(PlayerPrefs.GetString("FUCK", "XD"));
            playerScore += 50;
            Debug.Log("playerScore= " + playerScore);
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
