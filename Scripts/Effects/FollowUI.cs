using Unity.VisualScripting;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
    // RectTransform�� UI ���� Transform�̰�, �Ϲ� Transform���� ��ǥ�谡 UI�� ����ȭ�� �־� (��Ŀ, �ǹ� ��).
    RectTransform rectFrom;

    private void Awake() //  Awake�� "������Ʈ�� Ȱ��ȭ�� �� ���� 1ȸ ȣ��" ���� ���� = "�� �ε�� ��"��� ���� �ǰ�,Start���� ���� ȣ���.
    {
        // �� ��ũ��Ʈ�� ���� ������Ʈ���� RectTransform ������Ʈ�� ������
        rectFrom = GetComponent<RectTransform>();
    }
    private void FixedUpdate()// �Ƚõ� ������Ʈ 50���������� �����Ӹ��� ����Ǵ� �Լ�
    {
        // �÷��̾��� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ�Ͽ� UI ��ġ�� �ݿ�
        // Camera.main : ���� ī�޶� ����
        // WorldToScreenPoint : ���� ��ǥ �� ȭ�� ��ǥ�� ��ȯ
        // GameManager.instance.player : �÷��̾� ������Ʈ
        // transform.position : �÷��̾��� ���� ��ġ
        rectFrom.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        
            
    }
}
