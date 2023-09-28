using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public EnemyDamage script;

    public float health;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = 100;
    }

    public void Update()
    {
        slider.value = health;
        script.health.Value = health;
    }
}
