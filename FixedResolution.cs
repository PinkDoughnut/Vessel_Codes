using UnityEngine;

public class FixedResolution : MonoBehaviour
{
    [Header("�ػ� ���� (3:4 ����)")]
    public int targetWidth = 600;
    public int targetHeight = 800;
    public bool runInFullscreen = false;

    void Start()
    {
        // �ػ� ����
        Screen.SetResolution(targetWidth, targetHeight, runInFullscreen);
    }
}