using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : HealthPoints
{
    public static event Action OnPlayerDeath;

    public override void Die()
    {
        base.Die();
        OnPlayerDeath?.Invoke();
    }
}
