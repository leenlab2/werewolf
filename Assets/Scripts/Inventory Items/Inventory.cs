using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable] public class Slot
{
    public Item item;
    public int count;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] protected List<Slot> _slots = new List<Slot>();
    public Slot currentSelected { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (_slots.Count > 0)
        {
            currentSelected = _slots[0];
        } else
        {
            currentSelected = null;
        }
        
    }

    public void UseSelected(InputAction.CallbackContext ctx)
    {
        if (currentSelected == null || currentSelected.item == null || !ctx.started)
            return;

        Debug.Log("Using item " + currentSelected.item.name);
        currentSelected.item.Use();

        if (currentSelected.item.consumable)
        {
            currentSelected.count--;
        }
        
        if (currentSelected.count <= 0)
        {
            _slots.Remove(currentSelected);
            currentSelected = _slots.Count > 0 ? _slots[0] : null;
        }
    }

    public void RechargeFlashlight(InputAction.CallbackContext ctx)
    {
        if (currentSelected == null || typeof(Flashlight) != currentSelected.item.GetType())
            return;

        Flashlight flashlight = (Flashlight)currentSelected.item;
        flashlight.RechargeBattery(!ctx.canceled);
    }

    public void AddItem(Item item)
    {
        Slot slot = _slots.Find(s => s.item.GetType() == item.GetType());

        if (slot == null)
        {
            slot = new Slot();
            slot.item = item;
            slot.count = 1;
            _slots.Add(slot);
        }
        else
        {
            slot.count++;
        }

        if (currentSelected == null)
        {
            currentSelected = slot;
        }
    }
}
