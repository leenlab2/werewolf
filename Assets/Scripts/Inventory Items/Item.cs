using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static event Action<GameObject> OnItemUsed;

    public virtual void Use()
    {
        OnItemUsed?.Invoke(gameObject);
        // Do something
    }
}
