using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("스탯")]
    public PlayerStats stats = new PlayerStats();

    private PlayerInputActions inputActions;
    private Rigidbody rb;
    private Camera mainCamera;

    private PlayerWeaponManager weaponManager; // 플레이어 무기 공격 매니저

    // 마우스 & 이동
    private Vector2 mouseScreenPosition;
    private Vector3 mouseWorldPosition;
    private Vector3 moveDirection;
    private float currentSpeed;

    // 상태
    private bool isDashing = false;
    private PlayerState currentState = PlayerState.Normal;

    // 버스트
    private bool burstRequested = false;
    private float burstDelayTimer = 0f;
    private Vector3 burstDirection;

    // 부스터
    private bool isBoosterActive = false;

    // 게이지
    private float currentGauge;
    private float gaugeRegenTimer = 0f;
    private float gaugeZeroLockTimer = 0f;

    // 쿨타임
    private float burstCooldownTimer = 0f;

    private enum PlayerState
    {
        Normal,
        BurstDelay,
        BurstDash,
        Booster
    }

    void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        currentGauge = stats.maxGauge;

        weaponManager = GetComponent<PlayerWeaponManager>(); // WeaponManager 캐싱
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.MousePosition.performed += ctx =>
            mouseScreenPosition = ctx.ReadValue<Vector2>();

        inputActions.Player.Attack.started += ctx =>
        weaponManager.FireAllWeapons(); // 플레이어 Attack input을 WeaponManager에게 전달

        inputActions.Player.Dash.started += ctx => OnDashPressed();
        inputActions.Player.Dash.canceled += ctx => OnDashReleased();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        UpdateMouseWorldPosition();
        UpdateGauge();
        UpdateCooldowns();

        switch (currentState)
        {
            case PlayerState.Normal:
                UpdateNormalMovement();
                break;

            case PlayerState.BurstDelay:
                UpdateBurstDelay();
                break;

            case PlayerState.BurstDash:
                UpdateBurstDash();
                break;

            case PlayerState.Booster:
                UpdateBooster();
                break;
        }
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    // === 마우스 위치 계산 ===
    void UpdateMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position.y);

        if (groundPlane.Raycast(ray, out float distance))
        {
            mouseWorldPosition = ray.GetPoint(distance);
        }
    }

    // === 일반 이동 ===
    void UpdateNormalMovement()
    {
        Vector3 toMouse = mouseWorldPosition - transform.position;
        toMouse.y = 0;
        float distanceToMouse = toMouse.magnitude;

        // 데드라인 체크
        if (distanceToMouse <= stats.deadlineRadius)
        {
            moveDirection = Vector3.zero;
            currentSpeed = 0f;
            return;
        }

        // 방향 설정
        moveDirection = toMouse.normalized;

        // 속도 계산 (선형 보간)
        // 데드라인 경계 ~ maxSpeedDistance까지 minSpeed → maxSpeed
        float speedProgress = Mathf.Clamp01(
            (distanceToMouse - stats.deadlineRadius) / stats.maxSpeedDistance
        );
        currentSpeed = Mathf.Lerp(stats.minSpeed, stats.maxSpeed, speedProgress);

        // 회전
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    // === 버스트 시작 ===
    void OnDashPressed()
    {
        // 쿨타임 체크
        if (burstCooldownTimer > 0) return;

        // 게이지 체크
        if (currentGauge < stats.burstCost) return;

        // 벽 뚫기 불가
        Vector3 toMouse = mouseWorldPosition - transform.position;
        toMouse.y = 0;
        if (toMouse.magnitude <= stats.deadlineRadius) return;

        //대쉬상태인지
        isDashing = true;
        Debug.Log(">>> [버스트 시작]");

        // 버스트 시작
        currentState = PlayerState.BurstDelay;
        burstDelayTimer = stats.burstDelay;
        burstDirection = toMouse.normalized;

        // 게이지 소모
        currentGauge -= stats.burstCost;
        gaugeRegenTimer = stats.gaugeRegenDelay;

        // 회전
        transform.rotation = Quaternion.LookRotation(burstDirection);
    }

    // === 버스트 딜레이 (감속) ===
    void UpdateBurstDelay()
    {
        burstDelayTimer -= Time.deltaTime;

        // 감속 이동
        moveDirection = burstDirection;
        currentSpeed = stats.burstSlowSpeed;

        if (burstDelayTimer <= 0)
        {
            // 버스트 대쉬로 전환
            currentState = PlayerState.BurstDash;
            PerformBurstDash();
        }
    }

    // === 버스트 대쉬 실행 ===
    void PerformBurstDash()
    {
        // 순간 이동
        Vector3 targetPosition = transform.position + burstDirection * stats.burstDistance;
        transform.position = targetPosition;

        // 쿨타임 시작
        burstCooldownTimer = stats.burstCooldown;

        // 우클릭 누르고 있으면 부스터로, 아니면 일반으로
        if (isDashing && currentGauge > 0)
        {
            Debug.Log(">>> [부스터 전환 성공]");
            currentState = PlayerState.Booster;
            isBoosterActive = true;
        }
        else
        {
            Debug.Log($">>> [부스터 전환 실패] isDashing: {isDashing}, 게이지: {currentGauge:F1}");
            currentState = PlayerState.Normal;
            isDashing = false;
        }
    }

    // === 버스트 대쉬 업데이트 ===
    void UpdateBurstDash()
    {
        // 즉시 전환되므로 여기서는 대기만
        currentSpeed = 0f;
    }

    // === 부스터 ===
    void UpdateBooster()
    {
        // 게이지 소모
        currentGauge -= stats.boosterCostPerTick * (Time.deltaTime / 0.1f);
        gaugeRegenTimer = stats.gaugeRegenDelay;

        // 게이지 또는 입력 종료 체크
        if (currentGauge <= 0 || !isDashing)
        {
            Debug.Log(">>> [부스터 종료]");
            currentState = PlayerState.Normal;
            isBoosterActive = false;
            burstCooldownTimer = stats.burstCooldown;
            isDashing = false; 
            return;
        }

        // 마우스 방향으로 계속 이동
        Vector3 toMouse = mouseWorldPosition - transform.position;
        toMouse.y = 0;

        if (toMouse.magnitude > 0.1f)
        {
            moveDirection = toMouse.normalized;
            currentSpeed = stats.boosterSpeed;

            // 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );
        }
    }

    void OnDashReleased()
    {
        isDashing = false;
    }

    // === 이동 적용 ===
    void ApplyMovement()
    {
        if (currentState == PlayerState.BurstDash)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 velocity = moveDirection * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    // === 게이지 시스템 ===
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
            if (gaugeZeroLockTimer < stats.gaugeZeroLockTime)
                return;
        }
        else
        {
            gaugeZeroLockTimer = 0f;
        }

        // 게이지 회복
        if (currentGauge < stats.maxGauge)
        {
            currentGauge += stats.gaugeRegenRate * Time.deltaTime;
            currentGauge = Mathf.Clamp(currentGauge, 0, stats.maxGauge);
        }
    }

    void UpdateCooldowns()
    {
        if (burstCooldownTimer > 0)
            burstCooldownTimer -= Time.deltaTime;
    }

    // === 카메라용 Getter ===
    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    public bool IsMoving()
    {
        return currentSpeed > 0.1f;
    }

    public bool IsActionState()
    {
        return currentState == PlayerState.BurstDelay ||
               currentState == PlayerState.BurstDash ||
               currentState == PlayerState.Booster;
    }

    public float GetCurrentGauge()
    {
        return currentGauge;
    }

    public float GetMaxGauge()
    {
        return stats.maxGauge;
    }
}
