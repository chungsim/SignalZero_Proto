using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 메인 컨트롤러
/// - 컴포넌트 관리 및 연결
/// - 입력 처리 및 전달
/// - Update 루프 관리
/// - IDamageAble 구현 (PlayerCombat으로 전달)
/// </summary>
public class PlayerController : MonoBehaviour, IDamageAble
{
    [Header("스탯")]
    public PlayerMovementStats movementStats;
    public PlayerCombatStats combatStats;

    // 컴포넌트
    private PlayerMovement playerMovement;
    private PlayerDash playerDash;
    private PlayerCombat playerCombat;
    private PlayerInputActions inputActions;
    private Rigidbody rb;
    private Camera mainCamera;
    private AudioManager audioManager;

    void Awake()
    {
        // 컴포넌트 가져오기
        playerMovement = GetComponent<PlayerMovement>();
        playerDash = GetComponent<PlayerDash>();
        playerCombat = GetComponent<PlayerCombat>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

        // ✅ AudioManager 찾기 (Instance 대신 FindObjectOfType 사용)
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("[PlayerController] AudioManager를 찾을 수 없습니다!");
        }

        // 입력 시스템
        inputActions = new PlayerInputActions();

        // ✅ Stats를 먼저 설정 (Initialize 이전에!)
        if (playerMovement != null)
        {
            playerMovement.movementStats = movementStats;
        }

        if (playerDash != null)
        {
            playerDash.movementStats = movementStats;
            playerDash.combatStats = combatStats;
        }

        if (playerCombat != null)
        {
            playerCombat.combatStats = combatStats;
        }

        // ✅ 그 다음 Initialize 호출
        // 이동 시스템 초기화
        if (playerMovement != null)
        {
            playerMovement.Initialize(rb, mainCamera);
        }

        // 대쉬 시스템 초기화
        if (playerDash != null)
        {
            playerDash.Initialize(playerMovement, audioManager);
        }

        // 전투 시스템 초기화
        if (playerCombat != null)
        {
            playerCombat.Initialize(
                GetComponent<PlayerWeaponManager>(),
                rb,
                inputActions
            );
        }
    }

    void OnEnable()
    {
        inputActions.Player.Enable();

        // 공격 입력
        inputActions.Player.Attack.started += _ => playerCombat?.OnAttackPressed();

        // 대쉬 입력
        inputActions.Player.Dash.started += _ => playerDash?.OnDashPressed();
        inputActions.Player.Dash.canceled += _ => playerDash?.OnDashReleased();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        // 사망 체크
        if (playerCombat != null && playerCombat.IsDead()) return;

        // ✅ 매 프레임 마우스 위치 업데이트
        if (playerMovement != null)
        {
            Vector2 mousePos = inputActions.Player.MousePosition.ReadValue<Vector2>();
            playerMovement.SetMousePosition(mousePos);
        }

        // 이동 시스템 업데이트
        if (playerMovement != null)
        {
            playerMovement.UpdateMovement();
        }

        // 대쉬 시스템 업데이트
        if (playerDash != null)
        {
            playerDash.UpdateDash();
        }

        // 전투 시스템 업데이트
        if (playerCombat != null)
        {
            playerCombat.UpdateCombat();
        }
    }

    void FixedUpdate()
    {
        // 사망 체크
        if (playerCombat != null && playerCombat.IsDead()) return;

        // 물리 기반 이동 적용
        if (playerMovement != null)
        {
            playerMovement.ApplyMovement();
        }
    }

    // ===== 카메라용 Public API =====
    public Vector3 GetMoveDirection()
    {
        return playerMovement != null ? playerMovement.GetMoveDirection() : Vector3.zero;
    }

    public bool IsMoving()
    {
        return playerMovement != null && playerMovement.IsMoving();
    }

    public bool IsActionState()
    {
        return playerDash != null && playerDash.IsActionState();
    }

    // ===== 게이지 Getter =====
    public float GetCurrentGauge()
    {
        return playerDash != null ? playerDash.GetCurrentGauge() : 0f;
    }

    public float GetMaxGauge()
    {
        return playerDash != null ? playerDash.GetMaxGauge() : 0f;
    }

    // ===== HP Getter =====
    public float GetCurrentHp()
    {
        return playerCombat != null ? playerCombat.GetCurrentHp() : 0f;
    }

    public float GetMaxHp()
    {
        return playerCombat != null ? playerCombat.GetMaxHp() : 0f;
    }

    public bool IsDead()
    {
        return playerCombat != null && playerCombat.IsDead();
    }

    // ===== IDamageAble 구현 =====
    /// <summary>
    /// 몬스터 등 외부에서 데미지를 줄 때 호출
    /// PlayerCombat으로 전달
    /// </summary>
    public void GetDamage(int damage)
    {
        if (playerCombat != null)
        {
            playerCombat.GetDamage(damage);
        }
    }
}