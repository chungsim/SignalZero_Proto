using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    private float stateUpdateDuration = 0.1f;

    public int maxHp;
    public int curHp;

    [SerializeField] private MonsterState curState;
    private MonsterFsm monsterFsm;

    //temp
    public Transform playerTransform;

    public LayerMask monsterLayer;

    private void Start()
    {
        // 상태 변화 체크 코루틴 시작
        playerTransform = GameObject.Find("TestPlayer").transform;
        monsterFsm = new MonsterFsm(new MonsterIdleState(this));
        StartCoroutine(StateRoutine());
        
    }
    private void FixedUpdate()
    {
        monsterFsm.UpdateState();
    }

    private void ChangeState(MonsterState nextState)
    {
        if(curState == nextState) return;
        curState = nextState;

        switch (curState)
        {
            case MonsterState.Idle:
                monsterFsm.ChangeState(new MonsterIdleState(this));
                break;

            case MonsterState.Move:
                monsterFsm.ChangeState(new MonsterMoveState(this));
                break;

            case MonsterState.Attack:
                monsterFsm.ChangeState(new MonsterAttackState(this));
                break;
        }
    }

    // 주기마다 상태변화 조건 체크
    IEnumerator StateRoutine()
    {
        bool flag = true;

        while (flag)
        {
            if (IsPlayerInAttackRange())
            {
                ChangeState(MonsterState.Attack);
            }
            else if (IsPlayerInChaseRange())
            {
                ChangeState(MonsterState.Move);
            }
            else
            {
                ChangeState(MonsterState.Idle);
            }

            yield return new WaitForSeconds(stateUpdateDuration);
        }

        yield return null;
    }

    // 플레이어가 공격범위에 들어 왔을 때
    private bool IsPlayerInAttackRange()
    {
        if(Vector3.Distance(playerTransform.position, transform.position) <= monsterData.attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }   
    }

    // 플레이어가 추격 범위 안에 들어 왔을 때
    private bool IsPlayerInChaseRange()
    {
        if(Vector3.Distance(playerTransform.position, transform.position) < monsterData.detectRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int damage)
    {
        int result = curHp - damage;
        curHp = result > 0 ? result : 0;

        if(curHp <= 0)
        {
            Die();
        } 
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
