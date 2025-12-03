using UnityEngine;

/// <summary>
/// 플레이어 이동 관련 스탯
/// </summary>
[CreateAssetMenu(fileName = "PlayerMovementStats", menuName = "Player/Movement Stats")]
public class PlayerMovementStats : ScriptableObject
{
    [Header("=== 기본 이동 ===")]
    [Tooltip("기본 최고 이동 속도 - 플레이어가 평소 움직일 때의 최고 속도")]
    public float moveSpeed = 15f;

    [Tooltip("최고 속도에 도달하는 거리 (마우스 커서와의 거리 기준)")]
    public float maxSpeedDistance = 5f;

    [Tooltip("최소 이동 속도 (가까운 거리에서의 속도)")]
    public float minSpeed = 1f;

    [Header("=== 데드라인 (정지 구역) ===")]
    [Tooltip("데드라인 기본 너비 - 플레이어가 정지하는 마우스와의 최소 거리")]
    public float deadlineRadius = 0.5f;

    [Header("=== 회전 ===")]
    [Tooltip("선회력 - 플레이어가 마우스 방향으로 회전하는 속도 (높을수록 빠름)")]
    public float turnSpeed = 10f;
}
