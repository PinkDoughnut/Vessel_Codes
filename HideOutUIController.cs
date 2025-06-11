using UnityEngine;

public class HideOutUIController : MonoBehaviour
{
    public GameObject hideOutUI;      // 하이드아웃 UI 패널
    public AudioSource sfxSource;     // 효과음 재생용 AudioSource
    public AudioClip doorClip;        // 문 여는/닫는 효과음

    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            ExitHideOut();
        }
    }

    // 진입 & 토글용 버튼
    public void ToggleHideOut()
    {
        isActive = !isActive;
        hideOutUI.SetActive(isActive);

        PlaySound();
    }

    // X버튼 및 ESC로 호출
    public void ExitHideOut()
    {
        isActive = false;
        hideOutUI.SetActive(false);

        PlaySound();
    }

    private void PlaySound()
    {
        if (sfxSource != null && doorClip != null)
            sfxSource.PlayOneShot(doorClip);
    }
}