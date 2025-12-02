using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolSO", menuName = "Pools/PoolData")]
public class PoolData : ScriptableObject
{
    public int key;                 // WeaponSO.projectileTypeID와 연결
    public GameObject prefab;       // Pool에서 관리할 프리팹
    public int initialCount = 10;   // 초기에 생성할 오브젝트 수
}
