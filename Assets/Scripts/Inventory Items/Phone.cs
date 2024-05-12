using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Phone : Item
{
    public static event Action OnPlayerCall;

    public override void PickUp()
    {
        base.PickUp();
        OnPlayerCall?.Invoke();
    }
}
