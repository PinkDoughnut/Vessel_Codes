using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    // �ٰŸ� ����
    Vector3 leftPos = new Vector3(-0.2f, -0.75f, 0);// �⺻ ��ġ ��
    Quaternion leftRot = Quaternion.Euler(0, 0, -60);// �⺻ ȸ�� ��
    Vector3 leftPosReverse = new Vector3(0.3f, -0.8f, 0);// ���� ��ġ��
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -120);// ���� ȸ����


    // ���Ÿ� ����
    Vector3 rightPos = new Vector3(1f, -0.9f, 0);// �⺻ ��ġ ��
    Quaternion rightRot = Quaternion.Euler(0, 0, -60); // �⺻ ȸ�� ��
    Vector3 rightPosReverse = new Vector3(-1f, -0.9f, 0);// ���� ��ġ ��
    Quaternion rightRotReverse = Quaternion.Euler(0, 0, 60); // ���� ȸ�� ��


    void Awake()
    {
        player = GetComponentInParent<Player>().GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft)
        {   // ��������
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            transform.localPosition = isReverse ? leftPosReverse : leftPos;
            spriter.flipY = isReverse;
            //spriter.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {//���Ÿ�����
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            transform.localRotation = isReverse ? rightRotReverse : rightRot;
            spriter.flipX = isReverse;
            //spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
