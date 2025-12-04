using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;

    private AudioManager audioManager;
    private void Awake()
    {
        Instance = this;
        
    }

    // -----------------------------------------
    //  Lazy 방식으로 혹시 모를 Null 방지
    // -----------------------------------------
    private void EnsureAudioManager()
    {
        if (audioManager == null)
        {
            if (GameManager.Instance != null)
                audioManager = GameManager.Instance.audioManager;
        }
    }

    // FX 묶음 실행 함수
    public void CreateImpactFX(Vector3 pos, BulletSO data) // 충돌 시 FX 적용 예정
    {
        // TODO: FX 풀링으로 전환 가능
        Debug.Log($"Impact FX at {pos}");
    }

    // 총알 충돌 시 FX/SFX 실행
    public void OnBulletImpact(Vector3 pos, BulletSO bulletData)
    {
        EnsureAudioManager();

        CreateImpactFX(pos, bulletData);

        if (bulletData?.audiodata?.impactSFX != null)
        {
            audioManager?.PlayLimitedSFX(
                bulletData.audiodata.impactSFX,
                bulletData.audiodata.impactVolume
            );
        }
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
