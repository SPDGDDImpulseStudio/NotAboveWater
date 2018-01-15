using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heartrateScript : MonoBehaviour
{
    public Slider heartRate;
    public Image spr_heartRate;
    public List<Sprite> images;

    public void Update()
    {
        if (heartRate.value < 1)
            heartRate.value += 0.1f * Time.deltaTime;
        else if (heartRate.value == 1)
            heartRate.value = 0;

        if (Player.Instance.currHealth > Player.Instance.maxHealth / 2)
            spr_heartRate.sprite = images[0];
        else if (Player.Instance.currHealth > Player.Instance.maxHealth / 4 && Player.Instance.currHealth <= Player.Instance.maxHealth / 2)
            spr_heartRate.sprite = images[1];
        else if (Player.Instance.currHealth <= Player.Instance.maxHealth / 4)
            spr_heartRate.sprite = images[2];
    }
}
