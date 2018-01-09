using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Leaderboard : MonoBehaviour {

    public GameObject NamesGroup;

    public GameObject ScoresGroup;


    public List<Text> namesText = new List<Text>();
    public List<Text> scoresText = new List<Text>();

	void Start () {
        namesText = new List<Text>(NamesGroup.GetComponentsInChildren<Text>());
        scoresText = new List<Text>(ScoresGroup.GetComponentsInChildren<Text>());
        LoadRanking();
	}
	
    void LoadRanking()
    {
        for (int i = 0; i < scoresText.Count; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.leaderboard + i.ToString()))
            {
                namesText[i].text = i + 1 + " " + PlayerPrefs.GetString(GameManager.leaderboardName + i.ToString());
                scoresText[i].text = PlayerPrefs.GetFloat(GameManager.leaderboardScore + i.ToString()).ToString() ;
            }
            else scoresText[i].text = "NIL";
        }
    }
	
}
