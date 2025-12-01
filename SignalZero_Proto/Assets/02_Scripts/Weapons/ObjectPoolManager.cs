using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    private Transform poolRoot;

    [Serializable]
    public class PoolData
    {
        public int key;                 // WeaponSO.projectileTypeID와 연결
        public GameObject prefab;       // Pool에서 관리할 프리팹
        public int initialCount = 10;   // 초기에 생성할 오브젝트 수
    }

    [SerializeField] private List<PoolData> poolSettings = new();

    private Dictionary<int, Queue<GameObject>> pools = new();
    private Dictionary<int, GameObject> prefabLookup = new();

    private void Awake()
    {
        Instance = this;
        poolRoot = new GameObject("PoolContainer").transform;
        poolRoot.SetParent(transform);

        InitializePools();
    }

    // 초기 Pool 생성 (씬 로딩 시 1회 수행)
    private void InitializePools()
    {
        foreach (var pool in poolSettings)
        {
            prefabLookup[pool.key] = pool.prefab;
            pools[pool.key] = new Queue<GameObject>();

            // 초기 오브젝트 미리 생성
            for (int i = 0; i < pool.initialCount; i++)
            {
                GameObject obj = CreateInstance(pool.key);
                obj.SetActive(false);
                pools[pool.key].Enqueue(obj);
            }
        }
    }

    // Instantiate + IPoolable.Initialize
    private GameObject CreateInstance(int key)
    {
        GameObject obj = Instantiate(prefabLookup[key], poolRoot);

        // 각 풀 오브젝트는 자신이 어떤 풀에 속하는지 key로 기억함
        var poolable = obj.GetComponent<IPoolable>();
        poolable?.Initialize(ReturnToPool, key);

        return obj;
    }

    // Pool에서 꺼내기 (발사 시 호출)
    public GameObject GetObject(int key, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();

        // Queue가 비었으면 새로 Instantiate
        GameObject obj = pools[key].Count > 0
            ? pools[key].Dequeue()
            : CreateInstance(key);

        // 배치
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);

        // Pool에서 나온 직후 처리
        obj.GetComponent<IPoolable>()?.OnSpawn();

        return obj;
    }

    // Pool로 복귀시키기
    private void ReturnToPool(GameObject obj)
    {
        var poolable = obj.GetComponent<IPoolable>();
        int key = poolable.PoolKey;

        poolable.OnDespawn();

        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}



