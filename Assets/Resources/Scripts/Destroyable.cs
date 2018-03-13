using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Destroyable : MonoBehaviour {

    public GameObject afterPop;

    [Tooltip("The rigidbodies of what i want to push away from the player")]
    public List<Rigidbody> rb = new List<Rigidbody>();
    
    [Range(1f,1000f)]
    public float force = 1f;

    [Tooltip("If u want to apply force when click on this thing")]
    public bool shootAway = false;

    public float scores = 100;
    public void OnHit()
    {
        gameObject.SetActive(false);
        afterPop.SetActive(true);
        Player.Instance.GainScore(scores);
        if (shootAway)
        {
            for (int i = 0;i < rb.Count; i++)
            {
                Vector3 dir = Player.Instance.transform.position - this.transform.localPosition;
                rb[i].AddForce(dir * force);
            }
        }
    }
}
