using Unity.VisualScripting;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
    // RectTransform은 UI 전용 Transform이고, 일반 Transform보다 좌표계가 UI에 최적화돼 있어 (앵커, 피벗 등).
    RectTransform rectFrom;

    private void Awake() //  Awake는 "오브젝트가 활성화될 때 최초 1회 호출" 게임 시작 = "씬 로드될 때"라고 보면 되고,Start보다 먼저 호출됨.
    {
        // 이 스크립트가 붙은 오브젝트에서 RectTransform 컴포넌트를 가져옴
        rectFrom = GetComponent<RectTransform>();
    }
    private void FixedUpdate()// 픽시드 업데이트 50프레임으로 프레임마다 실행되는 함수
    {
        // 플레이어의 월드 좌표를 스크린 좌표로 변환하여 UI 위치에 반영
        // Camera.main : 메인 카메라를 참조
        // WorldToScreenPoint : 월드 좌표 → 화면 좌표로 변환
        // GameManager.instance.player : 플레이어 오브젝트
        // transform.position : 플레이어의 현재 위치
        rectFrom.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        
            
    }
}
