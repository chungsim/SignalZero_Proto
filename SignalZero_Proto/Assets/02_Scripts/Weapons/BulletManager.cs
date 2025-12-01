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
        // TODO: HP가 있는 대상이라면 HP 감소 처리
        Debug.Log($"Damage {dmg} to {target.name}");
    }

}
