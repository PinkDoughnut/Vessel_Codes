using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float fadeSpeed = 2;
    private SpriteRenderer sr;
    private Color color;

    private void Awake()
    {
       sr = GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    private void Update()
    {
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        if (color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
