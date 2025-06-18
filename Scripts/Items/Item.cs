// Item.cs
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLv;
    Text textName;
    Text textdesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLv = texts[0];
        textName = texts[1];
        textdesc = texts[2];

        textName.text = data.itemName;
    }

    private void OnEnable()
    {
        textLv.text = "Lv." + (level + 1);

        switch (data.itemType)
        {
            case ItemData.ItemType.melee:
            case ItemData.ItemType.Range:
                textdesc.text = string.Format(data.itemDesc, data.damage[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textdesc.text = string.Format(data.itemDesc, data.damage[level] * 100);
                break;
            default:
                textdesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.melee:
            case ItemData.ItemType.Range:
                if (weapon == null)
                {
                    GameObject newWeapon = new GameObject("Weapon");
                    newWeapon.transform.parent = GameManager.instance.player.transform;
                    newWeapon.transform.localPosition = Vector3.zero;
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage + data.baseDamage * data.damage[level];
                    int nextCount = data.counts[level];
                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (gear == null)
                {
                    GameObject newGear = new GameObject("Gear");
                    newGear.transform.parent = GameManager.instance.player.transform;
                    newGear.transform.localPosition = Vector3.zero;
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damage[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.heal:
                GameManager.instance.Health = GameManager.instance.MaxHealth;
                break;
        }

        if (level == data.damage.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
