﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heartrateScript : MonoBehaviour
{
    public Slider heartRate;
    public Image spr_heartRate;
    public List<Sprite> images;
    float originValue;
    void Awake()
    {
        originValue = heartRate.value;
    }

    void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        heartRate.value = originValue;
        StartCoroutine(GameplayUpdate());
    }
    public IEnumerator GameplayUpdate()
    {
        while (true)
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
            yield return null;
        }
    }
}
