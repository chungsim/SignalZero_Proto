using UnityEngine;

/// <summary>
/// 플레이어 전투 시스템
/// - HP 관리
/// - 데미지 처리
/// - 무기 발사
/// - 사망 처리
/// </summary>
public class PlayerCombat : MonoBehaviour, IDamageAble
{
    [Header("스탯")]
    public PlayerCombatStats combatStats;

    // 컴포넌트
    private PlayerWeaponManager weaponManager;
    private Rigidbody rb;
    private PlayerInputActions inputActions;

    // HP
    private float currentHp;
    private bool isDead = false;

    // 무기 발사
    private bool isFiring = false;

    // ===== 초기화 =====
    public void Initialize(PlayerWeaponManager weaponMgr, Rigidbody rigidbody, PlayerInputActions inputs)
    {
        weaponManager = weaponMgr;
        rb = rigidbody;
        inputActions = inputs;

        // HP 초기화
        currentHp = combatStats.hpMax;
    }

    // ===== 입력 처리 =====
    public void OnAttackPressed()
    {
        isFiring = true;
    }

    // ===== 업데이트 =====
    public void UpdateCombat()
    {
        if (isDead) return;

        if (isFiring)
        {
            weaponManager.FireAllWeapons();
            isFiring = false;
        }
    }

    // ===== HP 시스템 =====
    public void GetDamage(int damage)
    {
        TakeDamage((float)damage);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Max(0, currentHp);

        Debug.Log($"[PlayerCombat] 데미지 {damage} 받음! 현재 HP: {currentHp}/{combatStats.hpMax}");

        // HP가 0 이하면 사망
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("[PlayerCombat] 플레이어 사망!");

        // 이동 정지
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

    // ===== Getter =====
    public float GetCurrentHp() => currentHp;
    public float GetMaxHp() => combatStats.hpMax;
    public bool IsDead() => isDead;
}