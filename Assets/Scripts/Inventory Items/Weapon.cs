using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private int damage;
    public bool isAxe = false;

    public override void Use()
    {
        base.Use();

        if (isAxe && PlayerController.instance.isWerewolf) {
            PlayerController.instance.inventory.RemoveItem(this);
            RaiseItemError();
            return;
        } else if (!isAxe && !PlayerController.instance.isWerewolf)
        {
            RaiseItemError();
            return;
        }

        PlayerController.instance.AttackAttempt(damage);
    }
}
