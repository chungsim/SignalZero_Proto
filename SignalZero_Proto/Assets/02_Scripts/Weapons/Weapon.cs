using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public WeaponSO weaponData;

    private float lastFireTime = 0f;

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        TryShoot();
    }

    private void TryShoot()
    {
        if (Time.time - lastFireTime < weaponData.cooldown)
            return;

        lastFireTime = Time.time;
        Shoot();
    }

    private void Shoot()
    {
        for (int i = 0; i < weaponData.numPerShot; i++)
        {
            float angle = Random.Range(-weaponData.spreadAngle, weaponData.spreadAngle);
            Quaternion rot = transform.rotation * Quaternion.Euler(0, angle, 0);

            GameObject bulletObj = ObjectPoolManager.Instance.GetObject(
                weaponData.projectileTypeID,
                transform.position,
                rot
            );

            BulletController bullet = bulletObj.GetComponent<BulletController>();
            bullet.SetData(weaponData.bulletData);

            Vector3 dir = rot * Vector3.forward;
            bullet.Init(dir);
        }
    }
}



