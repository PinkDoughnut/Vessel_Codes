using UnityEngine;
using System.Collections.Generic;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    public class DropItem
    {
        public string name;
        public GameObject prefab;
        public Rarity rarity;
    }

    public enum Rarity { Poor, Normal, Rare, SuperRare }

    [Header("등급별 아이템 리스트")]
    public List<DropItem> poorItems = new List<DropItem>();
    public List<DropItem> normalItems = new List<DropItem>();
    public List<DropItem> rareItems = new List<DropItem>();
    public List<DropItem> superRareItems = new List<DropItem>();

    [Header("드랍 확률 설정")]
    public float dropChance = 0.1f; // 10%
    public float poorRate = 0.5f; // 10%중 50%
    public float normalRate = 0.3f; // 10%중 30%
    public float rareRate = 0.15f; // 10%중 15%
    public float superRareRate = 0.05f;// 10%중 5%

    public void Drop(Vector3 position)
    {
        if (Random.value > dropChance) return; // 10% 확률로 드랍

        float roll = Random.value;
        List<DropItem> pool = null;

        if (roll < superRareRate)
            pool = superRareItems;
        else if (roll < rareRate + superRareRate)
            pool = rareItems;
        else if (roll < normalRate + rareRate + superRareRate)
            pool = normalItems;
        else
            pool = poorItems;

        if (pool == null || pool.Count == 0) return;

        // 랜덤 아이템 선택 후 생성
        int index = Random.Range(0, pool.Count);
        Instantiate(pool[index].prefab, position, Quaternion.identity);
    }
}


