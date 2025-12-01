using UnityEngine;

[System.Serializable]
public class CameraSettings
{
    [Header("카메라 위치")]
    public Vector3 offset = new Vector3(0, 12, -14);

    [Header("FOV 설정")]
    public float normalFOV = 80f;
    public float actionFOV = 60f;
    public float fovChangeSpeed = 2f;      // FOV 감소 속도
    public float fovRestoreSpeed = 12f;    // FOV 복구 속도

    [Header("추적 설정")]
    public float normalSmoothSpeed = 0.14f;
    public float actionSmoothSpeed = 0.24f;

    [Header("최대 지연 거리")]
    public float normalMaxLag = 5f;
    public float actionMaxLag = 8f;

    [Header("방향 오프셋")]
    public float directionOffsetAmount = 2f;  // 이동 방향으로 치우칠 거리
    public float offsetSmoothSpeed = 3f;
}
