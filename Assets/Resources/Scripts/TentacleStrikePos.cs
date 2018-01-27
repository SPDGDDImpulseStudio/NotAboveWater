using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleStrikePos : MonoBehaviour
{

    void OnTriggerEnter(Collider x)
    {
        //Debug.Log(x.transform.name);
    }

    void OnTriggerExit(Collider x)
    {
        //Debug.Log(x.transform.name);

    }

#if UNITY_EDITOR
    [Header("[EDITOR]")]
    public Color color;
    public float sphereSize= 5f;
    public bool showEditor;
    void OnDrawGizmos()
    {
        if (!showEditor) return;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(this.transform.position, sphereSize);
    }
#endif
}
