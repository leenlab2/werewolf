using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public static event Action<GameObject> OnItemUsed;
    public static event Action<GameObject> OnItemPickedUp;

    public void PickUpItem()
    {
        PlayerController.instance.inventory.AddItem(this);
        transform.Find("Model").gameObject.SetActive(false);  
        OnItemPickedUp?.Invoke(gameObject);
    }

    public virtual void Use()
    {
        OnItemUsed?.Invoke(gameObject);
        // Do something
    }
}
