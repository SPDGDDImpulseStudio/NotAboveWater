using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : ISingleton<Stats>
{
    #region Attributes

    public float roundsFired0, roundsHit1, damageTaken2, timeTaken3, oxygenLeftover4, oxygenLost5, healthCollected6, secretCollected7, chainComboCURRENT8, chainComboMAX9;

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
            case 0:
                roundsFired0 += data;
                break;
            case 1:
                roundsHit1 += data;
                break;
            case 2:
                damageTaken2 += data;
                break;
            case 3:
                timeTaken3 += data;
                break;
            case 4:
                oxygenLeftover4 += data;
                break;
            case 5:
                oxygenLost5 += data;
                break;
            case 6:
                healthCollected6 += data;
                break;
            case 7:
                secretCollected7 += data;
                break;
            case 8:
                chainComboCURRENT8 += data;
                ChainComboCounter();
                break;
            case 9:
                chainComboMAX9 += data;
                break;

        }
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
