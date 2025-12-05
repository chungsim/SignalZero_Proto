using UnityEngine;

public static class GlobalBootstrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // 이미 GameManager가 살아 있으면 생성하지 않음
        if (GameManager.Instance != null)
            return;

        // Resources에서 GameManagerPrefab을 불러와 생성
        var prefab = Resources.Load<GameObject>("GameManager");

        if (prefab == null)
        {
            Debug.LogError("[Bootstrap] GameManagerPrefab을 Resources 폴더에서 찾지 못했습니다.");
            return;
        }

        var gm = Object.Instantiate(prefab);
        Object.DontDestroyOnLoad(gm);

        Debug.Log("[Bootstrap] GameManager 자동 생성 완료");
    }
}
