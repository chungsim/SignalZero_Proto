using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    public MonsterAttackState(Monster monster) : base(monster){}

    private LayerMask playerMask = LayerMask.GetMask("Water"); // 테스트 용

    private float nextAttackTime = 0f;
    
    public override void OnStateEnter()
    {
        
    }
    public override void OnStateUpdate()
    {
        AttackPlayer();
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
}
