using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : HealthPoints
{
    public override void Die()
    {
        base.Die();
        GameState.instance.OnPlayerDeath();
    }
}
