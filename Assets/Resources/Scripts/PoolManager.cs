using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ISingleton<PoolManager> {
    
    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();
    
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
                newObject.name = prefab.name +" " + i;
                newObject.GetComponent<PoolObject>().TurnOff();
            }
        }else{
#if UNITY_EDITOR
            Debug.Log(prefab + " is already inside the pool!");
#endif
        }
        FinishedCreatingPoolObject();
    }
    void FinishedCreatingPoolObject()
    {
        isProcessingPath = false;
        TryProcessNext();
    }

    //In a sense, any script that calls this should've req for prefab making + storing
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

