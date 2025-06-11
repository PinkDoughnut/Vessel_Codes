using UnityEngine;

public class SettingPanelController : MonoBehaviour
{
    public GameObject settingPanel;      // ����â �г� ������Ʈ
    public AudioSource sfxSource;        // ȿ���� AudioSource
    public AudioClip openSound;
    public AudioClip closeSound;


    void Update()
    {
        if (settingPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }
    public void TogglePanel()
    {
        bool isActive = settingPanel.activeSelf;

        settingPanel.SetActive(!isActive); // ���� ����

        // ���� ���
        if (sfxSource != null)
        {
            if (!isActive && openSound != null)
                sfxSource.PlayOneShot(openSound);
            else if (isActive && closeSound != null)
                sfxSource.PlayOneShot(closeSound);
        }
    }

    public void ClosePanel()
    {
        if (!settingPanel.activeSelf) return;

        settingPanel.SetActive(false);

        if (sfxSource != null && closeSound != null)
            sfxSource.PlayOneShot(closeSound);
    }
}
