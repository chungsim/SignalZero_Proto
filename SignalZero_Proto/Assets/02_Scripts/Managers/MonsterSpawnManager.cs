using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{

    [Header("SO")]
    [SerializeField] private MonsterDatas monsterDatas;

    [SerializeField] private List<Field> visitedFields;
    [SerializeField] private Transform curFieldTarnsform;
    [SerializeField] private List<MonsterSpawnData> monsterSpawnDatas;

    [Header("Boss")]
    [SerializeField] private int bossSpawnCount;
    [SerializeField] private int curMidKillCount;
    [SerializeField] private float bossSpawnDistance;
    [SerializeField] private int maxMinionNum;
    [SerializeField] private int curMinionNum;

    [Header("Drop")]
    [SerializeField] private GameObject[] dropItems;

    void Awake()
    {
        GameManager.Instance.monsterSpawnManager = this;
    }
    void Start()
    {
        visitedFields.Clear();
    }

    public bool AddField(Field curField)
    {
        // 처음 방문한 필드이면 리스트에 적재, 현재 필드 업데이트
        if (!visitedFields.Contains(curField))
        {
            visitedFields.Add(curField);
            curFieldTarnsform = curField.transform;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SpawnMonsters(MonsterSpawnData monsterSpawnData)
    {
        foreach(MonsterPosPair monsterPosPair in monsterSpawnData.monsterPosPairs)
        {
            // 몬스터 프리펩 스폰
            GameObject go = Instantiate(monsterPosPair.monsterPrefab, transform);

            // 몬스터 위치 적용
            go. transform.position = curFieldTarnsform.position + monsterPosPair.spawnLocalPos;
        }    
    }

    public void SpawnMinion(Transform bossTransform, int num)
    {

        for(int i = 0; i < num; i++)
        {
            // 미니언 최대 수 도달 시 스폰 정지
            if(curMinionNum > maxMinionNum) return;

            Vector3 spawnPos = bossTransform.position + bossTransform.forward * -UnityEngine.Random.Range(10, 30) + bossTransform.right * UnityEngine.Random.Range(-20  , 20) ;
            GameObject go = Instantiate(monsterSpawnDatas[3].monsterPosPairs[0].monsterPrefab, transform);
            go. transform.position = spawnPos;
            curMinionNum++;
        }  
    }

    public void SpawnDrone(int num)
    {
        Transform playerTransform = GameManager.Instance.characterManager.GetPlayerTransform();
        for(int i = 0; i < num; i++)
        {
            // 미니언 최대 수 도달 시 스폰 정지
            if(curMinionNum > maxMinionNum) return;

            Vector3 spawnPos = playerTransform.position + playerTransform.forward * -UnityEngine.Random.Range(-50 , 50) + playerTransform.right * UnityEngine.Random.Range(-50  , 50) ;
            GameObject go = Instantiate(monsterSpawnDatas[1].monsterPosPairs[0].monsterPrefab, transform);
            go. transform.position = spawnPos;
            curMinionNum++;
        }  
    }

    public void killMinion()
    {
        curMinionNum--;
    }

    public void SpawnBoss()
    {
        Field randField;
        while (true)
        {
            randField = GameManager.Instance.fieldManager.spawnFields[UnityEngine.Random.Range(0, GameManager.Instance.fieldManager.spawnFields.Count)].GetComponent<Field>();

            if( !visitedFields.Contains(randField) && Vector3.Distance(randField.transform.position, GameManager.Instance.characterManager.GetPlayerTransform().position) > bossSpawnDistance)
            {
                break;
            }
        }
        
        GameObject go = Instantiate(monsterSpawnDatas[2].monsterPosPairs[0].monsterPrefab, transform);
        go.transform.position = randField.transform.position;
        ActiveBossHp();

        // 필드 메니저에서     
    }

    private void ActiveBossHp()
    {
        // GameManager.Instance.uiManager.bossHPBar.gameObject.SetActive(true);
        // GameManager.Instance.uiManager.bossHPBackground.gameObject.SetActive(true);
        GameManager.Instance.uiManager.bossHPObject.SetActive(true);
    }

    private void DeactiveBossHp()
    {
        GameManager.Instance.uiManager.bossHPBar.gameObject.SetActive(false);
        GameManager.Instance.uiManager.bossHPBackground.gameObject.SetActive(false);
    }

    public void AddMidKillCount()
    {
        curMidKillCount++;

        if(curMidKillCount == bossSpawnCount)
        {
            SpawnBoss();
        }
    }

    public void SpawnWeaponItem(Vector3 pos)
    {
        GameObject go = Instantiate(dropItems[UnityEngine.Random.Range(0, dropItems.Length)]);
        go.transform.position = pos;
    }

    public void ClearAllMonster()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        for(int i = allChildren.Length -1 ; i >= 0; i--)
        {
            Destroy(allChildren[i].gameObject);
        }
    }

}
