using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    //..��������� ������ ����
    public GameObject[] prefabs;
    //..Ǯ ����� �ϴ� ����Ʈ
    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // ������ Ǯ�� ��� �ִ� ���ӿ�����Ʈ ����
        // �߰��ϸ� select ������ �Ҵ�
        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        //�� ã�Ҵٸ�
        //���Ӱ� �����ϰ� select ������ �Ҵ�

        return select;
    }
}

