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
    public GameObject grp_TreasureFound;

    public List<Text> txt_Names;
    public List<Text> txt_TimeTaken;
    public List<Text> txt_Accuracy;
    public List<Text> txt_ComboMAX;
    public List<Text> txt_TotalScore;
    public List<Text> txt_TreasureFound;
    void Awake()
    {
        txt_Names = new List<Text>(grp_Names.GetComponentsInChildren<Text>());
        txt_TimeTaken = new List<Text>(grp_TimeTaken.GetComponentsInChildren<Text>());
        txt_Accuracy = new List<Text>(grp_Accuracy.GetComponentsInChildren<Text>());
        txt_ComboMAX = new List<Text>(grp_ComboMAX.GetComponentsInChildren<Text>());
        txt_TotalScore = new List<Text>(grp_TotalScore.GetComponentsInChildren<Text>());
        txt_TreasureFound = new List<Text>(grp_TreasureFound.GetComponentsInChildren<Text>());
    }

    void OnEnable()
    {
        Display();
    }
    public int currentInt =6 ;


    public void Display()
    {
        for (int i = 0; i  < txt_Accuracy.Count; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.leaderboard + i))
            {
                txt_Names[i].text = PlayerPrefs.GetString(GameManager.leaderboardName + i);
                txt_TotalScore[i].text = System.Math.Round(PlayerPrefs.GetFloat(GameManager.leaderboardScore + i)).ToString();
                txt_Accuracy[i].text = (System.Math.Round(PlayerPrefs.GetFloat(GameManager.leaderboardAccuracy + i), 2) * 100).ToString() + "%";
                txt_TreasureFound[i].text = PlayerPrefs.GetFloat(GameManager.leaderboardTreasures + i).ToString();
                txt_ComboMAX[i].text = PlayerPrefs.GetFloat(GameManager.leaderboardCombo + i).ToString();


                float seconds, minutes;
                seconds = Mathf.FloorToInt(PlayerPrefs.GetFloat(GameManager.leaderboardTime + i) % 60);
                minutes = Mathf.FloorToInt(PlayerPrefs.GetFloat(GameManager.leaderboardTime + i) / 60);

                if (seconds < 1)    txt_TimeTaken[i].text = minutes.ToString() + ":00";
                else if (seconds < 10) txt_TimeTaken[i].text = minutes.ToString() + ":0" + seconds;
                else                txt_TimeTaken[i].text = minutes.ToString() + ":" + seconds;
            }
            else
            {
                txt_Names[i].text = "N/A";
                txt_TimeTaken[i].text = "N/A";
                txt_ComboMAX[i].text = "N/A";
                txt_Accuracy[i].text = "N/A";
                txt_TotalScore[i].text = "N/A";
                txt_TreasureFound[i].text = "N/A";
             }

            ChangeColor(txt_Names[i], i);
            ChangeColor(txt_TotalScore[i], i);
            ChangeColor(txt_Accuracy[i], i);
            ChangeColor(txt_TreasureFound[i], i);
            ChangeColor(txt_ComboMAX[i], i);
            ChangeColor(txt_TimeTaken[i], i);

        }
        currentInt = 6;
    }

    void ChangeColor(Text text, int i)
    {
        Color x = Color.blue;
        x.g = 1;
        if (currentInt == i)
            text.color = x;
        else
            text.color = Color.white;
    }
}
