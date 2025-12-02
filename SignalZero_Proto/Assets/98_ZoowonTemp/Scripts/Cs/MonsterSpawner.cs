using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    private GameObject monsterPrefab;

    public void SpawnMonster(MonsterData monsterData)
    {
        GameObject go = Instantiate(monsterPrefab, transform);
        Monster monster;
        if(go.TryGetComponent<Monster>(out monster))
        {
            monster.monsterData = monsterData;
        }
    }
}
