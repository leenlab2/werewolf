using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] private int healAmount;

    public override void Use()
    {
        PlayerController.instance.Heal(healAmount);
    }
}
