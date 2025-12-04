using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(Monster monster) : base(monster){}

    private LayerMask playerMask = LayerMask.GetMask("Water"); // 테스트 용

    private float nextAttackTime = 0f;
    private MonsterBehavior curBehavior;
    
    public override void OnStateEnter()
    {
        if(Array.Exists(_monster.monsterData.monsterbehaviors, element => element == MonsterBehavior.Spawn))
        {
            curBehavior = MonsterBehavior.Spawn;
        }
    }
    public override void OnStateUpdate()
    {
        if(curBehavior == MonsterBehavior.Spawn)
        {
            SpawnMinion();
        }
        else
        {
            AttackPlayer();
        }
        
    }
    public override void OnStateExit()
    {
        
    }

    private void AttackPlayer()
    {
        if (Time.time < nextAttackTime) return;

        Ray ray = new Ray(_monster.transform.position, _monster.transform.forward);
        RaycastHit hitData;
            
        if(Physics.Raycast(ray, out hitData, _monster.monsterData.attackRange))
        {
            Debug.Log(hitData.collider.name);
            nextAttackTime = Time.time + 1f;
        }          
        
    }

    private void SpawnMinion()
    {
        if (Time.time < nextAttackTime) return;

        GameManager.Instance.monsterSpawnManager.SpawnMinion(_monster.transform, 4);
        nextAttackTime = Time.time + _monster.monsterData.spawnCoolTime;      
    }
}
