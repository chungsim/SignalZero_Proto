using UnityEngine;
using System;

public class BulletController : MonoBehaviour, IPoolable
{
    private Action<GameObject> returnToPool;

    // 충돌 처리, FX 처리하는 매니저
    private BulletManager bulletManager;

    // 풀 식별용 키 _ WeaponSO.projectileTypeID 에 맞춰서 생성됨
    public int PoolKey { get; private set; }

    // ScriptableObject 데이터
    private BulletSO data;

    // 이동 관련 값
    private Vector3 direction;
    private Vector3 spawnPos;
    private float traveledDistance;

    // BulletManager를 Lazy 방식으로 가져오기 (Awake 순서 문제 대응)
    private void EnsureManager()
    {
        if (bulletManager == null)
            bulletManager = BulletManager.Instance;
    }

    // Instantiate 시 최초 1회 호출됨
    public void Initialize(Action<GameObject> returnAction, int poolKey)
    {
        returnToPool = returnAction;
        PoolKey = poolKey;

        // BulletManager.Instance가 아직 생성 안 되었을 가능성이 있음 → Lazy 방식 필요
        EnsureManager();
    }

    // ScriptableObject 데이터 입력
    public void SetData(BulletSO bulletData)
    {
        data = bulletData;
        transform.localScale = Vector3.one * bulletData.bulletSize;
    }

    // 발사 시점에서 방향 설정
    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        spawnPos = transform.position;
        traveledDistance = 0f;
    }

    public void OnSpawn()
    {
        // 필요 시 TrailRenderer, 파티클 초기화 등 넣을 자리
    }

    public void OnDespawn()
    {
        // 풀 복귀 직전 정리 필요 시 사용
    }

    private void Update()
    {
        EnsureManager();

        // 데이터나 매니저가 준비되지 않으면 이동시키지 않음
        if (data == null || bulletManager == null)
            return;

        float move = data.bulletSpeed * Time.deltaTime;
        transform.position += direction * move;

        traveledDistance += move;

        // 사거리 도달 → FX → Pool 반환
        if (traveledDistance >= data.range)
        {
            Die(transform.position, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        EnsureManager();

        if (data == null || bulletManager == null)
        {
            returnToPool?.Invoke(gameObject);
            return;
        }

        bulletManager.ApplyDamage(other, data.damagePerShot);

        Die(transform.position, true);
    }

    private void Die(Vector3 pos, bool createFx)
    {
        EnsureManager();

        if (createFx)
            bulletManager.CreateImpactFX(pos, data);

        returnToPool?.Invoke(gameObject);
    }
}



