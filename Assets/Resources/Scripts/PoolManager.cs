using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : ISingleton<PoolManager> {

    public GameObject bulletPrefab; 
    void Start()
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

    int spawnBullets = 10;
   
    Queue<BulletScript> bulletQueue = new Queue<BulletScript>();

    List<BulletScript> onUseBulletList = new List<BulletScript>();

    public BulletScript DeqBullet()
    {
        if (bulletQueue.Count > 0) {

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
            if(onUseBulletList[i] == x)
            {
                onUseBulletList.RemoveAt(i);
                break;
            }
        }
    }


    /*
     Obj Pooling

    Queue to refer to use 
    List of GO that currently in use
     */
}
