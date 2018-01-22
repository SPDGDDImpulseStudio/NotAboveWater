using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    #region Attributes
    public static Stats Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Stats>();
                if (_instance == null)
                    Debug.LogError("STOP");
                DontDestroyOnLoad(_instance.gameObject);
            }
            if (!_instance.gameObject.activeSelf)
                _instance.gameObject.SetActive(true);
            return _instance;
        }
    }
    static Stats _instance;

    public float roundsFired, roundsHit, damageTaken, timeTaken, oxygenLeftover, oxygenLost, healthCollected, secretCollected;

    #endregion
}
