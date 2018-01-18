using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ISingleton<PoolManager> {

    void Start()
    {
        SpawnBullets();
        SpawnCircles();
    }

    int spawnBullets = 10;
    #region BulletPool

    public GameObject bulletPrefab;

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
    public GameObject circlePrefab;

    public CirclePosUpdate DeqCircle()
    {
        if (circleQueue.Count == 0) return null;
        else
        return circleQueue.Dequeue();
    }
}
