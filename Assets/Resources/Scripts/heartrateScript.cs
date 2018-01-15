using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heartrateScript : MonoBehaviour
{
    public Slider heartRate;

    public void Update()
    {
        if (heartRate.value < 1)
            heartRate.value += 0.1f * Time.deltaTime;
        else if (heartRate.value == 1)
            heartRate.value = 0;
    }
}
