using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AssignButtons : MonoBehaviour {


    public Button titleButton, settingBtn, leaderBoardBtn, quitBtn;

    UnityEngine.Playables.PlayableDirector pd;
    // Use this for initialization
	void Start () {
        titleButton.onClick.AddListener(SceneFade);
        quitBtn.onClick.AddListener(SceneStop);

	}
    void SceneFade()
    {
        Player.Instance.PlayerTurnOnTitleOff();
  

    }

    void SceneStop()
    {
        SceneChanger.Instance.ExitGame();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
