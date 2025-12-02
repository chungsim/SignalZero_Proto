using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    [Header("얻을 무기 데이터")]
    public WeaponSO weaponData;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어인지 체크
        PlayerWeaponManager playerWeapon = other.GetComponent<PlayerWeaponManager>();
        if (playerWeapon == null) return;

        // 플레이어에게 무기 장착 시도
        bool equipped = playerWeapon.TryEquipWeapon(weaponData);

        if (equipped)
        {
            // 장착 성공 → 아이템 파괴
            Destroy(gameObject);
        }
        else
        {
            // 슬롯 꽉 찼음 → 장착 실패 (선택사항: UI 표시)
            Debug.Log("무기 슬롯이 꽉 찼습니다.");
        }
    }
}
