using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 그 중에서 랜덤 3개 아이템활성화
        int[] ran = new int[3];
        while(true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }
        for(int index = 0; index < ran.Length; index++)
        {
            Item ranItem = items[ran[ index]];

            // 3. 만렙 아이템의 경우는 소비아이템으로 대체
            if(ranItem.level == ranItem.data.damage.Length)
            {
                // 소비 아이템이 하나일 경우 그거 하나만 활성화
                items[4].gameObject.SetActive(true);
                // 여러개 일 경우 랜덤값을 주어 등장하게 한다
                // items[Random.Range(4, 7)].gamaObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }



    }
}
