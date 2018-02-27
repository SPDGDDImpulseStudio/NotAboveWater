using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class TreasureChest : MonoBehaviour {


    public GameObject toReplaceTo;

    void OnMouseDown()
    {
     
    }

   public void OnHit()
    {
        Instantiate(toReplaceTo, this.transform.position, this.transform.rotation);
        Stats.Instance.TrackStats(7, 1);
        Player.Instance.GainScore(Random.Range(200, 500));
        Destroy(this.gameObject);
    }
}
