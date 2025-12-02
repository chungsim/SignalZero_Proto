using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("무기 장착 슬롯 (Player 하위에 직접 배치)")]
    [SerializeField] private Transform[] weaponSlots = new Transform[4];

    private Weapon[] weapons = new Weapon[4];     // 슬롯에 붙은 Weapon 스크립트
    private WeaponSO[] weaponData = new WeaponSO[4]; // 장착된 무기 데이터

    void Awake()
    {
        // 슬롯 안의 Weapon 가져오기
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] != null)
                weapons[i] = weaponSlots[i].GetComponentInChildren<Weapon>();
        }
    }

    //테스트용 발사

    // -----------------------------
    // (1) 무기 장착
    // -----------------------------
    public bool TryEquipWeapon(WeaponSO data)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponData[i] == null)
            {
                weaponData[i] = data;

                if (weapons[i] != null)
                {
                    // Weapon.cs 안에서 모델/사운드/탄종 등을 처리할 예정
                    weapons[i].LoadData(data);
                }

                return true;
            }
        }

        // 빈 슬롯 없음
        return false;
    }

    // -----------------------------
    // (2) 모든 무기 발사
    // -----------------------------
    public void FireAllWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weaponData[i] != null && weapons[i] != null)
            {
                weapons[i].TryShoot();
            }
        }
    }
}
