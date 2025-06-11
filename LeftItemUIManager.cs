using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftItemUIManager : MonoBehaviour
{
    public static LeftItemUIManager instance;

    [Header("아이템별 UI 오브젝트")]
    public GameObject bloodUI;
    public Text bloodText;

    public GameObject devilBookUI;
    public Text devilBookText;

    public GameObject devilHeadUI;
    public Text devilHeadText;

    public GameObject dollUI;
    public Text dollText;

    public GameObject skullUI;
    public Text skullText;

    public GameObject heartUI;
    public Text heartText;

    private Dictionary<string, (GameObject ui, Text text)> uiMap;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // 딕셔너리 매핑
        uiMap = new Dictionary<string, (GameObject, Text)>
        {
            { "Blood", (bloodUI, bloodText) },
            { "Devil's Book", (devilBookUI, devilBookText) },
            { "Devil's Head", (devilHeadUI, devilHeadText) },
            { "Doll", (dollUI, dollText) },
            { "Skull", (skullUI, skullText) },
            { "Heart", (heartUI, heartText) }
        };

        // UI 전부 끔
        foreach (var pair in uiMap.Values)
        {
            pair.ui.SetActive(false);
        }
    }

    void Update()
    {
        // 타이틀 UI 켜져있으면 숨김
        if (GameManager.instance != null && GameManager.instance.titleUIGroup.activeSelf)
        {
            foreach (var pair in uiMap.Values)
                pair.ui.SetActive(false);
        }
    }

    public void AddUI(ItemData data)
    {
        string name = data.itemName;
        if (uiMap.TryGetValue(name, out var ui))
        {
            ui.ui.SetActive(true);

            int count = MaterialInventoryManager.instance.GetCount(data);
            ui.text.text = "X " + count;
        }
        else
        {
            Debug.LogWarning($"[LeftItemUIManager] Unknown item name: {name}");
        }
    }

    public void ClearUI()
    {
        foreach (var pair in uiMap.Values)
        {
            pair.ui.SetActive(false);
            pair.text.text = "X 0";
        }
    }
}
