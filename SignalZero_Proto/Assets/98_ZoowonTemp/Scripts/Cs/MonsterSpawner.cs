using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private MonsterDatas monsterDatas;

    public void SpawnMonster(int index, Vector3 pos)
    {
        GameObject go = Instantiate(monsterDatas.monsterPrefabList[index], transform);
        Monster monster;
        if(go.TryGetComponent<Monster>(out monster))
        {
            monster.monsterData = monsterDatas.monsterDataList[index];
        }
        else
        {
            Debug.Log($"{go.name} has no Monster component!!");
        }
        go. transform.position = pos;
    }

    private void LoadMonsterModel()
    {
        // 몬스터 코드에 맞는 모델링 적용
        // 모델링 리소스 
    }
}
