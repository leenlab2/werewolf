using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : HealthPoints
{
    private PlayerController controller;
    public override void Die()
    {
        base.Die();
        GameState.instance.OnPlayerDeath();
        controller = GetComponent<PlayerController>();
        controller.InitPlayerState(GameState.numDeaths);
    }
}
