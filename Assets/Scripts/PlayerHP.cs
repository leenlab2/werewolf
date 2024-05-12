using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : HealthPoints
{
    [Header("HUD UI for Damage Effects")]
    [SerializeField] private Image glow;
    [SerializeField] private Image damageSplatter;

    [Header("Levels of Damage")]
    [SerializeField] private Sprite lowDamage;
    [SerializeField] private Sprite mediumDamage;
    [SerializeField] private Sprite highDamage;

    private float mediumDamageThreshold = 0.5f;
    private float highDamageThreshold = 0.25f;

    public static event Action OnPlayerDeath;

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        UpdateDamageHUD();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateDamageHUD();
    }

    public override void Die()
    {
        base.Die();
        OnPlayerDeath?.Invoke();
    }

    private void UpdateDamageHUD()
    {
        glow.enabled = false;

        if (_currentHealth < highDamageThreshold * _maxHealth)
        {
            damageSplatter.sprite = highDamage;
            glow.enabled = true;
        }
        else if (_currentHealth < mediumDamageThreshold * _maxHealth)
        {
            damageSplatter.sprite = mediumDamage;
        }
        else if (_currentHealth < _maxHealth)
        {
            damageSplatter.sprite = lowDamage;
        }

        damageSplatter.enabled = _currentHealth < _maxHealth;
    }

    private void Regenerate()
    {
        if (!PlayerController.instance.appetiteEnabled && (Time.time % 2 == 0))
        {
            Heal(1);
        }
    }

    private void FixedUpdate()
    {
        Regenerate();

    //    // slowly take damage over time
    //   if (Time.time % 1 == 0)
    //    {
    //        TakeDamage(1);
    //    }
    }
}
