using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private InventoryItem[] slots = new InventoryItem[4];

    public InventoryItem[] Slots => slots;
    public InventoryItem EquippedItem { get; private set; } = InventoryItem.None;

    public event Action OnInventoryChanged;
    public event Action<InventoryItem> OnEquippedItemChanged;

    public void ResetInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = InventoryItem.None;
        }

        EquippedItem = InventoryItem.None;

        OnInventoryChanged?.Invoke();
        OnEquippedItemChanged?.Invoke(EquippedItem);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetInventory();   // refresh everytime so an item does not persist over different games )))
    }

    public bool AddItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == item)
                return true; // already owned
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == InventoryItem.None)
            {
                slots[i] = item;
                OnInventoryChanged?.Invoke();
                return true;
            }
        }

        Debug.LogWarning("Inventory is full.");
        return false;
    }

    public void RemoveItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == item)
            {
                slots[i] = InventoryItem.None;

                if (EquippedItem == item)
                {
                    EquippedItem = InventoryItem.None;
                    OnEquippedItemChanged?.Invoke(EquippedItem);
                }

                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    // when the user clicks an inventory slot
    public void ToggleEquipSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length)
            return;

        InventoryItem item = slots[slotIndex];
        if (item == InventoryItem.None)
            return;

        EquippedItem = EquippedItem == item ? InventoryItem.None : item;
        OnEquippedItemChanged?.Invoke(EquippedItem);
    }

    // check if a player owns a certain item
    public bool HasItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == item)
                return true;
        }
        return false;
    }
}