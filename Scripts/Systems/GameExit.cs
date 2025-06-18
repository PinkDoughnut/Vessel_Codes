using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameExit : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource sfx;
    public AudioClip screamClip;

    [Header("Fade Settings")]
    public GameObject fadeImageObject; // 검정 배경 이미지가 들어있는 오브젝트
    public Image fadeImage;            // 알파값 조절용 Image
    public float fadeDuration = 1.5f;

    private bool isQuitting = false;

    public void OnQuitButtonPressed()
    {
        if (isQuitting) return; // 중복 클릭 방지
        isQuitting = true;

        StartCoroutine(QuitGame());
    }

    IEnumerator QuitGame()
    {
        DisableALLInputs(); // UI 잠금

        // 검은 이미지 표시
        fadeImageObject.SetActive(true);
        fadeImage.color = new Color(0f, 0f, 0f, 0f); // 알파 0부터 시작

        // 사운드 재생
        sfx.clip = screamClip;
        sfx.volume = 1f;
        sfx.Play();

        // 화면 페이드아웃
        yield return StartCoroutine(FadeToBlack());

        // 사운드 페이드아웃
        yield return StartCoroutine(FadeOutAudio());

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void DisableALLInputs()
    {
        CanvasGroup cg = FindObjectOfType<CanvasGroup>();
        if (cg != null)
        {
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    IEnumerator FadeToBlack()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }

    IEnumerator FadeOutAudio()
    {
        float t = 0f;
        float startVol = sfx.volume;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            sfx.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        sfx.volume = 0f;
    }
}
