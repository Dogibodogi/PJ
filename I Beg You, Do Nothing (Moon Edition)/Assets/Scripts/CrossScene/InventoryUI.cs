using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemIconEntry
    {
        public InventoryItem item;
        public Sprite icon;
    }

    [SerializeField] private Button[] slotButtons;
    [SerializeField] private Image[] slotIcons;
    [SerializeField] private Image[] slotHighlights;
    [SerializeField] private ItemIconEntry[] iconEntries;

    [Header("Show inventory only in these scenes")]
    [SerializeField] private string[] allowedScenes;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Equipped Objects")]
    [SerializeField] private GameObject uvLampObject;

    private bool inventoryAllowedInCurrentScene = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < slotButtons.Length; i++)
        {
            int index = i;
            slotButtons[i].onClick.AddListener(() => InventoryManager.Instance.ToggleEquipSlot(index));
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uvLampObject = null;

        UVLightTool[] tools = Resources.FindObjectsOfTypeAll<UVLightTool>();
        for (int i = 0; i < tools.Length; i++)
        {
            if (tools[i].gameObject.scene == scene)
            {
                uvLampObject = tools[i].gameObject;
                break;
            }
        }

        inventoryAllowedInCurrentScene = false;

        for (int i = 0; i < allowedScenes.Length; i++)
        {
            if (scene.name == allowedScenes[i])
            {
                inventoryAllowedInCurrentScene = true;
                break;
            }
        }

        if (inventoryPanel != null)
            inventoryPanel.SetActive(inventoryAllowedInCurrentScene);

        if (InventoryManager.Instance != null)
            RefreshEquippedVisual(InventoryManager.Instance.EquippedItem);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += Refresh;
            InventoryManager.Instance.OnEquippedItemChanged += RefreshEquippedVisual;
        }

        Refresh();

        if (InventoryManager.Instance != null)
            RefreshEquippedVisual(InventoryManager.Instance.EquippedItem);
    }

    private void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
            InventoryManager.Instance.OnEquippedItemChanged -= RefreshEquippedVisual;
        }
    }

    public void Refresh()
    {
        if (InventoryManager.Instance == null)
            return;

        InventoryItem[] slots = InventoryManager.Instance.Slots;

        for (int i = 0; i < slotIcons.Length; i++)
        {
            InventoryItem item = slots[i];
            Sprite icon = GetIcon(item);

            slotIcons[i].sprite = icon;
            slotIcons[i].enabled = icon != null;

            bool equipped = InventoryManager.Instance.EquippedItem == item && item != InventoryItem.None;
            if (slotHighlights[i] != null)
                slotHighlights[i].enabled = equipped;
        }
    }

    private void RefreshEquippedVisual(InventoryItem equippedItem)
    {
        Refresh();

        if (!inventoryAllowedInCurrentScene)
        {
            if (uvLampObject != null)
            {
                UVLightTool uvTool = uvLampObject.GetComponent<UVLightTool>();
                if (uvTool != null)
                    uvTool.ForceTurnOff();

                uvLampObject.SetActive(false);
            }

            return;
        }

        if (uvLampObject != null)
        {
            if (equippedItem == InventoryItem.UVLight)
            {
                uvLampObject.SetActive(true);
            }
            else
            {
                UVLightTool uvTool = uvLampObject.GetComponent<UVLightTool>();
                if (uvTool != null)
                {
                    uvTool.ForceTurnOff();
                }

                uvLampObject.SetActive(false);
            }
        }
    }

    private Sprite GetIcon(InventoryItem item)
    {
        for (int i = 0; i < iconEntries.Length; i++)
        {
            if (iconEntries[i].item == item)
                return iconEntries[i].icon;
        }

        return null;
    }
}