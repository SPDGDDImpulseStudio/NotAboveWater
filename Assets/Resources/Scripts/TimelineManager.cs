using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TimelineManager : ISingleton<TimelineManager> {

    public PlayableDirector dir;

    public PlayableAsset asset;
    
    public void Start()
    {

        //Debug.Log(dir.GetGenericBinding(dir.playableAsset.));
    }

    public void PauseTL()
    {
        dir.Pause();
    }

    public void ResumeTL()
    {
        dir.Resume();
    }
	
}
