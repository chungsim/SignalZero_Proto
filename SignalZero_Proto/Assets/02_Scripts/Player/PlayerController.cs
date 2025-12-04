using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour , IDamageAble
{
    [Header("스탯")]
    public PlayerMovementStats movementStats;  // 이동 관련 스탯
    public PlayerCombatStats combatStats;      // 전투 관련 스탯

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
    public float currentGauge;
    public float gaugeRegenTimer = 0f;
    public float gaugeZeroLockTimer = 0f;

    // 쿨타임
    public float burstCooldownTimer = 0f;

    // ===== HP 시스템 추가 =====
    public float currentHp;
    public bool isDead = false;

    // 무기 발사

    private bool isFiring;

    // 사운드 출력

    [SerializeField] private PlayerAudioData audioData;

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

        // 게이지 초기화
        currentGauge = combatStats.gaugeMax;

        // HP 초기화
        currentHp = combatStats.hpMax;

        weaponManager = GetComponent<PlayerWeaponManager>(); // WeaponManager 캐싱
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.MousePosition.performed += ctx =>
            mouseScreenPosition = ctx.ReadValue<Vector2>();

        inputActions.Player.Attack.started += _ => isFiring = true; // 플레이어 Attack input을 저장

        inputActions.Player.Dash.started += ctx => OnDashPressed();
        inputActions.Player.Dash.canceled += ctx => OnDashReleased();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        // 사망 상태면 업데이트 중지
        if (isDead) return;

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

        if (isFiring)
        {
            weaponManager.FireAllWeapons();
            isFiring = false;
        }
            
    }

    void FixedUpdate()
    {
        if (isDead) return;
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
        if (distanceToMouse <= movementStats.deadlineRadius)
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
            (distanceToMouse - movementStats.deadlineRadius) / movementStats.maxSpeedDistance
        );
        currentSpeed = Mathf.Lerp(movementStats.minSpeed, movementStats.moveSpeed, speedProgress);

        // 회전
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * movementStats.turnSpeed
            );
        }
    }

    // === 버스트 시작 ===
    void OnDashPressed()
    {
        // 쿨타임 체크
        if (burstCooldownTimer > 0) return;

        // 게이지 체크
        if (currentGauge < combatStats.burstCon) return;

        // 벽 뚫기 불가
        Vector3 toMouse = mouseWorldPosition - transform.position;
        toMouse.y = 0;
        if (toMouse.magnitude <= movementStats.deadlineRadius) return;

        //대쉬상태인지
        isDashing = true;
        Debug.Log(">>> [버스트 시작]");

        // 버스트 시작
        currentState = PlayerState.BurstDelay;
        burstDelayTimer = combatStats.burstDelay;
        burstDirection = toMouse.normalized;

        // 게이지 소모
        currentGauge -= combatStats.burstCon;
        gaugeRegenTimer = combatStats.gaugeRegen;

        // 회전
        transform.rotation = Quaternion.LookRotation(burstDirection);
    }

    // === 버스트 딜레이 (감속) ===
    void UpdateBurstDelay()
    {
        // BoosterStart SFX
        AudioManager.Instance.PlaySFX(audioData.boosterStartSFX);

        burstDelayTimer -= Time.deltaTime;

        // 감속 이동
        moveDirection = burstDirection;
        currentSpeed = movementStats.moveSpeed * combatStats.burstSlow;

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
        Vector3 targetPosition = transform.position + burstDirection * combatStats.burstRange;
        transform.position = targetPosition;

        // 쿨타임 시작
        burstCooldownTimer = combatStats.burstCool;

        // 우클릭 누르고 있으면 부스터로, 아니면 일반으로
        if (isDashing && currentGauge > 0)
        {
            Debug.Log(">>> [부스터 전환 성공]");
            currentState = PlayerState.Booster;
            isBoosterActive = true;

            // BoosterLoop 시작
            AudioManager.Instance.PlayLoop(audioData.boosterLoopSFX);
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
        
    }

    // === 부스터 ===
    void UpdateBooster()
    {
        // 게이지 소모
        currentGauge -= combatStats.boosterCon * (Time.deltaTime / 0.1f);
        gaugeRegenTimer = combatStats.gaugeRegen;

        // 게이지 또는 입력 종료 체크
        if (currentGauge <= 0 || !isDashing)
        {
            Debug.Log(">>> [부스터 종료]");

            // BoosterLoop 정지
            AudioManager.Instance.StopLoop();

            // BoosterEnd SFX
            AudioManager.Instance.PlaySFX(audioData.boosterEndSFX);

            currentState = PlayerState.Normal;
            isBoosterActive = false;
            burstCooldownTimer = combatStats.burstCool;
            isDashing = false;
            return;
        }

        // 마우스 방향으로 계속 이동
        Vector3 toMouse = mouseWorldPosition - transform.position;
        toMouse.y = 0;

        if (toMouse.magnitude > 0.1f)
        {
            moveDirection = toMouse.normalized;
            currentSpeed = combatStats.boosterSpeed;

            // 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * movementStats.turnSpeed
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

    // ===== HP 시스템 =====

    public void GetDamage(int damage)
    {
        TakeDamage((float)damage);
    }
    // 플레이어가 데미지를 받음
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Max(0, currentHp);

        Debug.Log($"[PlayerController] 데미지 {damage} 받음! 현재 HP: {currentHp}/{combatStats.hpMax}");

        // HP가 0 이하면 사망
        if (currentHp <= 0)
        {
            Die();
        }
    }

    /// 플레이어 사망 처리
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("[PlayerController] 플레이어 사망!");

        // 이동 정지
        currentSpeed = 0f;
        moveDirection = Vector3.zero;
        rb.velocity = Vector3.zero;

        // 입력 비활성화
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }

        // TODO: 사망 애니메이션, 이펙트, UI 표시
        // TODO: GameManager에 게임 오버 알림

        // 임시: 3초 후 오브젝트 비활성화
        Invoke(nameof(DeactivatePlayer), 3f);
    }

    private void DeactivatePlayer()
    {
        gameObject.SetActive(false);
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

    // === 게이지 Getter ===
    public float GetCurrentGauge()
    {
        return currentGauge;
    }

    public float GetMaxGauge()
    {
        return combatStats.gaugeMax;
    }

    // ===== HP Getter 추가 =====
    public float GetCurrentHp()
    {
        return currentHp;
    }

    public float GetMaxHp()
    {
        return combatStats.hpMax;
    }

    public bool IsDead()
    {
        return isDead;
    }
}