using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Player/Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("이동 관련")]
    public float minSpeed = 1f;           // 최소 속도
    public float maxSpeed = 10f;          // 최대 속도
    public float maxSpeedDistance = 5f;   // 최고 속도 범위 거리

    [Header("데드라인")]
    public float deadlineRadius = 0.5f;   // 데드라인 반지름

    [Header("버스트")]
    public float burstDistance = 5f;      // 버스트 이동 거리
    public float burstDelay = 0.3f;       // 버스트 딜레이 시간
    public float burstSlowSpeed = 0.4f;   // 버스트 딜레이 중 감속 속도
    public float burstCost = 20f;         // 버스트 게이지 소모량
    public float burstCooldown = 1f;      // 버스트 쿨타임

    [Header("부스터")]
    public float boosterSpeed = 15f;      // 부스터 최고 속도
    public float boosterCostPerTick = 5f; // 0.1초당 게이지 소모량

    [Header("게이지")]
    public float maxGauge = 100f;         // 최대 게이지
    public float gaugeRegenRate = 10f;    // 초당 게이지 회복량
    public float gaugeRegenDelay = 0.5f;  // 게이지 회복 딜레이
    public float gaugeZeroLockTime = 2f;  // 게이지 0일 때 회복 락 시간
}
