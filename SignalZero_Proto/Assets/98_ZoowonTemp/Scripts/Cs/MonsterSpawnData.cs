using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterPosPair
{
    public GameObject monsterPrefab;
    public Vector3 spawnLocalPos;
}

[CreateAssetMenu(fileName = "MonsterSpawnData", menuName = "GameData/Monster Spawn Data")]
public class MonsterSpawnData : ScriptableObject
{
    public List<MonsterPosPair> monsterPosPairs = new List<MonsterPosPair>();
}
