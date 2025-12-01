using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterRoles{Minion, MidBoss, Boss}
public enum Monsterbehavior{Patrol, Chase, Flee}

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
    public int contactDps;
    public Monsterbehavior[] monsterbehaviors;
}

[CreateAssetMenu(fileName = "MonsterDatas", menuName = "GameData/Monster Datas")]
public class MonsterDatas : ScriptableObject
{
    public List<MonsterData> monsterDatas = new List<MonsterData>(); 
}
