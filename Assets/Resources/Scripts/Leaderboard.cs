using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Leaderboard : MonoBehaviour {

    public GameObject NamesGroup;

    public GameObject ScoresGroup;

    public GameObject AccuracyGroup;

    public GameObject TreasuresGroup;

    public GameObject TimeGroup;

    public List<Text> namesText = new List<Text>();
    public List<Text> scoresText = new List<Text>();
    public List<Text> accuracyText = new List<Text>();
    public List<Text> treasuresText = new List<Text>();
    public List<Text> timeText = new List<Text>();

    string playerName;
    float playerScore;
    float playerAccuracy;
    float playerTreasures;
    float playerTime;
    float playerComboTotal;
    float playerComboMax;

	void Start () {
        namesText = new List<Text>(NamesGroup.GetComponentsInChildren<Text>());
        scoresText = new List<Text>(ScoresGroup.GetComponentsInChildren<Text>());
        accuracyText = new List<Text>(AccuracyGroup.GetComponentsInChildren<Text>());
        treasuresText = new List<Text>(TreasuresGroup.GetComponentsInChildren<Text>());
        timeText = new List<Text>(TimeGroup.GetComponentsInChildren<Text>());
        LoadRanking();
	}
	
    void LoadRanking()
    {
        for (int i = 0; i < scoresText.Count; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.leaderboard + i.ToString()))
            {
                namesText[i].text = i + 1 + " " + PlayerPrefs.GetString(GameManager.leaderboardName + i.ToString());
                scoresText[i].text = PlayerPrefs.GetFloat(GameManager.leaderboardScore + i.ToString()).ToString();
                //namesText[i].text = i + 1 + " " + PlayerPrefs.GetString(GameManager.leaderboardName);
                //scoresText[i].text = PlayerPrefs.GetFloat(GameManager.leaderboardScore).ToString();
            }
            else scoresText[i].text = "NIL";
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            bool save = false;
            playerName = "ImpulseDev" + Random.Range(0, 100);
            playerScore = Random.Range(0, 99999);
            //PlayerPrefs.SetString(GameManager.leaderboardName, "ImpulseDev" + Random.Range(0, 100));
            //PlayerPrefs.SetFloat(GameManager.leaderboardScore, Random.Range(0, 99999));
            for (int i = 0; i < scoresText.Count; i++)
            {
                if (PlayerPrefs.HasKey(GameManager.leaderboard + i.ToString()))
                {
                    if (playerScore < PlayerPrefs.GetFloat(GameManager.leaderboardScore + i.ToString()))
                    {
                        save = true;
                        continue;
                    } else
                    {
                        PlayerPrefs.SetString(GameManager.leaderboardName + (i+1).ToString(), playerName);
                        PlayerPrefs.SetFloat(GameManager.leaderboardScore + (i+1).ToString(), playerScore);
                    }

                    if (save)
                    {
                        PlayerPrefs.SetString(GameManager.leaderboardName + i.ToString(), playerName);
                        PlayerPrefs.SetFloat(GameManager.leaderboardScore + i.ToString(), playerScore);
                        save = false;
                        break;
                    }
                }
            }
            PlayerPrefs.Save();
            //print(PlayerPrefs.GetString(GameManager.leaderboardName) + "   " + PlayerPrefs.GetFloat(GameManager.leaderboardScore));
            LoadRanking();
        }
    }

#endif

}
