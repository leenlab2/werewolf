using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private bool isInventoryItem = false;
    [SerializeField] public Sprite icon;
    [SerializeField] public bool consumable = false;
    public static event Action<GameObject> OnItemUsed;
    public static event Action<GameObject> OnItemPickedUp;

    public virtual void PickUp()
    {
        if (isInventoryItem)
        {
            // Pickup item
            PlayerController.instance.inventory.AddItem(this);
            Transform model = transform.Find("Model");
            model.gameObject.SetActive(false);
        }
         
        OnItemPickedUp?.Invoke(gameObject);
    }

    public virtual void Use()
    {
        OnItemUsed?.Invoke(gameObject);
        // Do something
    }
}
