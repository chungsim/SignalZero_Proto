using UnityEngine;

/// <summary>
/// 플레이어 캐릭터 관리 매니저
/// - GameManager가 관리
/// - 몬스터 등 다른 시스템에서 플레이어 Transform 참조 제공
/// </summary>
public class CharacterManager : MonoBehaviour
{
    // ========== 싱글톤 제거됨 ==========
    // GameManager를 통해 접근하도록 변경

    // ========== 플레이어 참조 ==========
    [Header("플레이어 프리팹")]
    [Tooltip("씬에 생성할 플레이어 프리팹")]
    [SerializeField] private GameObject playerPrefab;

    [Header("플레이어 인스턴스")]
    [Tooltip("현재 씬의 플레이어 오브젝트")]
    [SerializeField] private GameObject playerInstance;

    public Transform playerTransform;
    public PlayerController playerController;

    // ========== 초기화 ==========
    // Awake는 더 이상 싱글톤 설정 안 함
    private void Awake()
    {
        // 싱글톤 코드 제거됨
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

    // ========== 플레이어 생성/등록 ==========

    /// <summary>
    /// 프리팹으로부터 플레이어를 생성
    /// </summary>
    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("[CharacterManager] 플레이어 프리팹이 설정되지 않았습니다!");
            return;
        }

        if (playerInstance != null)
        {
            Debug.LogWarning("[CharacterManager] 플레이어가 이미 존재합니다. 기존 플레이어를 삭제합니다.");
            Destroy(playerInstance);
        }

        playerInstance = Instantiate(playerPrefab, position, rotation);
        playerTransform = playerInstance.transform;
        playerController = playerInstance.GetComponent<PlayerController>();

        Debug.Log($"[CharacterManager] 플레이어 생성 완료: {playerInstance.name} at {position}");
    }

    /// <summary>
    /// 씬에 이미 존재하는 플레이어를 등록 (수동 배치된 경우)
    /// </summary>
    public void RegisterPlayer(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("[CharacterManager] 등록할 플레이어가 null입니다!");
            return;
        }

        playerInstance = player;
        playerTransform = player.transform;
        playerController = player.GetComponent<PlayerController>();

        Debug.Log($"[CharacterManager] 플레이어 등록 완료: {player.name}");
    }

    /// <summary>
    /// 씬에서 플레이어를 자동으로 찾아서 등록
    /// </summary>
    public void FindAndRegisterPlayer()
    {
        if (playerInstance != null)
        {
            Debug.Log("[CharacterManager] 플레이어가 이미 등록되어 있습니다.");
            return;
        }

        // PlayerController가 있는 오브젝트를 찾음
        PlayerController foundPlayer = FindObjectOfType<PlayerController>();

        if (foundPlayer != null)
        {
            RegisterPlayer(foundPlayer.gameObject);
        }
        else
        {
            Debug.LogWarning("[CharacterManager] 씬에서 플레이어를 찾을 수 없습니다!");
        }
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