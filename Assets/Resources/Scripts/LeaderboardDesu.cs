using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardDesu : MonoBehaviour
{
    public GameObject grp_Names;
    public GameObject grp_TimeTaken;
    public GameObject grp_Accuracy;
    public GameObject grp_ComboMAX;
    public GameObject grp_TotalScore;

    public List<Text> txt_Names;
    public List<Text> txt_TimeTaken;
    public List<Text> txt_Accuracy;
    public List<Text> txt_ComboMAX;
    public List<Text> txt_TotalScore;

    void Start()
    {
        txt_Names = new List<Text>(grp_Names.GetComponentsInChildren<Text>());
        txt_TimeTaken = new List<Text>(grp_TimeTaken.GetComponentsInChildren<Text>());
        txt_Accuracy = new List<Text>(grp_Accuracy.GetComponentsInChildren<Text>());
        txt_ComboMAX = new List<Text>(grp_ComboMAX.GetComponentsInChildren<Text>());
        txt_TotalScore = new List<Text>(grp_TotalScore.GetComponentsInChildren<Text>());

        Display();
    }

    void Display()
    {
        for ( int i = 0; i  < txt_Accuracy.Count; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.leaderboard + i))
            {
                txt_Names[i].text = PlayerPrefs.GetString(GameManager.leaderboardName + i);
                txt_TotalScore[i].text = PlayerPrefs.GetFloat(GameManager.leaderboardScore + i).ToString();
                txt_Accuracy[i].text = System.Math.Round(PlayerPrefs.GetFloat(GameManager.leaderboardAccuracy + i),2).ToString();




                float seconds, minutes;
                seconds = Mathf.RoundToInt( PlayerPrefs.GetFloat(GameManager.leaderboardTime + i)/60);
                minutes = Mathf.RoundToInt(seconds / 60);
                txt_TimeTaken[i].text = minutes.ToString() + ":" + seconds;
            }
            else break;
        }
    }
}
