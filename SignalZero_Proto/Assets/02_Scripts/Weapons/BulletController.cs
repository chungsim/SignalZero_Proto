using UnityEngine;
using System;

public class BulletController : MonoBehaviour, IPoolable
{
    private Action<GameObject> returnToPool;

    // 충돌 처리, FX 처리하는 매니저
    private BulletManager bulletManager;

    // 풀 식별용 키
    public int PoolKey { get; private set; }

    // ScriptableObject 데이터
    private BulletSO data;

    // 이동 관련 값
    private Vector3 direction;
    private Vector3 spawnPos;
    private float traveledDistance;

    private Rigidbody rb;

    // -----------------------------------------
    //  Lazy 방식: BulletManager가 늦게 생성돼도 문제 없게
    // -----------------------------------------
    private void EnsureManager()
    {
        if (bulletManager == null)
            bulletManager = BulletManager.Instance;
    }

    // 최초 Instantiate 시 1회 호출
    public void Initialize(Action<GameObject> returnAction, int poolKey)
    {
        returnToPool = returnAction;
        PoolKey = poolKey;

        EnsureManager();
    }

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
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
        // 필요 시 Trail, 파티클 초기화
    }

    public void OnDespawn()
    {
        // Pool 복귀 전 정리
    }

    // -----------------------------------------
    //  메인 이동 로직 — MovePosition 사용
    // -----------------------------------------
    private void FixedUpdate()
    {
        EnsureManager();

        if (data == null) return;

        float move = data.bulletSpeed * Time.fixedDeltaTime;
        Vector3 nextPos = rb.position + direction * move;
        rb.MovePosition(nextPos);

        traveledDistance += move;

        if (traveledDistance >= data.range)
        {
            Die(transform.position, true);
        }
    }


    // -----------------------------------------
    //  Trigger 충돌 처리
    // -----------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        EnsureManager();

        if (data == null || bulletManager == null)
        {
            returnToPool?.Invoke(gameObject);
            return;
        }

        // 피격 사운드 처리
        bulletManager.OnBulletImpact(transform.position, data);

        // 데미지 처리
        bulletManager.ApplyDamage(other, data.damagePerShot);

        // 즉시 사라짐
        Die(transform.position, true);
    }

    // -----------------------------------------
    //  제거 처리 (FX + Pool 반환)
    // -----------------------------------------
    private void Die(Vector3 pos, bool createFx)
    {
        EnsureManager();

        if (createFx)
            bulletManager.CreateImpactFX(pos, data);

        returnToPool?.Invoke(gameObject);
    }
}




