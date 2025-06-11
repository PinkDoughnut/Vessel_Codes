using UnityEngine;

public class FixedResolution : MonoBehaviour
{
    [Header("해상도 설정 (3:4 비율)")]
    public int targetWidth = 600;
    public int targetHeight = 800;
    public bool runInFullscreen = false;

    void Start()
    {
        // 해상도 고정
        Screen.SetResolution(targetWidth, targetHeight, runInFullscreen);
    }
}