using UnityEngine;

/// <summary>
/// 플레이어 캐릭터 관리 매니저
/// - GameManager가 관리
/// - 몬스터 등 다른 시스템에서 플레이어 Transform 참조 제공
/// </summary>
public class CharacterManager : MonoBehaviour
{
    // ========== 플레이어 참조 ==========
    [Header("플레이어 프리팹")]
    [Tooltip("씬에 생성할 플레이어 프리팹")]
    [SerializeField] private GameObject playerPrefab;

    [Header("플레이어 스폰 위치")]
    [Tooltip("플레이어 프리팹 생성 시 초기 위치")]
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;

    [Header("플레이어 인스턴스")]
    [Tooltip("현재 씬의 플레이어 오브젝트")]
    [SerializeField] private GameObject playerInstance;

    public Transform playerTransform;
    public PlayerController playerController;

    // ========== 초기화 ==========
    /// <summary>
    /// GameManager에서 호출하는 초기화 메서드
    /// 플레이어 생성 및 등록을 처리
    /// </summary>
    public void Init()
    {
        // 이미 플레이어가 있으면 스킵
        if (playerInstance != null)
        {
            Debug.Log("[CharacterManager] 플레이어가 이미 등록되어 있습니다.");
            return;
        }

        // 1. 먼저 씬에 있는 플레이어 찾기 (수동 배치된 경우)
        PlayerController foundPlayer = FindObjectOfType<PlayerController>();
        if (foundPlayer != null)
        {
            playerInstance = foundPlayer.gameObject;
            playerTransform = playerInstance.transform;
            playerController = playerInstance.GetComponent<PlayerController>();
            Debug.Log($"[CharacterManager] 씬에서 플레이어를 찾아 등록: {playerInstance.name}");
            return;
        }

        // 2. 씬에 없으면 프리팹으로 자동 생성
        if (playerPrefab != null)
        {
            playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            playerTransform = playerInstance.transform;
            playerController = playerInstance.GetComponent<PlayerController>();
            Debug.Log($"[CharacterManager] 프리팹으로 플레이어 생성 완료: {playerInstance.name} at {spawnPosition}");
        }
        else
        {
            Debug.LogError("[CharacterManager] 플레이어 프리팹이 설정되지 않았고 씬에도 플레이어가 없습니다!");
        }
    }

    // ========== Public API - 플레이어 Transform 제공 ==========

    /// <summary>
    /// 플레이어의 Transform을 반환 (몬스터 추격 등에 사용)
    /// </summary>
    public Transform GetPlayerTransform()
    {
        if (playerTransform == null && playerInstance != null)
        {
            playerTransform = playerInstance.transform;
        }
        return playerTransform;
    }

    /// <summary>
    /// 플레이어의 현재 위치를 반환
    /// </summary>
    public Vector3 GetPlayerPosition()
    {
        Transform pt = GetPlayerTransform();
        return pt != null ? pt.position : Vector3.zero;
    }

    /// <summary>
    /// 플레이어 GameObject 반환
    /// </summary>
    public GameObject GetPlayerObject()
    {
        return playerInstance;
    }

    /// <summary>
    /// PlayerController 컴포넌트 반환
    /// </summary>
    public PlayerController GetPlayerController()
    {
        if (playerController == null && playerInstance != null)
        {
            playerController = playerInstance.GetComponent<PlayerController>();
        }
        return playerController;
    }

    // ========== 플레이어 존재 여부 확인 ==========

    /// <summary>
    /// 플레이어가 존재하는지 확인
    /// </summary>
    public bool HasPlayer()
    {
        return playerInstance != null;
    }

    // ========== 디버그 ==========
    private void OnGUI()
    {
        // 에디터에서 디버그용 정보 표시
        if (Application.isEditor)
        {
            GUI.Label(new Rect(10, 10, 300, 20), $"플레이어 등록: {(playerInstance != null ? playerInstance.name : "없음")}");
        }
    }
}