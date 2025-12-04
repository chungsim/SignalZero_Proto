using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private Transform target;

    [Header("카메라 오프셋")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);

    [Header("일반 상태")]
    [SerializeField] private float normalMaxLagDistance = 5f;
    [SerializeField] private float normalFov = 60f;

    [Header("액션 상태 (버스트/부스터)")]
    [SerializeField] private float actionMaxLagDistance = 8f;
    [SerializeField] private float actionFov = 50f;

    [Header("전환 속도")]
    //[SerializeField] private float paramLerpSpeed = 10f;
    [SerializeField] private float fovLerpSpeedIn = 15f;   // 줌 인 (FOV 감소)
    [SerializeField] private float fovLerpSpeedOut = 8f;   // 줌 아웃 (FOV 증가)

    [Header("방향 오프셋")]
    [SerializeField] private float directionOffsetAmount = 2f;
    [SerializeField] private float directionOffsetSpeed = 3f;

    private Vector3 currentDirectionOffset = Vector3.zero;

    // 현재 사용 중인 값
    private float currentMaxLagDistance;
    private float currentFov;

    // 목표값
    private float targetMaxLagDistance;
    private float targetFov;

    private Camera cam;
    private PlayerController playerController;

    private void Awake()
    {
        // 초기값 설정
        currentMaxLagDistance = normalMaxLagDistance;
        targetMaxLagDistance = normalMaxLagDistance;

        cam = GetComponent<Camera>();
        if (cam == null && Camera.main != null)
            cam = Camera.main;

        if (cam != null)
        {
            currentFov = targetFov = normalFov;
            cam.fieldOfView = currentFov;
        }

        if (target != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }
    }

    private void Start()
    {
        // 플레이어가 프리팹으로 생성되어도 자동으로 잡는다.
        if (target == null)
        {
            // GameManager를 통해 CharacterManager에 접근
            if (GameManager.Instance != null && GameManager.Instance.characterManager != null)
            {
                target = GameManager.Instance.characterManager.GetPlayerTransform();
                if (target != null)
                {
                    playerController = target.GetComponent<PlayerController>();
                    Debug.Log("[CameraFollow] 플레이어 자동 감지 완료");
                }
                else
                {
                    Debug.LogWarning("[CameraFollow] 플레이어를 찾을 수 없습니다");
                }
            }
            else
            {
                Debug.LogWarning("[CameraFollow] GameManager 또는 CharacterManager를 찾을 수 없습니다");
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 액션 상태에 따라 목표값 업데이트
        UpdateCameraMode();

        // 3. FOV 업데이트
        UpdateFOV();

        // 4. 위치 업데이트
        UpdatePosition();
    }

    private void UpdateCameraMode()
    {
        bool isAction = playerController != null && playerController.IsActionState();

        if (isAction)
        {
            targetMaxLagDistance = actionMaxLagDistance;
            targetFov = actionFov;
        }
        else
        {
            targetMaxLagDistance = normalMaxLagDistance;
            targetFov = normalFov;
        }
    }

    private void UpdateFOV()
    {
        if (cam == null) return;

        // 줌 인/아웃 방향에 따라 다른 속도 사용
        float fovSpeed = (targetFov < currentFov) ? fovLerpSpeedIn : fovLerpSpeedOut;

        currentFov = Mathf.Lerp(
            currentFov,
            targetFov,
            fovSpeed * Time.deltaTime
        );

        cam.fieldOfView = currentFov;
    }

    private void UpdatePosition()
    {
        currentMaxLagDistance = targetMaxLagDistance;

        // 이동 방향 오프셋 계산
        Vector3 moveDirection = Vector3.zero;
        if (playerController != null && playerController.IsMoving())
        {
            moveDirection = playerController.GetMoveDirection();
        }

        // 카메라 위치 보정 효과
        Vector3 targetDirectionOffset = new Vector3(
            moveDirection.x * directionOffsetAmount,
            0,
            moveDirection.z * directionOffsetAmount
        );

        // 카메라 딜레이 선형 보간
        currentDirectionOffset = Vector3.Lerp(
            currentDirectionOffset,
            targetDirectionOffset,
            directionOffsetSpeed * Time.deltaTime
        );

        // 목표 위치 계산 (오프셋 + 방향 오프셋)
        Vector3 idealPos = target.position + offset + currentDirectionOffset;

        // 최대 지연 거리 제한
        Vector3 diff = idealPos - (target.position + offset);   // == currentDirectionOffset;
        float dist = diff.magnitude;

        if (dist > currentMaxLagDistance)
        {
            idealPos = (target.position + offset) + diff.normalized * currentMaxLagDistance;
        }

        transform.position = idealPos;

        // (탑뷰/쿼터뷰 유지)
        // transform.LookAt()를 사용하지 않는 방식으로 수정
    }
}
