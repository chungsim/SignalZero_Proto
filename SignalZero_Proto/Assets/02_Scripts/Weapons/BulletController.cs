using UnityEngine;
using System;

public class BulletController : MonoBehaviour, IPoolable
{
    private Action<GameObject> returnToPool;
    private BulletManager bulletManager;

    public int PoolKey { get; private set; }

    private BulletSO data;
    private Vector3 direction;
    private Vector3 spawnPos;
    private float traveledDistance;

    // 최초 Pool 생성 시 1회 호출
    public void Initialize(Action<GameObject> returnAction, int poolKey)
    {
        returnToPool = returnAction;
        PoolKey = poolKey;

        bulletManager = BulletManager.Instance;
    }

    public void SetData(BulletSO bulletData)
    {
        data = bulletData;
        transform.localScale = Vector3.one * bulletData.bulletSize;
    }

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        spawnPos = transform.position;
        traveledDistance = 0f;
    }

    public void OnSpawn()
    {
        // 필요하면 여기에 초기화 코드
    }

    public void OnDespawn()
    {
        // 풀 복귀 직전 처리
    }

    private void Update()
    {
        if (data == null || bulletManager == null)
            return;

        float move = data.bulletSpeed * Time.deltaTime;
        transform.position += direction * move;

        traveledDistance += move;

        if (traveledDistance >= data.range)
        {
            Die(transform.position, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
        if (createFx)
            bulletManager.CreateImpactFX(pos, data);

        returnToPool?.Invoke(gameObject);
    }
}


