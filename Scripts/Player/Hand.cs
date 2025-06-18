using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    // 근거리 무기
    Vector3 leftPos = new Vector3(-0.2f, -0.75f, 0);// 기본 위치 값
    Quaternion leftRot = Quaternion.Euler(0, 0, -60);// 기본 회전 값
    Vector3 leftPosReverse = new Vector3(0.3f, -0.8f, 0);// 반전 위치값
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -120);// 반전 회전값


    // 원거리 무기
    Vector3 rightPos = new Vector3(1f, -0.9f, 0);// 기본 위치 값
    Quaternion rightRot = Quaternion.Euler(0, 0, -60); // 기본 회전 값
    Vector3 rightPosReverse = new Vector3(-1f, -0.9f, 0);// 반전 위치 값
    Quaternion rightRotReverse = Quaternion.Euler(0, 0, 60); // 반전 회전 값


    void Awake()
    {
        player = GetComponentInParent<Player>().GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {   // 근접무기
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            transform.localPosition = isReverse ? leftPosReverse : leftPos;
            spriter.flipY = isReverse;
            //spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {//원거리무기
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            transform.localRotation = isReverse ? rightRotReverse : rightRot;
            spriter.flipX = isReverse;
            //spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
