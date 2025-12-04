using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    public MonsterIdleState(Monster monster) : base(monster){}

    private Vector3 nextPatrolPosition;
    Vector3 sectionMaxPos;
    Vector3 sectionMinPos;

    private float moveSpeed;
    private float curSpeed;

    private bool canPatrol = false;
    private float nextPatrolTime;
    private float patrolColldown = 3f;
    private float rotateSpeed = 3f;
    
    public override void OnStateEnter()
    {
        if (Array.Exists(_monster.monsterData.monsterbehaviors, element => element == MonsterBehavior.Patrol))
        {
            canPatrol = true;
        }
        // 임시 섹션 범위
        sectionMaxPos = _monster.transform.position + new Vector3(10f, 0f , 10f);
        sectionMinPos = _monster.transform.position + new Vector3(-10f, 0f , -10f);
        nextPatrolPosition = _monster.transform.position;
        moveSpeed = _monster.monsterData.moveSpeed / 2;
        curSpeed = moveSpeed;
    }
    public override void OnStateUpdate()
    {
        if (canPatrol)
        {
            Patrol();
        }     
    }
    public override void OnStateExit()
    {
        
    }

    private void Patrol()
    {
        if(nextPatrolPosition == _monster.transform.position)
        {
            if(Time.time >= nextPatrolTime)
            {              
                nextPatrolPosition = RandPatrolPosition();         
            }
        }
        else
        {
            Vector3 direction = (nextPatrolPosition - _monster.transform.position).normalized;

            if(Vector3.Distance(nextPatrolPosition, _monster.transform.position) > 0.1f)
            {
                curSpeed = Mathf.Lerp(curSpeed, moveSpeed, Time.deltaTime);
                _monster.transform.position += direction * curSpeed * Time.deltaTime;
                LookAtDirection();
                nextPatrolTime = Time.time + patrolColldown;
            }
            else
            {
                curSpeed = Mathf.Lerp(curSpeed, 0f, Time.deltaTime);
                _monster.transform.position = nextPatrolPosition;
            }         
        }

    }

    private Vector3 RandPatrolPosition()
    {
        return new Vector3(UnityEngine.Random.Range(sectionMinPos.x, sectionMaxPos.x), 0, UnityEngine.Random.Range(sectionMinPos.z, sectionMaxPos.z)); 
    }


    void LookAtDirection()
    {
        Vector3 dir = nextPatrolPosition - _monster.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        Quaternion target = Quaternion.LookRotation(dir);
        _monster.transform.rotation = Quaternion.Slerp(_monster.transform.rotation, target, rotateSpeed * Time.deltaTime);
    }
}
