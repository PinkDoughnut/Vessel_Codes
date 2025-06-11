using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<string> obtainedMaterials = new List<string>();
    public static GameManager instance;
    public GameObject titleUIGroup;

    [Header("#Game Control")]
    public bool isLive;
    public float gameTime = 0f;
    public float maxGameTime = 2 * 10f;

    [Header("#Player Info")]
    public float Health;
    public float MaxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("#Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject enemyCleaner;

    void Awake()
    {
        instance = this;
        gameTime = 0f;
    }

    public void GameStart()
    {
        Health = MaxHealth;
        uiLevelUp.Select(1); // 임시 장비
        if (titleUIGroup != null)
            titleUIGroup.SetActive(false);

        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    public void GameRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (!isLive) return;

        gameTime += Time.deltaTime;

        if (float.IsNaN(gameTime) || float.IsInfinity(gameTime))
        {
            Debug.LogError("[GameManager] gameTime 값이 비정상입니다: " + gameTime);
            return;
        }

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

        // 스케일이 0이면 하이패스 필터 자동 OFF
        if (uiLevelUp != null && uiLevelUp.transform.localScale == Vector3.zero)
        {
            SoundOptionUI.Instance?.StopLevelUpEffect();
        }
    }

    public void GetExp()
    {
        if (!isLive) return;

        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;

            // 하이패스 필터 + 레벨업 효과음
            SoundOptionUI.Instance?.StartLevelUpEffect();
            SoundOptionUI.Instance?.PlaySFX(SoundOptionUI.Instance.levelUpSound);

            uiLevelUp.Show();
        }
    }

    public void OnLevelUpSelectionComplete()
    {
        uiLevelUp.Hide();

        // 효과 수동 해제
        SoundOptionUI.Instance?.StopLevelUpEffect();
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }

    public void AddMaterial(GameObject item)
    {
        obtainedMaterials.Add(item.name);
    }
}
