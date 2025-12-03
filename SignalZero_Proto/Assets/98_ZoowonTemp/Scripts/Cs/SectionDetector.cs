using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionDetector : MonoBehaviour
{
    [SerializeField] private float detectRange = 1f;
    [SerializeField] private float detectDuration = 0.2f;
    [SerializeField] private List<MonsterSpawnData> monsterSpawnDatas;
    private Field curField = null;
    private Coroutine curCoroutine = null;

    void Start()
    {
        StartDetector();    
    }
    public void StartDetector()
    {
        curCoroutine = StartCoroutine(DetectSection());
    }

    IEnumerator DetectSection()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hitData;
        if(Physics.Raycast(ray, out hitData, detectRange))  // 레이어 마스크 적용 예정
        {
            Field field;
            if(hitData.collider.TryGetComponent<Field>(out field))
            {
                if (GameManager.Instance.monsterSpawnManager.AddField(field))
                {
                    // 몬스터 스폰 구분, 추후 SO를 이용한 인덱스나 사전으로 교체 필요
                    switch (field.type)
                    {
                        case FieldType.radioShip:
                            Debug.Log("RadioShip Section");
                            GameManager.Instance.monsterSpawnManager.SpawnMonsters(monsterSpawnDatas[0]);
                            break;
                        
                        case FieldType.common:
                            Debug.Log("Common Section");
                            GameManager.Instance.monsterSpawnManager.SpawnMonsters(monsterSpawnDatas[1]);
                            break;
                    }
                }                
            }
        }
        yield return new WaitForSeconds(detectDuration);
        curCoroutine = StartCoroutine(DetectSection());
    }


}
