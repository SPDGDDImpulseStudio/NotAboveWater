using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public Image currentHealthBar;
    public Text ratioText;

    private float hitpoint = 150;
    private float maxHitpoint = 150;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float ratio = hitpoint / maxHitpoint;
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = (ratio * 100).ToString("0") + '%';
    }

    private void TakeDamage(float damage)
    {
        hitpoint -= damage;
        if (hitpoint < 0)
        {
            hitpoint = 0;
            Debug.Log("Dead");
        }

        UpdateHealthBar();
    }

    private void HealDamage(float heal)
    {
        hitpoint += heal;
        if (hitpoint > maxHitpoint)
            hitpoint = maxHitpoint;

        UpdateHealthBar();
    }
}
