using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class TreasureChest : MonoBehaviour {


    public GameObject toReplaceTo;

    void OnMouseDown()
    {
        if(Player.Instance.currBullet> 0)
        {
            Instantiate(toReplaceTo, this.transform.position,this.transform.rotation);
            Stats.Instance.TrackStats(7, 1);
            Destroy(this.gameObject);
        }
    }
}
