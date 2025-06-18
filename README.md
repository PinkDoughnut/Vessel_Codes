# **🎮 프로젝트명 : VESSEL**

**🕒 개발 기간**: 4주

**👤 제작 인원**: 1인 개발

**🧩 장르 및 시스템**: 뱀파이어 서바이벌 전투 + 타르코프 스타일 하이드아웃 시스템

사용 엔진 및 언어 : Unity / C#

### 프로젝트 개요

“VESSEL”은 생존 기반 2D 액션 게임으로, 실시간으로 등장하는 몬스터를 처치하여 자원을 획득하고, 이를 하이드아웃에서 사용해 플레이어의 능력치를 지속적으로 강화해 나가는 구조입니다.

기존의 뱀파이어 서바이벌류 게임의 반복성과 타르코프의 하이드아웃 성장 시스템을 접목하여, 단순 반복이 아닌 “축적된 자원을 통한 비선형 성장 구조”를 의도했습니다.

### **1. 게임 코어 시스템 관리**

- **구현 기능:** 게임 시작/종료, 일시정지/재개, 시간 관리, 플레이어 성장(경험치/레벨업), 전역 게임 상태 제어
- **적용 기술/코드 원리:**
    - **싱글톤 패턴:** `GameManager`를 싱글톤으로 구현하여 게임 전반의 상태 및 핵심 데이터를 중앙에서 효율적으로 관리했습니다. (`Awake()`에서 인스턴스 보장, `DontDestroyOnLoad`로 씬 전환에도 데이터 유지)
    - **게임 루프 제어:** `isLive` 플래그 및 `Time.timeScale` 조정을 통해 게임 흐름(시작, 일시정지, 재개, 게임오버/승리)을 유연하게 제어했습니다.
    - **안정적인 시간 관리:** `Update()`에서 `Time.deltaTime`으로 게임 시간을 누적하고, `float.IsNaN`, `float.IsInfinity` 검사를 통해 비정상적인 시간 값 발생을 방지했습니다.
    - **데이터 기반 레벨업:** `nextExp` 배열을 활용하여 레벨별 요구 경험치를 설정하고, `Mathf.Min`으로 배열 인덱스 오류를 방지하며 안전한 레벨업 로직을 구현했습니다.
    - **[관련 코드]:** `GameManager.cs`의 `Awake()`, `Update()`, `GameStart()`, `GameOver()`, `GameVictory()`, `GetExp()`, `Stop()`, `Resume()` 메서드

### **2. 플레이어 캐릭터 제어**

- **구현 기능:** 플레이어 이동(키보드 입력), 애니메이션 동기화, 물리 기반 충돌 처리, 체력 관리 및 사망 로직
- **적용 기술/코드 원리:**
    - **물리 기반 이동:** `FixedUpdate()`에서 `Rigidbody2D.MovePosition`을 활용하여 물리 연산 프레임에 맞춰 안정적인 캐릭터 이동을 구현했습니다.
    - **입력 및 애니메이션 제어:** `Input.GetAxisRaw`로 플레이어 입력을 받아 이동 방향을 설정하고, `LateUpdate()`에서 `Animator.SetFloat()` 및 `SpriteRenderer.flipX`를 사용하여 시각적 애니메이션을 정교하게 동기화했습니다.
    - **충돌 및 대미지 처리:** `OnCollisionStay2D`를 통해 적과의 지속적인 충돌을 감지하고, `Time.deltaTime` 기반으로 체력을 감소시키는 로직을 구현하여 생존 요소에 긴장감을 더했습니다.
    - **[관련 코드]:** `Player.cs`의 `Update()`, `FixedUpdate()`, `LateUpdate()`, `OnCollisionStay2D()` 메서드

### **3. 적 및 보스 몬스터 시스템**

- **구현 기능:** 적 이동 및 추적 AI, 피격/사망 처리, 보스 전용 패턴 및 체력 UI 연동, 아이템 드롭
- **적용 기술/코드 원리:**
    - **적 AI:** `Enemy.cs`에서 `Rigidbody2D.MovePosition`을 이용한 플레이어 추적 AI를 구현하고, `Physics2D.Raycast`를 활용한 간단한 장애물 회피 로직을 적용했습니다.
    - **보스 패턴:** `Boss.cs`는 플레이어 추적 및 스프라이트 방향 전환 로직을 포함하며, 피격 시 코루틴(`HitFlashRoutine`)을 통한 시각적 피드백과 사운드 연출을 구현했습니다.
    - **사망 및 아이템 드롭:** `Die()` 메서드에서 애니메이션 트리거, 물리 비활성화, 레이어 변경(`Ignore Raycast`) 등을 통해 사망 처리를 구현하고, `ItemDropper`를 활용하여 아이템 드롭까지 연계했습니다.
    - **[관련 코드]:** `Enemy.cs`의 `FixedUpdate()`, `TakeDamage()`, `Die()` 메서드, `Boss.cs`의 `FixedUpdate()`, `TakeDamage()`, `Die()`, `HitFlashRoutine()` 메서드

### **4. 인벤토리 및 아이템 드롭 시스템**

- **구현 기능:** 인벤토리 UI 활성화/비활성화, 획득 아이템 목록 표시, 확률 기반 아이템 드롭
- **적용 기술/코드 원리:**
    - **직렬화된 UI 슬롯 관리:** `HideoutInventoryUI.cs`에서 `[System.Serializable] ItemSlot` 클래스를 정의하여 Unity 인스펙터에서 개별 아이템 슬롯의 UI 요소와 데이터를 유연하게 연결했습니다.
    - **딕셔너리 기반 아이템 매핑:** `Dictionary<ItemData, ItemSlot>`를 사용하여 `ItemData`를 키로 하여 인벤토리 슬롯에 빠르게 접근, 효율적인 UI 업데이트 (`RefreshInventory`, `RefreshSlot`)를 구현했습니다.
    - **확률 기반 드롭 테이블:** `ItemDropper.cs`에서 `Random.value`와 등급별 (`Poor`, `Normal`, `Rare`, `SuperRare`) 확률 (`poorRate` 등)을 설정하여, 몬스터 사망 시 다양한 등급의 아이템이 드롭되도록 설계했습니다.
    - **[관련 코드]:** `HideoutInventoryUI.cs`의 `Awake()`, `RefreshInventory()`, `ItemDropper.cs`의 `Drop()` 메서드
