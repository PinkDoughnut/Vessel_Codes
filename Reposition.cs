using System.Collections;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    private Camera mainCam;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        mainCam = Camera.main;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;

                float moveX = 0f;
                float moveY = 0f;

                float tileSize = 40f; // 타일 하나의 크기 (필요 시 조절)

                if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
                {
                    moveX = tileSize * (diffX < 0 ? -1 : 1);
                }
                else
                {
                    moveY = tileSize * (diffY < 0 ? -1 : 1);
                }

                Vector3 moveVec = new Vector3(moveX, moveY, 0);
                transform.Translate(moveVec);
                break;

            case "Enemy":
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist.normalized * 40f); // 더 멀리 보내서 화면에서 안보이게
                }
                break;
        }
    }
}