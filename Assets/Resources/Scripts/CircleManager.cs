using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleManager : ISingleton<CircleManager> {

    CirclePosUpdate circle;

    public List<CirclePosUpdate> currCircle = new List<CirclePosUpdate>();
    
    void Start () {
        if (!circle)  circle = Resources.Load<CirclePosUpdate>("Prefabs/Circle");
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
            CirclePosUpdate x = currCircle[i];
            currCircle.RemoveAt(i);
            Destroy(x.gameObject);
        }
        //Since i still needa destroy it
    }

    public void RemoveThisBut(CirclePosUpdate button)
    {
        for(int i = 0; i < currCircle.Count; i++)
        {
            if (button == currCircle[i])
            {
                currCircle.RemoveAt(i);
            }
        }
    }

    public void SpawnButtons(int num, List<Vector2> _offSet, List<Vector2> pos, GameObject go)
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        for (int i = 0; i < num; i++)
        {
            currCircle.Add(Instantiate(circle, pos[i], Quaternion.identity));
            currCircle[i].transform.SetParent(canvas.transform);
            CirclePosUpdate x = currCircle[i].GetComponent<CirclePosUpdate>();
            x._ref = go;
            x.gameObject.name = i + " " + go.name;
            //Its gonna change to the AI for sure huh
            x.offSet = _offSet[i];
        }
    }

    CirclePosUpdate GetCircle(GameObject _ref)
    {
        CirclePosUpdate x = PoolManager.Instance.DeqCircle();
        x.Init_(_ref);

        return x;
    }

    #region Hide

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
        SpawnButtons(numToSpawn, offSets, vects, AI.Instance.gameObject);
    } 
    #endregion
}
