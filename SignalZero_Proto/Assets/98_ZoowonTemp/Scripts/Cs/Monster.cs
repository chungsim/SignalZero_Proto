using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour, IDamageAble
{
    public MonsterData monsterData;
    private float stateUpdateDuration = 0.1f;

    public int maxHp;
    public int curHp;

    [SerializeField] private Image hpGauge;

    [SerializeField] private MonsterState curState;
    private MonsterFsm monsterFsm;
    private bool isChasing = false;

    public Transform playerTransform;

    public LayerMask monsterLayer;

    private void Start()
    {
        // 상태 변화 체크 코루틴 시작
        playerTransform = GameManager.Instance.characterManager.GetPlayerTransform();
        LoadConditions();
        monsterFsm = new MonsterFsm(new MonsterIdleState(this));
        StartCoroutine(StateRoutine());
        
    }

    private void FixedUpdate()
    {
        monsterFsm.UpdateState();
    }

    private void LoadConditions()
    {
        maxHp = monsterData.maxHp;
        curHp = maxHp;
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
            else if (IsPlayerInChaseRange() || isChasing)
            {
                ChangeState(MonsterState.Move);
                isChasing = true;
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
        if(Vector3.Distance(playerTransform.position, transform.position) <= monsterData.attackRange && monsterData.monsterRole == MonsterRoles.Minion)
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

    public void GetDamage(int damage)
    {
        int result = curHp - damage;
        UpdateHpGauge();
        if(monsterData.monsterRole == MonsterRoles.Boss && GameManager.Instance.uiManager.bossHPBar.gameObject.activeInHierarchy)
        {
            UpdateBossHp();
        }

        if(curHp > 0)
        {
            curHp = result > 0 ? result : 0;

            if(curHp <= 0)
            {
                Die();
            }
        }  
    }

    private void UpdateHpGauge()
    {
        if(hpGauge != null)
        {
            hpGauge.fillAmount = (float)curHp / (float)maxHp;
        }
    }

    private void Die()
    {
        // 중간 보스 킬 카운트
        if(monsterData.monsterRole == MonsterRoles.MidBoss)
        {
            GameManager.Instance.monsterSpawnManager.AddMidKillCount();
            GameManager.Instance.monsterSpawnManager.SpawnWeaponItem(transform.position);
        }

        if(monsterData.monsterRole == MonsterRoles.Minion)
        {
            GameManager.Instance.monsterSpawnManager.killMinion();
        }

        if(monsterData.monsterRole == MonsterRoles.Boss)
        {
            // 게임 메니저의 게임 종료 연결
        }
        
        Destroy(gameObject);
    } 

    private void UpdateBossHp()
    {
        GameManager.Instance.uiManager.bossHPBar.fillAmount = (float)curHp / (float)maxHp;
    }

}
