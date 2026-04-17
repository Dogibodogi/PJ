using UnityEngine;

public class UVLampPickup : MonoBehaviour
{
    [SerializeField] private GameObject pickupVisualToHide;

    public void PickUp()
    {
        InventoryManager.Instance.AddItem(InventoryItem.UVLight);

        if (pickupVisualToHide != null)
            pickupVisualToHide.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}