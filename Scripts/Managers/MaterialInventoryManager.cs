using System.Collections.Generic;
using UnityEngine;

public class MaterialInventoryManager : MonoBehaviour
{
    public static MaterialInventoryManager instance;

    [System.Serializable]
    public class MaterialEntry
    {
        public ItemData itemData;
        public int count;
    }

    public List<MaterialEntry> materialList = new List<MaterialEntry>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("<color=red>★ 아이템 토탈 재료 초기화됨</color>");
            materialList.Clear();
        }

        // F4: 모든 아이템 수량 +1
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Debug.Log("<color=green>★ 모든 재료 수량 +1</color>");

            foreach (var entry in materialList)
            {
                entry.count += 1;

                // UI 동기화
                LeftItemUIManager.instance?.AddUI(entry.itemData);

                if (FindObjectOfType<HideoutInventoryUI>() is HideoutInventoryUI hideoutUI && hideoutUI.isActiveAndEnabled)
                {
                    hideoutUI.RefreshSlot(entry.itemData);
                }
            }
        }
    }

    public void AddMaterial(ItemData data, int amount = 1)
    {
        var entry = materialList.Find(x => x.itemData == data);
        if (entry != null) entry.count += amount;
        else materialList.Add(new MaterialEntry { itemData = data, count = amount });

        LeftItemUIManager.instance?.AddUI(data);

        if (FindObjectOfType<HideoutInventoryUI>() is HideoutInventoryUI hideoutUI && hideoutUI.isActiveAndEnabled)
        {
            hideoutUI.RefreshSlot(data);
        }
    }

    public bool HasEnough(ItemData data, int required)
    {
        var entry = materialList.Find(x => x.itemData == data);
        return entry != null && entry.count >= required;
    }

    public void UseMaterial(ItemData data, int amount)
    {
        var entry = materialList.Find(x => x.itemData == data);
        if (entry != null) entry.count -= amount;
    }

    public int GetCount(ItemData data)
    {
        var entry = materialList.Find(x => x.itemData == data);
        return entry != null ? entry.count : 0;
    }
}
