using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideoutInventoryUI : MonoBehaviour
{
    [System.Serializable]
    public class ItemSlot
    {
        public GameObject rootObject;
        public Text itemNameText;
        public Text itemDescText;
        public Text itemCountText;
        public ItemData itemData;
    }

    [Header("왼쪽 부모 오브젝트")]
    public List<ItemSlot> leftSlots;

    [Header("오른쪽 부모 오브젝트")]
    public List<ItemSlot> rightSlots;

    [Header("UI 설정")]
    public GameObject inventoryUIRoot;
    public Button closeButton;

    [Header("사운드")]
    public AudioSource sfxAudioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private Dictionary<ItemData, ItemSlot> itemSlotMap = new();

    void Awake()
    {
        foreach (var slot in leftSlots)
        {
            slot.rootObject.SetActive(false);
            itemSlotMap[slot.itemData] = slot;
        }

        foreach (var slot in rightSlots)
        {
            slot.rootObject.SetActive(false);
            itemSlotMap[slot.itemData] = slot;
        }

        closeButton.onClick.AddListener(() => ToggleInventory(false));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory(!inventoryUIRoot.activeSelf);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && inventoryUIRoot.activeSelf)
        {
            ToggleInventory(false);
        }
    }

    public void ToggleInventory(bool isOn)
    {
        inventoryUIRoot.SetActive(isOn);

        if (sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(isOn ? openSound : closeSound);
        }

        if (isOn)
        {
            RefreshInventory();
        }
    }

    public void RefreshInventory()
    {
        foreach (var pair in itemSlotMap)
        {
            int count = MaterialInventoryManager.instance.GetCount(pair.Key);

            if (count > 0)
            {
                var slot = pair.Value;
                slot.rootObject.SetActive(true);
                slot.itemNameText.text = pair.Key.itemName;
                slot.itemDescText.text = pair.Key.itemDesc;
                slot.itemCountText.text = "X " + count;
            }
            else
            {
                pair.Value.rootObject.SetActive(false);
            }
        }
    }

    public void RefreshSlot(ItemData data)
    {
        if (itemSlotMap.TryGetValue(data, out var slot))
        {
            int count = MaterialInventoryManager.instance.GetCount(data);

            if (count > 0)
            {
                slot.rootObject.SetActive(true);
                slot.itemNameText.text = data.itemName;
                slot.itemDescText.text = data.itemDesc;
                slot.itemCountText.text = "X " + count;
            }
            else
            {
                slot.rootObject.SetActive(false);
            }
        }
    }

    // 버튼 클릭 시 인벤토리 여는 전용 함수
    public void OpenInventoryFromButton()
    {
        ToggleInventory(true);
    }
}
