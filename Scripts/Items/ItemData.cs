using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { melee, Range, Glove, Shoe, heal, MaterialItem }

    [Header("# Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damage;
    public int[] counts;

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
