using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateImpactFX(Vector3 pos, BulletSO data) // 충돌 시 FX 적용 예정
    {
        // TODO: FX 풀링으로 전환 가능
        Debug.Log($"Impact FX at {pos}");
    }

    public void ApplyDamage(Collider target, float dmg)
    {
        // 충돌한 대상에서 IDamageAble 구현체 찾기
        IDamageAble damageable = target.GetComponentInParent<IDamageAble>();

        if (damageable != null)
        {
            damageable.GetDamage((int)dmg);
        }
        else
        {
            // 데미지를 받을 수 없는 오브젝트
            Debug.Log($"대상 {target.name},은 IDamageAble이 아님" );
        }
    }


}
