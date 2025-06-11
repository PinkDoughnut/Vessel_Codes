using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleSequence : MonoBehaviour
{
    [Header("UI Components")]
    public Image titleImage;
    public Image buttonBGImage;
    public Text buttonText;

    [Header("Additional Buttons")]
    public Image[] otherButtonBGs;   // 추가된 버튼 배경들 (예: Hide Out, Setting, Out)
    public Text[] otherButtonTexts;  // 추가된 버튼 텍스트들

    [Header("New Visual Components")]
    public Image backgroundImage;
    public Image candleImage;

    [Header("Settings")]
    public float fadeDuration = 2f;
    public float holdDuration = 2f;
    public Color textStartColor = Color.white;
    public Color textTargetColor = Color.red;

    [Header("Advanced Timing")]
    [Range(0.1f, 5f)]
    public float otherButtonFadeMultiplier = 4f; // 추가 버튼만 빠르게 사라지도록

    [Header("UI Group Root")]
    public GameObject gameStartGroup;

    [Header("Audio")]
    public AudioSource audioSource; // Inspector에 AudioSource 연결
    public AudioClip startSfx;      // 효과음 파일

    [Header("Background Music")]
    public AudioSource titleMusicSource;
    public AudioSource gameMusicSource;
    public float musicFadeDuration = 2f;

    private bool isClicked = false;

    private Color originalTitleColor;
    private Color originalBGColor;
    private Color originalBackgroundColor;
    private Color originalCandleColor;

    private Color[] originalOtherBGColors;
    private Color[] originalOtherTextColors;

    void Awake()
    {
        originalTitleColor = titleImage.color;
        originalBGColor = buttonBGImage.color;
        originalBackgroundColor = backgroundImage.color;
        originalCandleColor = candleImage.color;

        originalOtherBGColors = new Color[otherButtonBGs.Length];
        originalOtherTextColors = new Color[otherButtonTexts.Length];

        for (int i = 0; i < otherButtonBGs.Length; i++)
        {
            originalOtherBGColors[i] = otherButtonBGs[i].color;
            originalOtherTextColors[i] = otherButtonTexts[i].color;
        }

        if (titleMusicSource && !titleMusicSource.isPlaying)
        {
            titleMusicSource.loop = true;
            titleMusicSource.Play();
        }
    }

    public void ResetTitleUI()
    {
        titleImage.color = originalTitleColor;
        buttonBGImage.color = originalBGColor;
        buttonText.color = textStartColor;
        backgroundImage.color = originalBackgroundColor;
        candleImage.color = originalCandleColor;
        isClicked = false;

        for (int i = 0; i < otherButtonBGs.Length; i++)
        {
            otherButtonBGs[i].color = originalOtherBGColors[i];
            otherButtonTexts[i].color = originalOtherTextColors[i];
        }
    }

    public void OnClickStart()
    {
        if (isClicked) return;
        isClicked = true;

        if (audioSource && startSfx)
            audioSource.PlayOneShot(startSfx);

        StartCoroutine(FadeOutAndStartGame());
    }

    IEnumerator FadeOutAndStartGame()
    {
        float time = 0f;

        // 1단계: UI 알파값 줄이기
        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            float fadeAlpha = Mathf.Lerp(1f, 0f, t);

            // 타이틀, 시작 버튼
            titleImage.color = new Color(originalTitleColor.r, originalTitleColor.g, originalTitleColor.b, fadeAlpha);
            buttonBGImage.color = new Color(originalBGColor.r, originalBGColor.g, originalBGColor.b, fadeAlpha);
            buttonText.color = Color.Lerp(textStartColor, textTargetColor, t);

            // 추가 버튼들은 더 빠르게
            float t_other = Mathf.Clamp01(t * otherButtonFadeMultiplier);
            float otherFadeAlpha = Mathf.Lerp(1f, 0f, t_other);

            for (int i = 0; i < otherButtonBGs.Length; i++)
            {
                otherButtonBGs[i].color = new Color(
                    originalOtherBGColors[i].r,
                    originalOtherBGColors[i].g,
                    originalOtherBGColors[i].b,
                    otherFadeAlpha
                );

                otherButtonTexts[i].color = new Color(
                    originalOtherTextColors[i].r,
                    originalOtherTextColors[i].g,
                    originalOtherTextColors[i].b,
                    otherFadeAlpha
                );
            }

            time += Time.deltaTime;
            yield return null;
        }

        // 최종 값 보정
        titleImage.color = new Color(originalTitleColor.r, originalTitleColor.g, originalTitleColor.b, 0f);
        buttonBGImage.color = new Color(originalBGColor.r, originalBGColor.g, originalBGColor.b, 0f);
        buttonText.color = textTargetColor;

        for (int i = 0; i < otherButtonBGs.Length; i++)
        {
            otherButtonBGs[i].color = new Color(0, 0, 0, 0);
            otherButtonTexts[i].color = new Color(0, 0, 0, 0);
        }

        // 2단계: 배경 암전 + 촛불 빨리 꺼짐
        time = 0f;
        float candleFadeDuration = fadeDuration * 0.5f;

        while (time < fadeDuration)
        {
            float t_bg = time / fadeDuration;
            float t_candle = Mathf.Clamp01(time / candleFadeDuration);

            backgroundImage.color = Color.Lerp(originalBackgroundColor, Color.black, t_bg);

            float candleAlpha = Mathf.Lerp(1f, 0f, t_candle);
            candleImage.color = new Color(originalCandleColor.r, originalCandleColor.g, originalCandleColor.b, candleAlpha);

            time += Time.deltaTime;
            yield return null;
        }

        backgroundImage.color = Color.black;
        candleImage.color = new Color(originalCandleColor.r, originalCandleColor.g, originalCandleColor.b, 0f);

        yield return new WaitForSeconds(holdDuration);

        // 음악 전환 페이드
        yield return StartCoroutine(FadeOutMusic(titleMusicSource, musicFadeDuration));
        yield return StartCoroutine(FadeInMusic(gameMusicSource, musicFadeDuration));

        if (gameStartGroup != null)
            gameStartGroup.SetActive(false);

        GameManager.instance.GameStart();
    }

    IEnumerator FadeOutMusic(AudioSource source, float duration)
    {
        if (source == null) yield break;

        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
    }

    IEnumerator FadeInMusic(AudioSource source, float duration)
    {
        if (source == null) yield break;

        source.volume = 0f;
        source.loop = true;
        source.Play();

        float time = 0f;

        while (time < duration)
        {
            source.volume = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        source.volume = 1f;
    }
}
