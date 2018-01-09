using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class EventBoxes : MonoBehaviour {

    public PlayableAsset asset1, asset2;
    
	void Update () {
		
	}
    void OnTriggerEnter(Collider x)
    {
        TimelineManager.Instance.PauseTL();
        StartCoroutine(Event());
    }

    IEnumerator Event()
    {
        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (Input.mousePosition.x > Screen.width / 2)
                {

                    TimelineManager.Instance.dir.Play(asset1);
                }
                else

                    TimelineManager.Instance.dir.Play(asset2);
                //TimelineManager.Instance.dir.GetGenericBinding()
                break;

            }
            yield return null;
        }
        //TimelineManager.Instance.PlayTimeline();
        Debug.Log("A");
    }
}
