using UnityEngine;

/// <summary>
/// 플레이어 일반 이동 시스템
/// - 마우스 추적 이동
/// - 속도 계산
/// - 회전 처리
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("스탯")]
    public PlayerMovementStats movementStats;

    // 컴포넌트
    private Rigidbody rb;
    private Camera mainCamera;

    // 마우스 & 이동
    private Vector2 mouseScreenPosition;
    private Vector3 mouseWorldPosition;
    private Vector3 moveDirection;
    private float currentSpeed;

    // 외부 제어용
    private bool isExternalControl = false;  // Dash 시스템이 제어 중인지

    // ===== 초기화 =====
    public void Initialize(Rigidbody rigidbody, Camera camera)
    {
        rb = rigidbody;
        mainCamera = camera;
    }

    // ===== 입력 처리 =====
    public void SetMousePosition(Vector2 screenPos)
    {
        mouseScreenPosition = screenPos;
    }

    // ===== 업데이트 =====
    public void UpdateMovement()
    {
        UpdateMouseWorldPosition();

        // 외부 제어 중이 아닐 때만 일반 이동
        if (!isExternalControl)
        {
            UpdateNormalMovement();
        }
    }

    public void ApplyMovement()
    {
        Vector3 velocity = moveDirection * currentSpeed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
    }

    // ===== 마우스 위치 계산 =====
    void UpdateMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position.y);

        if (groundPlane.Raycast(ray, out float distance))
        {
            mouseWorldPosition = ray.GetPoint(distance);
        }
    }

    // ===== 일반 이동 =====
    void UpdateNormalMovement()
    {
        Vector3 toMouse = mouseWorldPosition - transform.position;
        toMouse.y = 0;
        float distanceToMouse = toMouse.magnitude;

        // 데드라인 체크
        if (distanceToMouse <= movementStats.deadlineRadius)
        {
            moveDirection = Vector3.zero;
            currentSpeed = 0f;
            return;
        }

        // 방향 설정
        moveDirection = toMouse.normalized;

        // 속도 계산 (선형 보간)
        float speedProgress = Mathf.Clamp01(
            (distanceToMouse - movementStats.deadlineRadius) / movementStats.maxSpeedDistance
        );
        currentSpeed = Mathf.Lerp(movementStats.minSpeed, movementStats.moveSpeed, speedProgress);

        // 회전
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * movementStats.turnSpeed
            );
        }
    }

    // ===== 외부 제어 (Dash 시스템) =====
    /// <summary>
    /// Dash 시스템이 직접 이동을 제어할 때 호출
    /// </summary>
    public void SetExternalControl(bool active, Vector3 direction, float speed)
    {
        isExternalControl = active;
        if (active)
        {
            moveDirection = direction;
            currentSpeed = speed;
        }
    }

    /// <summary>
    /// Dash 시스템이 회전을 제어할 때 호출
    /// </summary>
    public void SetRotation(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * movementStats.turnSpeed
            );
        }
    }

    // ===== Getter =====
    public Vector3 GetMoveDirection() => moveDirection;
    public float GetCurrentSpeed() => currentSpeed;
    public bool IsMoving() => currentSpeed > 0.1f;
    public Vector3 GetMouseWorldPosition() => mouseWorldPosition;
}