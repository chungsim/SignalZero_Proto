using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterRoles {Minion, MidBoss, Boss}
public enum MonsterBehavior {Patrol, Chase, Flee, Spawn}
public enum MonsterState {Idle, Move, Attack}

[System.Serializable]
public class MonsterData
{
    public int id;
    public string name;
    public MonsterRoles monsterRole;
    public int maxHp;
    public int curHp;
    public float moveSpeed;
    public float detectRange;
    public float attackRange;
    public int contactDps;
    public MonsterBehavior[] monsterbehaviors;
    public float spawnCoolTime;
    public GameObject boss;
}

[CreateAssetMenu(fileName = "MonsterDatas", menuName = "GameData/Monster Datas")]
public class MonsterDatas : ScriptableObject
{
    public List<MonsterData> monsterDataList = new List<MonsterData>();
    public List<GameObject> monsterPrefabList = new List<GameObject>();
}
