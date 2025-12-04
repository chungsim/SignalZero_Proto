using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameManager gameManagerPrefab;

    private void Awake()
    {
        // GameManager가 없다면 생성
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerPrefab);
        }
    }
}

