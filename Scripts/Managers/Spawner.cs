using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("스폰 위치")]
    public Transform[] spawnPoint;

    [Header("레벨별 몬스터 스폰 데이터")]
    public LevelSpawnData[] spawnLevels;

    [Header("보스 설정")]
    public GameObject bossPrefab;
    public float bossSpawnTime = 240f;
    private bool bossSpawned = false;

    [Header("레벨 설정")]
    public float levelTime = 60f;
    private int currentLevel;

    private Dictionary<LevelSpawnData, float> dataTimers = new Dictionary<LevelSpawnData, float>();

    void Start()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        spawnPoint = System.Array.FindAll(spawnPoint, t => t.name.Contains("Point"));
        Debug.Log($"[Spawner] 스폰포인트 개수: {spawnPoint.Length}");

        if (spawnLevels == null || spawnLevels.Length == 0)
            Debug.LogError("[Spawner] spawnLevels 배열이 비어있습니다!");

        foreach (var data in spawnLevels)
        {
            dataTimers[data] = 0f;
        }
    }

    void Update()
    {
        if (GameManager.instance == null || !GameManager.instance.isLive)
            return;

        currentLevel = Mathf.FloorToInt(GameManager.instance.gameTime / levelTime);

        LevelSpawnData[] matchingSpawnData = GetAllMatchingSpawnData(currentLevel);
        foreach (var data in matchingSpawnData)
        {
            dataTimers[data] += Time.deltaTime;

            if (dataTimers[data] > data.spawnInterval)
            {
                SpawnMonster(data);
                dataTimers[data] = 0f;
            }
        }

        if (!bossSpawned && GameManager.instance.gameTime >= bossSpawnTime)
        {
            bossSpawned = true;
            Debug.Log("[Spawner] 보스 소환 조건 만족 - SpawnBoss() 호출됨");
            SpawnBoss();
        }

        // F1 키로 강제 보스 소환
        if (Input.GetKeyDown(KeyCode.F1) && !bossSpawned)
        {
            bossSpawned = true;
            Debug.Log("[Spawner] F1 키 입력 - 보스 강제 소환");
            SpawnBoss();
        }
    }

    LevelSpawnData[] GetAllMatchingSpawnData(int level)
    {
        List<LevelSpawnData> result = new List<LevelSpawnData>();
        foreach (var data in spawnLevels)
        {
            if (level >= data.minLevel && (data.maxLevel == -1 || level <= data.maxLevel))
                result.Add(data);
        }
        return result.ToArray();
    }

    void SpawnMonster(LevelSpawnData data)
    {
        if (data.monsters == null || data.monsters.Length == 0)
            return;

        // 랜덤으로 한 마리만 소환 (동시 폭발 방지)
        MonsterEntry entry = data.monsters[Random.Range(0, data.monsters.Length)];

        if (entry.prefab == null)
        {
            Debug.LogError("[Spawner] MonsterEntry의 prefab이 null입니다!");
            return;
        }

        Vector3 spawnPos = GetRandomSpawnPoint();

        GameObject enemy = Instantiate(entry.prefab, spawnPos, Quaternion.identity);

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.health = entry.health;
            enemyScript.speed = entry.speed;
        }
    }

    void SpawnBoss()
    {
        Vector3 spawnPos = GetRandomSpawnPoint();
        GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"[Spawner] 보스 소환됨 - 위치: {spawnPos}");

        Boss bossScript = boss.GetComponent<Boss>();
        if (bossScript != null)
        {
            bossScript.OnBossSpawn();
        }
    }

    Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoint.Length <= 1)
        {
            Debug.LogError("[Spawner] 스폰 포인트가 2개 이상 필요합니다");
            return transform.position;
        }

        return spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}

[System.Serializable]
public class LevelSpawnData
{
    public int minLevel;
    public int maxLevel; // -1이면 무제한
    public float spawnInterval = 3f;
    public MonsterEntry[] monsters;
}

[System.Serializable]
public class MonsterEntry
{
    public GameObject prefab;
    public int health;
    public float speed;
}
