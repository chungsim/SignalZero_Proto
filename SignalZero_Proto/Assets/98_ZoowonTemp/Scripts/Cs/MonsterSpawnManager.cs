using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance;

    [Header("SO")]
    [SerializeField] private MonsterDatas monsterDatas;

    [SerializeField] private List<Field> visitedFields;
    [SerializeField] private Transform curFieldTarnsform;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        visitedFields.Clear();
    }

    public bool AddField(Field curField)
    {
        // 처음 방문한 필드이면 리스트에 적재, 현재 필드 업데이트
        if (!visitedFields.Contains(curField))
        {
            visitedFields.Add(curField);
            curFieldTarnsform = curField.transform;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnMonsters(MonsterSpawnData monsterSpawnData)
    {
        foreach(MonsterPosPair monsterPosPair in monsterSpawnData.monsterPosPairs)
        {
            // 몬스터 프리펩 스폰
            GameObject go = Instantiate(monsterPosPair.monsterPrefab, transform);

            // 몬스터 위치 적용
            go. transform.position = curFieldTarnsform.position + monsterPosPair.spawnLocalPos;
        }    
    }

}
