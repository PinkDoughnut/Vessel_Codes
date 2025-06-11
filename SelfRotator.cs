using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    public GameObject afterImagePrefab; // SpriteRenderer가 있는 잔상 프리팹
    public float createInterval = 0.05f;
    private float timer;

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;

        timer += Time.deltaTime;
        if (timer >= createInterval)
        {
            CreateAfterImage();
            timer = 0f;
        }
    }

    void CreateAfterImage()
    {
        GameObject image = Instantiate(afterImagePrefab, transform.position, transform.rotation);

        SpriteRenderer originalRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer imageRenderer = image.GetComponent<SpriteRenderer>();

        if (originalRenderer != null && imageRenderer != null)
        {
            imageRenderer.sprite = originalRenderer.sprite;
            imageRenderer.sortingLayerID = originalRenderer.sortingLayerID;
            imageRenderer.sortingOrder = originalRenderer.sortingOrder - 1;

            // 색상 강제로 보라색으로 변경
            imageRenderer.color = new Color(0.5f, 0.1f, 1f, 0.6f); // 진한 보라 + 투명도
        }

        Destroy(image, 0.3f);
    }
}