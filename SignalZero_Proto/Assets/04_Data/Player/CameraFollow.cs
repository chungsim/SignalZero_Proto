using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("타겟")]
    [SerializeField] private Transform target;

    [Header("카메라 오프셋")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);

    [Header("일반 상태")]
    [SerializeField] private float normalSmoothTime = 0.2f;
    [SerializeField] private float normalMaxLagDistance = 5f;
    [SerializeField] private float normalFov = 60f;

    [Header("액션 상태 (버스트/부스터)")]
    [SerializeField] private float actionSmoothTime = 0.35f;
    [SerializeField] private float actionMaxLagDistance = 8f;
    [SerializeField] private float actionFov = 50f;

    [Header("전환 속도")]
    [SerializeField] private float paramLerpSpeed = 10f;
    [SerializeField] private float fovLerpSpeedIn = 15f;   // 줌 인 (FOV 감소)
    [SerializeField] private float fovLerpSpeedOut = 8f;   // 줌 아웃 (FOV 증가)

    [Header("방향 오프셋")]
    [SerializeField] private float directionOffsetAmount = 2f;
    [SerializeField] private float directionOffsetSpeed = 3f;

    private Vector3 velocity;
    private Vector3 currentDirectionOffset;

    // 현재 사용 중인 값
    private float currentSmoothTime;
    private float currentMaxLagDistance;
    private float currentFov;

    // 목표값
    private float targetSmoothTime;
    private float targetMaxLagDistance;
    private float targetFov;

    private Camera cam;
    private PlayerController playerController;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null && Camera.main != null)
            cam = Camera.main;

        if (target != null)
        {
            playerController = target.GetComponent<PlayerController>();
        }

        // 초기값 설정
        currentSmoothTime = normalSmoothTime;
        currentMaxLagDistance = normalMaxLagDistance;
        targetSmoothTime = normalSmoothTime;
        targetMaxLagDistance = normalMaxLagDistance;

        if (cam != null)
        {
            currentFov = targetFov = normalFov;
            cam.fieldOfView = currentFov;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // 1. 액션 상태에 따라 목표값 업데이트
        UpdateCameraMode();

        // 2. 파라미터 보간
        InterpolateParameters();

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
            targetSmoothTime = actionSmoothTime;
            targetMaxLagDistance = actionMaxLagDistance;
            targetFov = actionFov;
        }
        else
        {
            targetSmoothTime = normalSmoothTime;
            targetMaxLagDistance = normalMaxLagDistance;
            targetFov = normalFov;
        }
    }

    private void InterpolateParameters()
    {
        currentSmoothTime = Mathf.Lerp(
            currentSmoothTime,
            targetSmoothTime,
            paramLerpSpeed * Time.deltaTime
        );

        currentMaxLagDistance = Mathf.Lerp(
            currentMaxLagDistance,
            targetMaxLagDistance,
            paramLerpSpeed * Time.deltaTime
        );
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
        // 이동 방향 오프셋 계산
        Vector3 moveDirection = Vector3.zero;
        if (playerController != null && playerController.IsMoving())
        {
            moveDirection = playerController.GetMoveDirection();
        }

        Vector3 targetDirectionOffset = new Vector3(
            moveDirection.x * directionOffsetAmount,
            0,
            moveDirection.z * directionOffsetAmount
        );

        currentDirectionOffset = Vector3.Lerp(
            currentDirectionOffset,
            targetDirectionOffset,
            directionOffsetSpeed * Time.deltaTime
        );

        // 목표 위치 계산 (오프셋 + 방향 오프셋)
        Vector3 idealPos = target.position + offset + currentDirectionOffset;

        // SmoothDamp로 부드럽게 이동
        Vector3 newPos = Vector3.SmoothDamp(
            transform.position,
            idealPos,
            ref velocity,
            currentSmoothTime
        );

        // 최대 지연 거리 제한
        Vector3 diff = newPos - (target.position + offset);
        float dist = diff.magnitude;

        if (dist > currentMaxLagDistance)
        {
            newPos = (target.position + offset) + diff.normalized * currentMaxLagDistance;
        }

        transform.position = newPos;

        // (탑뷰/쿼터뷰 유지)
        // transform.LookAt()를 사용하지 않는 방식으로 수정
    }
}
