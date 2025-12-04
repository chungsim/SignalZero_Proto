using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponSO weaponData;
    [SerializeField] private float lastFireTime = 0f;
    [SerializeField] private Transform modelRoot;
    [SerializeField] private WeaponModelDatabase modelDB;

    // 무기 데이터 적용
    public void LoadData(WeaponSO data)
    {
        weaponData = data;
        // 기존 모델 삭제

        foreach (Transform child in modelRoot)
            Destroy(child.gameObject);

        // 새 모델 로드 (나중에 Addressables)
        GameObject model = modelDB.Get(data.modelID);
        if (model != null) Instantiate(model, modelRoot);

 
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
        // 사운드

        // 발사 시 SFX 출력
        GameManager.Instance.audioManager.PlaySFX(
            weaponData.audioData.fireSFX,
            weaponData.audioData.fireVolume
        );


        // 탄환 생성
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



