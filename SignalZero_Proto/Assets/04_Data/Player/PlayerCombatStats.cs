using UnityEngine;

/// <summary>
/// 플레이어 전투, 버스트, 부스터, 게이지, HP 관련 스탯
/// </summary>
[CreateAssetMenu(fileName = "PlayerCombatStats", menuName = "Player/Combat Stats")]
public class PlayerCombatStats : ScriptableObject
{
    [Header("=== 체력 ===")]
    [Tooltip("체력 최대치 - 플레이어가 적에게 피해를 입고 버틸 수 있는 최대치")]
    public float hpMax = 1000f;

    [Header("=== 버스트 (대쉬) ===")]
    [Tooltip("버스트(대쉬) 거리 - 버스트 발동 시 순간 이동하는 거리")]
    public float burstRange = 8f;

    [Tooltip("버스트 딜레이 - 버스트 발동 전 딜레이 시간 (이 시간 동안 감속)")]
    public float burstDelay = 0.5f;

    [Tooltip("버스트 딜레이 감쇠 - 버스트 딜레이 중 이동 속도 배율 (0.4 = 40% 속도)")]
    public float burstSlow = 0.4f;

    [Tooltip("버스트 소모량 - 버스트 1회 발동 시 소모하는 게이지")]
    public float burstCon = 100f;

    [Tooltip("버스트 쿨타임 - 버스트 재사용 대기 시간")]
    public float burstCool = 0.5f;

    [Header("=== 부스터 (달리기) ===")]
    [Tooltip("부스터(달리기) 속도 - 부스터 상태일 때의 이동 속도")]
    public float boosterSpeed = 24f;

    [Tooltip("부스터 소모량 - 부스터 상태일 때 델타타임당 소모하는 게이지 (초당 약 50)")]
    public float boosterCon = 5f;

    [Header("=== 게이지 (에너지) ===")]
    [Tooltip("최대 게이지(마나) - 버스트와 부스터를 발동하기 위한 에너지의 총량")]
    public float gaugeMax = 500f;

    [Tooltip("게이지 회복속도 - 게이지가 델타타임당 회복되는 양 (초당 약 500)")]
    public float gaugeRecovery = 50f;

    [Tooltip("게이지 회복 딜레이 - 버스트/부스터 사용 후 게이지 회복 시작까지 대기 시간")]
    public float gaugeRegen = 0.5f;

    [Tooltip("제로 게이지 시 회복 락 - 게이지가 0이 되었을 때 회복 불가능한 시간")]
    public float gaugeLock = 2f;
}