using UnityEngine;

//싱글톤으로 전역 접근 가능
// 플레이어 Transform 참조
public class CharacterManager : MonoBehaviour
{
    // ========== 싱글톤 ==========
    private static CharacterManager instance;
    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject("CharacterManager");
                    instance = go.AddComponent<CharacterManager>();
                }
            }
            return instance;
        }
    }

    // ========== 플레이어 참조 ==========
    [Header("플레이어 프리팹")]
    [Tooltip("씬에 생성할 플레이어 프리팹")]
    [SerializeField] private GameObject playerPrefab;

    [Header("플레이어 인스턴스")]
    [Tooltip("현재 씬의 플레이어 오브젝트")]
    [SerializeField] private GameObject playerInstance;

    private Transform playerTransform;
    private PlayerController playerController;

    // ========== 초기화 ==========
    private void Awake()
    {
        // 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // ========== 플레이어 Transform 제공 ==========

    // 플레이어의 Transform을 반환 (몬스터 추격 등에 사용)
    public Transform GetPlayerTransform()
    {
        if (playerTransform == null && playerInstance != null)
        {
            playerTransform = playerInstance.transform;
        }
        return playerTransform;
    }

    // 플레이어의 현재 위치를 반환
    public Vector3 GetPlayerPosition()
    {
        Transform pt = GetPlayerTransform();
        return pt != null ? pt.position : Vector3.zero;
    }

    // 플레이어 GameObject 반환
    public GameObject GetPlayerObject()
    {
        return playerInstance;
    }

    // PlayerController 컴포넌트 반환
    public PlayerController GetPlayerController()
    {
        if (playerController == null && playerInstance != null)
        {
            playerController = playerInstance.GetComponent<PlayerController>();
        }
        return playerController;
    }

    // ========== 플레이어 생성/등록 ==========

    // 프리팹으로부터 플레이어를 생성
    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        if (playerPrefab == null)
        {
            Debug.LogError("[CharacterManager] 플레이어 프리팹이 설정되지 않았습니다.");
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

    // 씬에 이미 존재하는 플레이어를 등록
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

    // 씬에서 플레이어를 자동으로 찾아서 등록
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

    // 플레이어가 존재하는지 확인
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