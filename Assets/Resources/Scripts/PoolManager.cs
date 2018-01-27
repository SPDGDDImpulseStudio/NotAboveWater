using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ISingleton<PoolManager> {
    #region NotRelatedToGenericPool

    int spawnBullets = 10;

    public List<GameObject> prefabsListToPool;

    List<int> toSpawn;

    #endregion

    public GameObject bulletPrefab, circlePrefab, fishPrefab;

    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    void Start()
    {
        #region NotR2GenericPool

        toSpawn = new List<int>() {
            4,
            9,
            6
        };

        prefabsListToPool = new List<GameObject>()
        {
            circlePrefab,
            bulletPrefab,
            fishPrefab
        };

        SpawnBullets();
        SpawnCircles(); 
        #endregion
    }
    
    bool isProcessingPath = false;

    Queue<PathRequest> pathReqQueue = new Queue<PathRequest>();

    PathRequest currPathReq;

    public static void RequestCreatePool(GameObject _prefab, int _poolSize, Transform _parent)
    {
        PathRequest newPathReq = new PathRequest( _prefab, _poolSize, _parent);
        Instance.pathReqQueue.Enqueue(newPathReq);
        Instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if (!isProcessingPath && pathReqQueue.Count > 0)
        {
            isProcessingPath = true;
            currPathReq = pathReqQueue.Dequeue();
            CreatePool(currPathReq.prefab, currPathReq.poolSize, currPathReq.parent);

            Debug.Log("currPath is now: " + currPathReq.prefab);
        }
    }

    void CreatePool(GameObject prefab, int poolSize, Transform parent)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());
            
            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab);
                poolDictionary[poolKey].Enqueue(newObject);
                newObject.transform.SetParent(parent);
                newObject.GetComponent<PoolObject>().TurnOff();
            }
        }

        FinishedCreatingPoolObject();
    }
    void FinishedCreatingPoolObject()
    {
        isProcessingPath = false;
        TryProcessNext();
    }
/*
    Player 
        VFX
    AI/Tentacle 
        Bullet
        Circle
    
    
         */
    public GameObject ReturnGOFromList(GameObject prefabToReturn)
    {
        int prefabKey = prefabToReturn.GetInstanceID();

        if (poolDictionary.ContainsKey(prefabKey))
        {
            GameObject firstItemOfQueue = poolDictionary[prefabKey].Dequeue();

            poolDictionary[prefabKey].Enqueue(firstItemOfQueue);

            return firstItemOfQueue;
        }
        else
        {
            Debug.LogError("This " + prefabToReturn +" is not included in the list!");
            return null;
        }
    }
    //NotR
    void SpawnPoolObject()
    {
        for (int i = 0; i < prefabsListToPool.Count; i++)
        {
            GameObject GO = new GameObject();
            if (prefabsListToPool[i] == circlePrefab)
            {
                Canvas[] newCanvases = FindObjectsOfType<Canvas>();
                for (int j = 0; j < newCanvases.Length; j++)
                {
                    if (newCanvases[j].name != "SceneChanger")
                    {
                        GO.transform.SetParent(newCanvases[j].transform);
                        break;
                    }
                }
            }
            GO.name = prefabsListToPool[i].name + " Parent";
            int poolKey = prefabsListToPool[i].GetInstanceID();
            if (!poolDictionary.ContainsKey(poolKey))
            {
                //poolDictionary.Add(poolKey, new List<PoolObject>());

                for (int j = 0; j < toSpawn[i]; j++)
                {
                    GameObject newGO = Instantiate(prefabsListToPool[i]);

                    newGO.name = prefabsListToPool[i].ToString() + j;
                    newGO.transform.SetParent(GO.transform);
                    //poolDictionary[poolKey].Add(newGO.GetComponent<PoolObject>());
                }
            }
        }
    }

    #region BulletPool
    
    Queue<BulletScript> bulletQueue = new Queue<BulletScript>();

    List<BulletScript> onUseBulletList = new List<BulletScript>();

    void SpawnBullets()
    {
        GameObject GO = new GameObject();
        for (int i = 0; i < spawnBullets; i++)
        {
            GameObject newGO = Instantiate(bulletPrefab);
            newGO.name = "Bullet " + i;
            newGO.transform.SetParent(GO.transform);
            EnqBullet(newGO.GetComponent<BulletScript>());
        }
        GO.name = "Bullet Parent";
    }
    public BulletScript DeqBullet()
    {
        if (bulletQueue.Count > 0)
        {
            BulletScript x = bulletQueue.Dequeue();
            onUseBulletList.Add(x);
            return x;
        }
        else
        {
            BulletScript x = onUseBulletList[0];
            return x;
        }
    }

    public void EnqBullet(BulletScript x)
    {
        bulletQueue.Enqueue(x);
        CheckBulletInList(x);
        x.gameObject.SetActive(false);
    }

    void CheckBulletInList(BulletScript x)
    {
        if (onUseBulletList.Count < 0) return;
        for (int i = 0; i < onUseBulletList.Count; i++)
        {
            if (onUseBulletList[i] == x)
            {
                onUseBulletList.RemoveAt(i);
                break;
            }
        }
    }


    #endregion
    #region CirclePool

    Queue<CirclePosUpdate> circleQueue = new Queue<CirclePosUpdate>();

    public void EnqCircle(CirclePosUpdate x)
    {
        circleQueue.Enqueue(x);
    }

    void SpawnCircles()
    {
        GameObject GO = new GameObject();
        Canvas[] newCanvases = FindObjectsOfType<Canvas>();
        for (int i = 0; i < newCanvases.Length; i++)
        {
            if (newCanvases[i].name != "SceneChanger")
            {
                GO.transform.SetParent(newCanvases[i].transform);
                break;
            }
        }

        for (int i = 0; i < spawnBullets; i++)
        {
            GameObject newGO = Instantiate(circlePrefab);
            newGO.name = "Circle " + i;
            newGO.transform.SetParent(GO.transform);
            EnqCircle(newGO.GetComponent<CirclePosUpdate>());
        }
        GO.name = "Circle Parent";
    }


    public CirclePosUpdate DeqCircle()
    {
        if (circleQueue.Count == 0) return null;
        else
            return circleQueue.Dequeue();
    } 
    #endregion
        
}
struct PathRequest
{
    public GameObject prefab;
    public int poolSize;
    public Transform parent;

    //public Action<List<Vector3>, bool> callBack;

    public PathRequest(GameObject _prefab, int _poolSize, Transform _parent )//, Action<List<Vector3>, bool> _callBack)
    {
        prefab = _prefab;
        poolSize = _poolSize;
        parent = _parent;
    }
}
//public class ObjectInstance
//{
//    GameObject go;
//    Transform _t;
//    bool hasPoolObject;

//    public ObjectInstance(GameObject _go)
//    {
//        go = _go;
//        if (go.GetComponent<PoolObject>())
//        {

//        }
//    }
//}

