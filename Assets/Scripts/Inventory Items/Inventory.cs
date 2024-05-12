using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Progress;

[System.Serializable] public class Slot
{
    public Item item;
    public int count;
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private Sprite selected;
    [SerializeField] private Sprite deselected;
    [SerializeField] protected List<Slot> _slots = new List<Slot>();
    public Slot currentSelected { get; private set; }

    // Start is called before the first frame update
    void OnEnable()
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

        UpdateInventoryUI();
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

        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        for (int i = 0; i < 3; i++)
        {
            Transform slotPanelUI = UI.transform.GetChild(i);
            Transform itemUI = slotPanelUI.GetChild(0);

            if (i >= _slots.Count || _slots[i].item == null)
            {
                itemUI.gameObject.SetActive(false);
                slotPanelUI.GetComponent<Image>().sprite = deselected;
            }
            else
            {
                itemUI.GetComponentInChildren<Image>().sprite = _slots[i].item.icon;
                itemUI.GetComponentInChildren<TextMeshProUGUI>().text = _slots[i].count.ToString();
                itemUI.gameObject.SetActive(true);

                if (_slots[i] == currentSelected)
                {
                    slotPanelUI.GetComponent<Image>().sprite = selected;
                }
                else
                {
                    slotPanelUI.GetComponent<Image>().sprite = deselected;
                }
            }
        }
    }

    public void SelectNext(InputAction.CallbackContext ctx)
    {
        Debug.Log("Selecting next item");
        if (!ctx.performed || _slots.Count == 0) { return; }

        float scrollDirection = ctx.action.ReadValue<float>();
        if (scrollDirection == 0) { return; }

        int increment = scrollDirection > 0 ? 1 : -1;
        int idx = _slots.IndexOf(currentSelected);
        idx = (idx + increment + _slots.Count) % _slots.Count;
        currentSelected = _slots[idx];

        UpdateInventoryUI();
    }
}
