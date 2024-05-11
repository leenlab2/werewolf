using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private int damage;

    public override void Use()
    {
        PlayerController.instance.AttackAttempt(damage);
    }
}
