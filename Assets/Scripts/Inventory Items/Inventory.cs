using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        currentSelected = _slots[0];
    }

    public void UseSelected()
    {
        if (currentSelected.item == null)
            return;

        currentSelected.item.Use();
        currentSelected.count--;

        if (currentSelected.count <= 0)
        {
            _slots.Remove(currentSelected);
            currentSelected = _slots[0];
        }
    }

    public void AddItem(Item item)
    {
        Slot slot = _slots.Find(s => s.item == item);

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
    }
}
