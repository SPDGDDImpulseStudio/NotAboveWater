using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float bossCurrHealth, bossMaxHealth;

    List<Tentacle> tentacles = new List<Tentacle>();
    float timer = 3f, timerNow = 0f;

    void Start () {
        bossCurrHealth = bossMaxHealth;
        tentacles = new List<Tentacle>(FindObjectsOfType<Tentacle>());
	}
    public UnityEngine.UI.Slider slider;

    public void ShootOnShell()
    {

    }

    public void ShootOnCore()
    {

    }
	
	// Update is called once per frame
	void Update () {
        slider.value = (bossCurrHealth / bossMaxHealth);
        if (timerNow < timer) timerNow += Time.deltaTime;
        else
        {
            int b = (int)Random.Range(0, tentacles.Count );
            for (int i = 0; i < tentacles.Count; i++)
            {
                if (i == b) tentacles[i].selectOne = true;
                else
                    tentacles[i].selectOne = false;
            }
            timer = Random.Range(3f, 4f);
            timerNow = 0f;
        }
	}

}
