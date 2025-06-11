using UnityEngine;
using UnityEngine.UI;

public class MaterialItem : MonoBehaviour
{
    public ItemData data;
    public int quantity;

    public Image iconImage;
    public Text nameText;
    public Text countText;

    public void Init(ItemData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;

        iconImage.sprite = data.itemIcon;
        nameText.text = data.itemName;
        countText.text = "x " + quantity.ToString();
    }

    public void SetQuantity(int q)
    {
        quantity = q;
        countText.text = "x " + quantity.ToString();
    }
}
