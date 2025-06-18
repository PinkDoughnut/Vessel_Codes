using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    [Header("알파 설정")]
    public float minAlpha = 0.2f;
    public float maxAlpha = 0.6f;

    [Header("스케일 설정")]
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public float pulseSpeed = 2f;

    private SpriteRenderer sr;
    private float time;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        time += Time.deltaTime * pulseSpeed;

        // 스케일 진동
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(time) + 1f) / 2f);
        transform.localScale = new Vector3(scale, scale, scale);

        // 알파값 진동
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(time) + 1f) / 2f);
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}
