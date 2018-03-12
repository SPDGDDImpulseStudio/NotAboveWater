using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : ISingleton<Stats>
{
    #region Attributes

    public float roundsFired0, roundsHit1, 
        
        
        damageTaken2, 
        timeTaken3, 
        
        
        oxygenLeftover4, 
        
        
        
        oxygenLost5,
        healthCollected6, secretCollected7, chainComboCURRENT8, 
        chainComboMAX9 , 
        
        
        
        
        gameScores;

    bool s1 = false;
    bool s2 = false;
    bool s3 = false;
    float chainTimer = 0;
    bool chainingCombo = false;

    public Text txt_chainCombo;

    #endregion


    public void TrackStats(int stats, float data)
    {
        switch (stats)
        {
            case 0: roundsFired0 += data;   break;
            case 1: roundsHit1 += data;     break;

            case 2: damageTaken2 += data;   break;
            case 3: timeTaken3 += data;     break;
            case 4: oxygenLeftover4 += data;break;
            case 5: oxygenLost5 += data;    break;
            case 6: healthCollected6 += data;break;
            case 7: secretCollected7 += data; break;
            case 8:
                chainComboCURRENT8 += data;
                ChainComboCounter();
                break;
            case 9: chainComboMAX9 += data; break;
            case 10: gameScores += data;  break;
        }
    }

    void SavePlayerPrefs(int index, float _score, float _accuracy, float _time, string name_ , float treasure, float combo_)
    {
        PlayerPrefs.SetFloat(GameManager.leaderboardScore + index.ToString(), _score);
        PlayerPrefs.SetString(GameManager.leaderboardName + index.ToString(), name_);
        PlayerPrefs.SetFloat(GameManager.leaderboardAccuracy + index.ToString(), _accuracy);
        PlayerPrefs.SetFloat(GameManager.leaderboardTime + index.ToString(), _time);
        PlayerPrefs.SetInt(GameManager.leaderboard + index.ToString(), 1);
        PlayerPrefs.SetFloat(GameManager.leaderboardTreasures + index.ToString(), treasure);
        PlayerPrefs.SetFloat(GameManager.leaderboardCombo + index.ToString(), combo_);
        PlayerPrefs.Save();
    }

    public void SaveStats(string _name)
    { 
        float accuracy =  roundsHit1 / roundsFired0;

        for (int i = 0; i < 5; i++)
        {
            if (PlayerPrefs.HasKey(GameManager.leaderboard + i))
            {
                if (GetPlayerPrefScore(i) < gameScores)
                {
                    ReplaceRanking(i);
                    SavePlayerPrefs(i, gameScores,accuracy, timeTaken3, _name, secretCollected7, chainComboMAX9);
                    break;
                }
                else continue;
            }
            else
            {
                SavePlayerPrefs(i, gameScores, accuracy, timeTaken3, _name, secretCollected7, chainComboMAX9);
                break;
            }
        }

        StatsClear();
    }

    float GetPlayerPrefScore(int i)
    {
        return PlayerPrefs.GetFloat(GameManager.leaderboardScore + i.ToString());
    }

    string GetPlayerPrefName(int i)
    {
        return PlayerPrefs.GetString(GameManager.leaderboardName + i.ToString());
    }

    float GetPlayerPrefAcc(int i)
    {
        return PlayerPrefs.GetFloat(GameManager.leaderboardAccuracy + i.ToString());
    }

    float GetPlayerPrefTime(int i)
    {
        return PlayerPrefs.GetFloat(GameManager.leaderboardTime + i.ToString());
    }

    float GetPlayerPrefTreasure(int i)
    {
        return PlayerPrefs.GetFloat(GameManager.leaderboardTreasures + i.ToString());
    }
    float GetPlayerPrefMaxCombo(int i)
    {
        return PlayerPrefs.GetFloat(GameManager.leaderboardCombo + i.ToString());
    }

    public void ReplaceRanking(int currentIndex)
    {
        //if (currentIndex + 1 <= 5)
        //{
        string _tempName = GetPlayerPrefName(currentIndex);
        float _tempScore = GetPlayerPrefScore(currentIndex),
        _tempAcc = GetPlayerPrefAcc(currentIndex),
        _tempTime = GetPlayerPrefTime(currentIndex),
        _tempTreas = GetPlayerPrefTreasure(currentIndex),
        _tempCombo = GetPlayerPrefMaxCombo(currentIndex);

        //Values Im pushing awayy TO, keeping it 
        string nextName = GetPlayerPrefName(currentIndex + 1);
        float nextScore = GetPlayerPrefScore(currentIndex + 1),
        nextAcc = GetPlayerPrefAcc(currentIndex + 1),
        nextTime = GetPlayerPrefAcc(currentIndex + 1),
        nextTrea = GetPlayerPrefTreasure(currentIndex + 1),
        nextMaxCombo = GetPlayerPrefMaxCombo(currentIndex + 1);
        for (int j = currentIndex + 1; j < 5; j++)
        {
            SavePlayerPrefs(j, _tempScore, _tempAcc ,_tempTime,_tempName , _tempTreas, _tempCombo);

            _tempName = nextName;
            _tempScore = nextScore;
            _tempAcc = nextAcc;
            _tempTime = nextTime;
            _tempCombo = nextMaxCombo;
            _tempTreas = nextTrea;

            nextName = GetPlayerPrefName(j + 1);
            nextScore = GetPlayerPrefScore(j + 1);
            nextAcc = GetPlayerPrefAcc(j + 1);
            nextTime = GetPlayerPrefTime(j + 1);
            nextMaxCombo = GetPlayerPrefMaxCombo(j + 1);
            nextTrea = GetPlayerPrefTreasure(j + 1);
        }
        //   }
    }



    public void StatsClear()
    {
        roundsFired0 = 0;
        roundsHit1 = 0;

        damageTaken2 = 0;
        timeTaken3 = 0;

        oxygenLeftover4 = 0;
        oxygenLost5 = 0;

        healthCollected6 = 0;

        secretCollected7 = 0;
        chainComboCURRENT8 = 0;
        chainComboMAX9 = 0;
        gameScores = 0;
        //Call it when replay game

    }

    void ChainComboCounter()
    {
        txt_chainCombo.CrossFadeAlpha(1, 0, false);
        txt_chainCombo.CrossFadeAlpha(0, 30 * Time.deltaTime, false);
        s1 = true;
        s2 = false;
        s3 = false;
        txt_chainCombo.fontSize = 28;
        txt_chainCombo.text = "Chain Combo\n" + chainComboCURRENT8;
        chainingCombo = true;
        chainTimer = 3;
    }

    /**public void ChainComboCounter()
    {
        txt_chainCombo.text = "Chain Combo: " + chainComboCURRENT8;
        //StartCoroutine(ComboAnim());

    }

    IEnumerator ComboAnim()
    {
        txt_chainCombo.fontSize ++;
        //yield return new WaitforSeconds(2);
        txt_chainCombo.fontSize--;
    }*/

    void Update()
    {
        if (s1)
        {
            txt_chainCombo.fontSize += 4;
            if (txt_chainCombo.fontSize >= 60)
            {
                s2 = true;
            }
        }
        if (s2)
        {
            print("wew");
            s1 = false;
            txt_chainCombo.fontSize -= 4;
            if (txt_chainCombo.fontSize <= 34)
            {
                s3 = true;
            }
        }
        if (s3)
        {
            s2 = false;
            txt_chainCombo.fontSize = 34;
        }
        if (chainingCombo)
        {
            //txt_chainCombo.CrossFadeAlpha(0, 10*Time.deltaTime, false);
            chainTimer -= 1 * Time.deltaTime;
        }
    }
}
