using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claws : Item
{
    [SerializeField] private int damage;
    public bool isClaws = false;

    public override void Use()
    {
        if (isClaws && PlayerController.instance.isWerewolf) return;

        PlayerController.instance.AttackAttempt(damage);
    }
}