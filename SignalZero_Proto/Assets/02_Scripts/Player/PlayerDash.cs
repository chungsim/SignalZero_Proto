using UnityEngine;

/// <summary>
/// 플레이어 대쉬 시스템
/// - 버스트 (고속 대쉬)
/// - 부스터 (지속 고속 이동)
/// - 게이지 관리
/// </summary>
public class PlayerDash : MonoBehaviour
{
    [Header("스탯")]
    public PlayerMovementStats movementStats;
    public PlayerCombatStats combatStats;

    [Header("오디오")]
    [SerializeField] private PlayerAudioData audioData;

    // 컴포넌트
    private PlayerMovement playerMovement;
    private AudioManager audioManager;

    // 상태
    public enum DashState
    {
        None,           // 대쉬 중 아님
        BurstDelay,     // 버스트 딜레이
        BurstDash,      // 버스트 고속 이동
        Booster         // 부스터 지속
    }
    private DashState currentState = DashState.None;
    private bool isDashing = false;

    // 버스트
    private float burstDelayTimer = 0f;
    private Vector3 burstDirection;
    private float burstTraveledDistance = 0f;

    // 부스터
    private bool isBoosterActive = false;

    // 게이지
    private float currentGauge;
    private float gaugeRegenTimer = 0f;
    private float gaugeZeroLockTimer = 0f;

    // 쿨타임
    private float burstCooldownTimer = 0f;

    // ===== 초기화 =====
    public void Initialize(PlayerMovement movement, AudioManager audioMgr)
    {
        playerMovement = movement;
        audioManager = audioMgr;

        // 게이지 초기화
        currentGauge = combatStats.gaugeMax;
    }

    // ===== 입력 처리 =====
    public void OnDashPressed()
    {
        // 쿨타임 체크
        if (burstCooldownTimer > 0) return;

        // 게이지 체크
        if (currentGauge < combatStats.burstCon) return;

        // 마우스 위치 가져오기
        Vector3 mouseWorldPos = playerMovement.GetMouseWorldPosition();
        Vector3 toMouse = mouseWorldPos - transform.position;
        toMouse.y = 0;

        // 벽 뚫기 불가
        if (toMouse.magnitude <= movementStats.deadlineRadius) return;

        // 대쉬 상태 설정
        isDashing = true;
        Debug.Log(">>> [버스트 시작]");

        // 게이지 소모
        currentGauge -= combatStats.burstCon;
        gaugeRegenTimer = combatStats.gaugeRegen;

        // 방향 설정
        burstDirection = toMouse.normalized;

        // 회전
        transform.rotation = Quaternion.LookRotation(burstDirection);

        // burstDelay가 0 이하면 즉시 BurstDash로 전환
        if (combatStats.burstDelay <= 0f)
        {
            Debug.Log(">>> [버스트 딜레이 건너뛰기 - 즉시 대쉬]");
            currentState = DashState.BurstDash;
            PerformBurstDash();
        }
        else
        {
            // 일반적인 경우: BurstDelay 단계 거침
            currentState = DashState.BurstDelay;
            burstDelayTimer = combatStats.burstDelay;
        }
    }

    public void OnDashReleased()
    {
        isDashing = false;
    }

    // ===== 업데이트 =====
    public void UpdateDash()
    {
        UpdateGauge();
        UpdateCooldowns();

        switch (currentState)
        {
            case DashState.None:
                // 일반 이동은 PlayerMovement가 처리
                break;

            case DashState.BurstDelay:
                UpdateBurstDelay();
                break;

            case DashState.BurstDash:
                UpdateBurstDash();
                break;

            case DashState.Booster:
                UpdateBooster();
                break;
        }
    }

    // ===== 버스트 딜레이 =====
    void UpdateBurstDelay()
    {
        burstDelayTimer -= Time.deltaTime;

        // PlayerMovement에게 감속 이동 지시
        float slowSpeed = movementStats.moveSpeed * combatStats.burstSlow;
        playerMovement.SetExternalControl(true, burstDirection, slowSpeed);

        if (burstDelayTimer <= 0)
        {
            // 버스트 대쉬로 전환
            currentState = DashState.BurstDash;
            PerformBurstDash();
        }
    }

    // ===== 버스트 대쉬 실행 =====
    void PerformBurstDash()
    {
        // 버스트 거리 추적 초기화
        burstTraveledDistance = 0f;

        // 쿨타임 시작
        burstCooldownTimer = combatStats.burstCool;

        // 버스트 사운드 재생
        if (audioData != null && audioData.boosterStartSFX != null)
        {
            audioManager.PlaySFX(audioData.boosterStartSFX);
        }

        Debug.Log(">>> [버스트 대쉬 시작] - 고속 이동 시작");
    }

    // ===== 버스트 대쉬 업데이트 =====
    void UpdateBurstDash()
    {
        // 버스트 속도: 부스터보다 4배 빠름
        float burstSpeed = combatStats.boosterSpeed * 4f;

        // PlayerMovement에게 고속 이동 지시
        playerMovement.SetExternalControl(true, burstDirection, burstSpeed);

        // 이동 거리 누적
        float frameDistance = burstSpeed * Time.deltaTime;
        burstTraveledDistance += frameDistance;

        // 목표 거리에 도달했는지 체크
        if (burstTraveledDistance >= combatStats.burstRange)
        {
            Debug.Log(">>> [버스트 대쉬 완료]");

            // 우클릭 누르고 있으면 부스터로, 아니면 일반으로
            if (isDashing && currentGauge > 0)
            {
                Debug.Log(">>> [부스터 전환 성공]");
                currentState = DashState.Booster;
                isBoosterActive = true;

                // 부스터 루프 사운드만 시작 (버스트 사운드는 이미 재생됨)
                audioManager.PlayLoop(audioData.boosterLoopSFX);
            }
            else
            {
                Debug.Log($">>> [일반 상태로 복귀] isDashing: {isDashing}, 게이지: {currentGauge:F1}");
                currentState = DashState.None;
                isDashing = false;
                // PlayerMovement에게 제어권 반환
                playerMovement.SetExternalControl(false, Vector3.zero, 0f);
            }
        }
    }

    // ===== 부스터 =====
    void UpdateBooster()
    {
        // 게이지 소모
        currentGauge -= combatStats.boosterCon * (Time.deltaTime / 0.1f);
        gaugeRegenTimer = combatStats.gaugeRegen;

        // 게이지 또는 입력 종료 체크
        if (currentGauge <= 0 || !isDashing)
        {
            Debug.Log(">>> [부스터 종료]");

            // 부스터 종료 사운드
            audioManager.PlaySFX(audioData.boosterEndSFX);
            audioManager.StopLoop();

            currentState = DashState.None;
            isBoosterActive = false;
            burstCooldownTimer = combatStats.burstCool;
            isDashing = false;

            // PlayerMovement에게 제어권 반환
            playerMovement.SetExternalControl(false, Vector3.zero, 0f);
            return;
        }

        // 마우스 방향으로 계속 이동
        Vector3 mouseWorldPos = playerMovement.GetMouseWorldPosition();
        Vector3 toMouse = mouseWorldPos - transform.position;
        toMouse.y = 0;

        if (toMouse.magnitude > 0.1f)
        {
            Vector3 direction = toMouse.normalized;

            // PlayerMovement에게 부스터 이동 지시
            playerMovement.SetExternalControl(true, direction, combatStats.boosterSpeed);
            playerMovement.SetRotation(direction);
        }
    }

    // ===== 게이지 시스템 =====
    void UpdateGauge()
    {
        // 게이지 회복 딜레이
        if (gaugeRegenTimer > 0)
        {
            gaugeRegenTimer -= Time.deltaTime;
            return;
        }

        // 게이지 0일 때 락
        if (currentGauge <= 0)
        {
            gaugeZeroLockTimer += Time.deltaTime;
            if (gaugeZeroLockTimer < combatStats.gaugeLock)
                return;
        }
        else
        {
            gaugeZeroLockTimer = 0f;
        }

        // 게이지 회복
        if (currentGauge < combatStats.gaugeMax)
        {
            currentGauge += combatStats.gaugeRecovery * Time.deltaTime;
            currentGauge = Mathf.Clamp(currentGauge, 0, combatStats.gaugeMax);
        }
    }

    void UpdateCooldowns()
    {
        if (burstCooldownTimer > 0)
            burstCooldownTimer -= Time.deltaTime;
    }

    // ===== Getter =====
    public bool IsActionState()
    {
        return currentState == DashState.BurstDelay ||
               currentState == DashState.BurstDash ||
               currentState == DashState.Booster;
    }

    public float GetCurrentGauge() => currentGauge;
    public float GetMaxGauge() => combatStats.gaugeMax;
    public bool IsBoosterActive() => isBoosterActive;
    public DashState GetCurrentState() => currentState;
}