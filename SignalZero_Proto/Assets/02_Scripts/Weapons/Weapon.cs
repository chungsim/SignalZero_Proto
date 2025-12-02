using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private float lastFireTime = 0f;

    // 무기 데이터 적용
    public void LoadData(WeaponSO data)
    {
        weaponData = data;

        // 나중에 modelID를 기반으로 모델링 교체
        // 또는 사운드, 플래시 등을 여기서 준비
    }

    // 발사 시도
    public void TryShoot()
    {
        if (weaponData == null) return;

        if (Time.time - lastFireTime < weaponData.cooldown)
            return;

        lastFireTime = Time.time;
        Shoot();
    }

    // 실제 발사
    private void Shoot()
    {
        for (int i = 0; i < weaponData.numPerShot; i++)
        {
            float angle = Random.Range(-weaponData.spreadAngle, weaponData.spreadAngle);
            Quaternion rot = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject bulletObj = ObjectPoolManager.Instance.GetObject(weaponData.projectileTypeID, transform.position, rot);

            BulletController bullet = bulletObj.GetComponent<BulletController>();
            bullet.SetData(weaponData.bulletData);

            Vector3 dir = rot * Vector3.forward; //무기 오브젝트 정면 기준으로 탄환 생성
            bullet.Init(dir);
        }
    }
}



