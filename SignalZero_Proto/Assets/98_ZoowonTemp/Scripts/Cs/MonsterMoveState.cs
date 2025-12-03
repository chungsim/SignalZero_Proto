using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class MonsterMoveState : MonsterBaseState
{
    public MonsterMoveState(Monster monster) : base(monster){}

    public Transform playerTransform;
    private bool canMove = true;
    private LayerMask monsterLayer = LayerMask.GetMask("Monster");

    private MonsterBehavior monsterBehavior;

    private float moveSpeed;
    private float curSpeed;
    public float separateDistance;
    public float attackRangeGap = 0.25f;
    private float rotateSpeed = 3f;
    
    public override void OnStateEnter()
    {
        moveSpeed = _monster.monsterData.moveSpeed;
        curSpeed = moveSpeed;
        separateDistance = _monster.monsterData.attackRange / 2;
        playerTransform = GameManager.Instance.characterManager.GetPlayerTransform();

        if(Array.Exists(_monster.monsterData.monsterbehaviors, element => element == MonsterBehavior.Chase))
        {
            monsterBehavior = MonsterBehavior.Chase;
        }
        else if (Array.Exists(_monster.monsterData.monsterbehaviors, element => element == MonsterBehavior.Flee))
        {
            monsterBehavior = MonsterBehavior.Flee;
        }
    }
    public override void OnStateUpdate()
    {
        if (canMove)
        {
            
            if(monsterBehavior == MonsterBehavior.Chase)
            {
                Chase();
            }
            else if (monsterBehavior == MonsterBehavior.Flee)
            {
                Flee();
            }
            
            AvoidOtherMonsters();
        }    
    }
    public override void OnStateExit()
    {
        playerTransform = null;
    }

    public void Chase()
    {

        // 플레이어 위치로 이동
        Vector3 direction = (playerTransform.position - _monster.transform.position).normalized;

        if(Vector3.Distance(playerTransform.position, _monster.transform.position) > _monster.monsterData.attackRange)
        {
            curSpeed = Mathf.Lerp(curSpeed, moveSpeed, Time.deltaTime);
            _monster.transform.position += direction * curSpeed * Time.deltaTime;
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, 0f, Time.deltaTime);
        }

        LookAtPlayer();
        
        // Quaternion quaternion =  Quaternion.LookRotation(direction);
        // _monster.transform.rotation = quaternion;
    }
    public void Flee()
    {
       // 플레이어 반대 위치로 이동
        Vector3 direction = (playerTransform.position - _monster.transform.position).normalized;

        if(Vector3.Distance(playerTransform.position, _monster.transform.position) < _monster.monsterData.detectRange)
        {
            curSpeed = Mathf.Lerp(curSpeed, moveSpeed, Time.deltaTime);
            _monster.transform.position -= direction * curSpeed * Time.deltaTime;
        }
        else
        {
            curSpeed = Mathf.Lerp(curSpeed, 0f, Time.deltaTime);
        } 

        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        Vector3 dir = playerTransform.position - _monster.transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        Quaternion target = Quaternion.LookRotation(dir);
        _monster.transform.rotation = Quaternion.Slerp(_monster.transform.rotation, target, rotateSpeed * Time.deltaTime);
    }

    void AvoidOtherMonsters()
    {
        Collider[] neighbors = Physics.OverlapSphere(_monster.transform.position, separateDistance, monsterLayer);

        Vector3 separationForce = Vector3.zero;

        foreach (Collider col in neighbors)
        {
            if (col.transform == _monster.transform) continue;

            Vector3 diff = _monster.transform.position - col.transform.position;

            diff.y = 0f;

            float distance = diff.magnitude;

            if (distance > 0f && distance < separateDistance)
            {
                float force = separateDistance - distance;
                separationForce += diff.normalized * force;
            }
        }

        _monster.transform.position += separationForce * Time.deltaTime * 1.5f;
    }
}
