using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private int damage;
    public bool isAxe = false;

    public override void Use()
    {
        if (isAxe && !PlayerController.instance.axeEnabled && !PlayerController.instance.isWerewolf) return;

        PlayerController.instance.AttackAttempt(damage);
    }
}
