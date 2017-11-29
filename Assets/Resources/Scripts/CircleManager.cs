using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleManager : MonoBehaviour {

    public static CircleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CircleManager>();
                if (_instance == null)
                {
                    GameObject newGO = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/CircleManager"));
                    _instance = newGO.GetComponent<CircleManager>();
                }
                if (_instance == null)
                    Debug.LogError("STOP");
                DontDestroyOnLoad(_instance.gameObject);
            }
            if (!_instance.gameObject.activeSelf) _instance.gameObject.SetActive(true);
            return _instance;
        }
    }

    static CircleManager _instance;

    public Button circle;

    public List<Button> currCircle = new List<Button>();

    public List<GameObject> refList = new List<GameObject>();
    // Use this for initialization

    void Start () {
        _instance = this;
        if (!circle)  circle = Resources.Load<Button>("Prefabs/Circle");

        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Spawn2Button();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ClearSet();
        }
	}
    public bool SetClear()
    {
        if (currCircle.Count == 0) return true;
        else return false;
    }
    public void ClearSet()
    {
        if (currCircle.Count == 0) return;
        
        for (int i = currCircle.Count - 1; i > -1; i--)
        {
            Button x = currCircle[i];
            currCircle.RemoveAt(i);
            Destroy(x.gameObject);
        }
        //Since i still needa destroy it
    }

    public void RemoveThisBut(Button button)
    {
        for(int i = 0; i < currCircle.Count; i++)
        {
            if (button == currCircle[i])
            {
                //Debug.Log(currCircle[i]);

                currCircle.RemoveAt(i);
            }
        }
    }
    //int numToSpawn ;
    public void SpawnButtons(int num, List<Vector2> _offSet, List<Vector2> pos, GameObject go)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        for (int i = 0; i < num; i++)
        {

            currCircle.Add(Instantiate(circle, pos[i], Quaternion.identity));
            currCircle[i].transform.SetParent(canvas.transform);
            CirclePosUpdate x = currCircle[i].GetComponent<CirclePosUpdate>();
            x._ref = go;
            //Its gonna change to the AI for sure huh
            x.offSet = _offSet[i];
        }
    }
    
    //The kind of thing i write in my 'event'
    //Under my Event i give them the lifespan too
    public void Spawn2Button()
    {
        const int numToSpawn = 3;
        Vector2 pos1 = new Vector2(Screen.height / (int)Random.Range(1, 7), Screen.width / (int)Random.Range(1, 7)),
                pos2 = new Vector2(Screen.height / (int)Random.Range(1, 7), Screen.width / (int)Random.Range(1, 7)),
                pos3 = new Vector2(),
                offSet1 = new Vector2(40, 50),
                offSet2 = new Vector2(-40, -50),
                offSet3 = new Vector2(40, -50);
        

        List<Vector2> vects = new List<Vector2>() {
         pos1,
         pos2,
         pos3,
        };
        
        List<Vector2> offSets = new List<Vector2>()
        {
            offSet1,
            offSet2,
            offSet3
        };
        SpawnButtons(numToSpawn, offSets, vects,AI.Instance.gameObject);
    }
}
