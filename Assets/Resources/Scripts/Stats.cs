using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : ISingleton<Stats>
{
    #region Attributes

    public float roundsFired0, roundsHit1, damageTaken2, timeTaken3, oxygenLeftover4, oxygenLost5, healthCollected6, secretCollected7, chainComboTOTAL8, chainComboMAX9;

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
                chainComboTOTAL8 += data;
                break;
            case 9:
                chainComboMAX9 += data;
                break;

        }
    }
}
