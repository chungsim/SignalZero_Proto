using System;
using UnityEngine;

public interface IPoolable
{
    // 이 오브젝트가 속한 Pool의 키(ID)
    int PoolKey { get; }

    // 오브젝트가 Pool에서 처음 만들어질 때 호출됨
    void Initialize(Action<GameObject> returnAction, int poolKey);

    // Pool에서 꺼낼 때 호출됨
    void OnSpawn();

    // Pool로 돌아가기 직전에 호출됨
    void OnDespawn();
}
