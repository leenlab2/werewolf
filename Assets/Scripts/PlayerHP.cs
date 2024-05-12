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
    [SerializeField] private Image hpBar;
    [SerializeField] private Image glitchedHPBar;

    [Header("Levels of Damage")]
    [SerializeField] private Sprite lowDamage;
    [SerializeField] private Sprite mediumDamage;
    [SerializeField] private Sprite highDamage;

    private float mediumDamageThreshold = 0.5f;
    private float highDamageThreshold = 0.25f;

    private float glitchedHP = -1f;

    public static event Action OnPlayerDeath;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (PlayerController.instance.glitchedHP)
        {
            glitchedHPBar.transform.parent.gameObject.SetActive(true);
            glitchedHP = _maxHealth / 2;
            glitchedHPBar.fillAmount = (float)glitchedHP / (_maxHealth / 2);
        }
        else
        {
            glitchedHPBar.transform.parent.gameObject.SetActive(false);
        }
    }

    public override void Heal(int healAmount)
    {
        if (!PlayerController.instance.glitchedHP)
        {
            base.Heal(healAmount);
        } else
        {
            // heal currenthealth first, then heal glitchedHp
            if (_currentHealth < _maxHealth)
            {
                _currentHealth += healAmount;
                if (_currentHealth > _maxHealth)
                {
                    healAmount = _currentHealth - _maxHealth;
                    _currentHealth = _maxHealth;
                } else
                {
                    healAmount = 0;
                }
            }

            glitchedHP += healAmount;
            if (glitchedHP > _maxHealth / 2)
            {
                glitchedHP = _maxHealth / 2;
            }

            glitchedHPBar.fillAmount = (float)glitchedHP / (_maxHealth / 2);
        }
        base.Heal(healAmount);
        hpBar.fillAmount = (float)_currentHealth / _maxHealth;
        UpdateDamageHUD();
    }

    public override void TakeDamage(int damage)
    {
        if (!PlayerController.instance.glitchedHP)
        {
            base.TakeDamage(damage);
        } else
        {
            // take dmg to currenthealth until it depletes, then start taking damage from glitchedHp
            if (_currentHealth > 0)
            {
                _currentHealth -= damage;
                if (_currentHealth <= 0)
                {
                    damage = -_currentHealth;
                    _currentHealth = 0;
                } else
                {
                    damage = 0;
                }
            }

            glitchedHP -= damage;
            if (glitchedHP <= 0)
            {
                glitchedHP = 0;
                Die();
            }

            glitchedHPBar.fillAmount = (float)glitchedHP / (_maxHealth / 2);
        }
        hpBar.fillAmount = (float)_currentHealth / _maxHealth;
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

       // // slowly take damage over time
       //if (Time.time % 1 == 0)
       // { 
       //     TakeDamage(1);
       //}
    }
}
