using UnityEngine;

public class HideOutUIController : MonoBehaviour
{
    public GameObject hideOutUI;      // ���̵�ƿ� UI �г�
    public AudioSource sfxSource;     // ȿ���� ����� AudioSource
    public AudioClip doorClip;        // �� ����/�ݴ� ȿ����

    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            ExitHideOut();
        }
    }

    // ���� & ��ۿ� ��ư
    public void ToggleHideOut()
    {
        isActive = !isActive;
        hideOutUI.SetActive(isActive);

        PlaySound();
    }

    // X��ư �� ESC�� ȣ��
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