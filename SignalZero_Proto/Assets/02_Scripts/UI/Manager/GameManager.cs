using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;


public enum GameState
{
	Start,
	Playing,
	Paused,
	GameOver
}
public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance { get; private set; }

    //public event Action Init;

    [Header("전역 매니저(DontDestroyOnLoad)")]
    [SerializeField] private AudioManager audioManagerPrefab;
    public AudioManager audioManager;

    [Header("씬 매니저(MainScene 전용)")]
    public UIManager uiManager;
    public FieldManager fieldManager;
    public CharacterManager characterManager;
    public MonsterSpawnManager monsterSpawnManager;


    private void Awake()
	{
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 전역 오디오 매니저 생성
        EnsureAudioManager();

        // 씬 로드 이벤트 연결
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // DDOL 오디오 매니저가 없으면 생성
    private void EnsureAudioManager()
    {
        if (audioManager == null)
        {
            audioManager = Instantiate(audioManagerPrefab);
            DontDestroyOnLoad(audioManager.gameObject);
        }
    }

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("씬 진입: " + scene.name);

		if (scene.name == "MainScene")
		{
            // 1) SceneManager 오브젝트 단 한 번만 찾기
            GameObject sceneManagerObj = GameObject.Find("SceneManager");
            if (sceneManagerObj == null)
            {
                Debug.LogError("[GameManager] SceneManager 오브젝트를 찾을 수 없습니다!");
                return;
            }

            // 2) 모든 매니저를 SceneManager 하위에서 GetComponent로 가져오기
            characterManager = sceneManagerObj.GetComponentInChildren<CharacterManager>();
            uiManager = sceneManagerObj.GetComponentInChildren<UIManager>();
            fieldManager = sceneManagerObj.GetComponentInChildren<FieldManager>();
            monsterSpawnManager = sceneManagerObj.GetComponentInChildren<MonsterSpawnManager>();

            // ========== 2. CharacterManager 초기화 (플레이어 생성) ==========
            if (characterManager != null)
            {
                characterManager.Init();
                Debug.Log("[GameManager] CharacterManager 초기화 완료!");
                var playerTr = characterManager.GetPlayerTransform();
                uiManager.characterUI = playerTr.GetComponentInChildren<CharacterUI>(true);
            }
            else
            {
                Debug.LogError("[GameManager] CharacterManager를 찾을 수 없습니다!");
            }

            // ========== 3. CameraFollow 초기화 (플레이어 참조) ==========
            CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.Init();
                Debug.Log("[GameManager] CameraFollow 초기화 완료!");
            }
            else
            {
                Debug.LogWarning("[GameManager] CameraFollow를 찾을 수 없습니다!");
            }

            Debug.Log("[GameManager] UIManager 초기화 완료!");

            if (uiManager != null)
            {
                uiManager.Init(); ;
            }

        }
	}
	
	void GameStart()
	{

	}

	void GameEnd()
	{

	}



}
