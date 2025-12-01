using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [Serializable]
    public class PoolData
    {
        public int key;
        public GameObject prefab;
        public int initialCount = 10;
    }

    [SerializeField] private List<PoolData> poolSettings = new();

    private Dictionary<int, Queue<GameObject>> pools = new();
    private Dictionary<int, GameObject> prefabLookup = new();

    private void Awake()
    {
        Instance = this;
        InitializePools();
    }

    // 초기 Pool 생성
    private void InitializePools()
    {
        foreach (var pool in poolSettings)
        {
            prefabLookup[pool.key] = pool.prefab;
            pools[pool.key] = new Queue<GameObject>();

            for (int i = 0; i < pool.initialCount; i++)
            {
                GameObject obj = CreateInstance(pool.key);
                obj.SetActive(false);
                pools[pool.key].Enqueue(obj);
            }
        }
    }

    // key 기반으로 신규 객체 생성
    private GameObject CreateInstance(int key)
    {
        GameObject obj = Instantiate(prefabLookup[key]);

        var poolable = obj.GetComponent<IPoolable>();
        poolable?.Initialize(ReturnToPool, key);    // 키 전달

        return obj;
    }

    // pool에서 가져오기
    public GameObject GetObject(int key, Vector3 pos, Quaternion rot)
    {
        if (!pools.ContainsKey(key))
            pools[key] = new Queue<GameObject>();

        GameObject obj = pools[key].Count > 0
            ? pools[key].Dequeue()
            : CreateInstance(key);

        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);

        obj.GetComponent<IPoolable>()?.OnSpawn();

        return obj;
    }

    // pool로 반환
    private void ReturnToPool(GameObject obj)
    {
        var poolable = obj.GetComponent<IPoolable>();
        int key = poolable.PoolKey;

        obj.GetComponent<IPoolable>()?.OnDespawn();

        obj.SetActive(false);
        pools[key].Enqueue(obj);
    }
}


