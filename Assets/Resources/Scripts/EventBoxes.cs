using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class EventBoxes : MonoBehaviour {

    public PlayableAsset asset1, asset2;
    public GameObject camera1;
    public GameObject camera2A;
    public GameObject camera2B;

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
                    camera1.SetActive(false);
                    camera2A.SetActive(false);
                }
                else
                {

                    TimelineManager.Instance.dir.Play(asset2);
                    camera1.SetActive(false);
                    camera2B.SetActive(false);
                }
                //TimelineManager.Instance.dir.GetGenericBinding()
                break;

            }
            yield return null;
        }
        //TimelineManager.Instance.PlayTimeline();
        Debug.Log("A");
    }
}
