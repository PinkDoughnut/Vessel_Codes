using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [Header("������Ʈ ������")]
    public GameObject[] objectPrefabs;

    [Header("���� �Ÿ� ����")]
    public float minSpawnDistance = 6f;
    public float maxSpawnDistance = 10f;

    [Header("���� �ֱ�")]
    public float spawnInterval = 5f;

    private Transform player;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private const int maxVisibleObjects = 8;
    private const int maxTotalObjects = 15;

    private void Start()
    {
        player = GameManager.instance.player.transform;
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (GameManager.instance == null || !GameManager.instance.isLive)
            {
                yield return null;
                continue;
            }

            int visibleCount = CountObjectsInView();
            if (visibleCount < maxVisibleObjects)
            {
                SpawnObject();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObject()
    {
        if (objectPrefabs.Length == 0 || player == null)
            return;

        // �ı� ������ ������Ʈ Ž��
        if (spawnedObjects.Count >= maxTotalObjects)
        {
            GameObject objectToRemove = null;
            foreach (GameObject o in spawnedObjects)
            {
                if (!IsVisibleOnCamera(o.transform.position))
                {
                    objectToRemove = o;
                    break;
                }
            }

            if (objectToRemove != null)
            {
                spawnedObjects.Remove(objectToRemove);
                Destroy(objectToRemove);
            }
            else
            {
                // ȭ�� �ȿ� �ִ� ������Ʈ�� �ְ� ���� �Ұ� �� �������� ����
                return;
            }
        }

        // 1. ������ ������ ����
        GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];

        // 2. ���� ���� + �Ÿ� ���
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawnPos = (Vector2)player.position + offset;

        // 3. ������Ʈ ����
        GameObject newObject = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(newObject);
    }

    int CountObjectsInView()
    {
        int count = 0;
        foreach (GameObject o in spawnedObjects)
        {
            if (o == null) continue;
            if (IsVisibleOnCamera(o.transform.position))
                count++;
        }
        return count;
    }

    bool IsVisibleOnCamera(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToViewportPoint(worldPos);
        return screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1;
    }
}
