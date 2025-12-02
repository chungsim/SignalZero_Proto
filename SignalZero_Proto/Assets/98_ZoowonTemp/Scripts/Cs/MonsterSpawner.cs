using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private GameObject monsterPrefab;

    public void SpawnMonster(MonsterData monsterData, Vector3 pos)
    {
        GameObject go = Instantiate(monsterPrefab, transform);
        Monster monster;
        if(go.TryGetComponent<Monster>(out monster))
        {
            monster.monsterData = monsterData;
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
        
    }
}
