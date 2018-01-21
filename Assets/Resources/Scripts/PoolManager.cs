using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ISingleton<PoolManager> {

    int spawnBullets = 10;


    public List<GameObject> prefabsListToPool;

    List<int> toSpawn;

    public GameObject bulletPrefab, circlePrefab, fishPrefab;

    Dictionary<int, List<PoolObject>> poolQueue = new Dictionary<int, List<PoolObject>>();
    void Start()
    {
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
    }

    public void DequeuePoolObject(CirclePosUpdate x)
    {
        for(int i = 0; i < poolQueue.Count; i++) {
         
        }
    }

    void SpawnPoolObject()
    {
        for (int i = 0; i < prefabsListToPool.Count; i++)
        {
            GameObject GO = new GameObject();
            if(i == 0)
            GO.transform.SetParent(FindObjectOfType<Canvas>().transform);
            GO.name = prefabsListToPool[i].name + " Parent";
            int poolKey = prefabsListToPool[i].GetInstanceID();
            if (!poolQueue.ContainsKey(poolKey))
            {
                poolQueue.Add(poolKey, new List<PoolObject>());

                for (int j = 0; j < toSpawn[i]; j++)
                {
                    GameObject newGO = Instantiate(prefabsListToPool[i]);

                    newGO.name = prefabsListToPool[i].ToString() + j;
                    newGO.transform.SetParent(GO.transform);
                    poolQueue[poolKey].Add(newGO.GetComponent<PoolObject>());
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

    Queue<CirclePosUpdate> circleQueue = new Queue<CirclePosUpdate>();

    public void EnqCircle(CirclePosUpdate x)
    {
        circleQueue.Enqueue(x);
    }

    void SpawnCircles() {
        GameObject GO = new GameObject();
        GO.transform.SetParent(FindObjectOfType<Canvas>().transform);
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

