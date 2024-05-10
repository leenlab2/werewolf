using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected List<Item> _items = new List<Item>();
    public Item currentSelected { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        currentSelected = _items[0];
    }

    public void UseSelected()
    {
        currentSelected.Use();
    }
}
