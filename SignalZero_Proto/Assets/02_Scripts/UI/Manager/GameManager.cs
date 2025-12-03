using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager Instance {  get { if (instance == null)instance = new GameManager();return instance; } }

	[Header("갖고있는 매니저")]
	public UIManager uiManager;
	public FieldManager fieldManager;
	public CharacterManager characterManager;
	public MonsterSpawnManager monsterSpawnManager;

	private void Awake()
	{
		if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

		
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("씬 진입: " + scene.name);

		if (scene.name == "MainScene")
		{
			characterManager = FindObjectOfType<CharacterManager>();
			uiManager = FindObjectOfType<UIManager>();
			uiManager.characterUI = FindObjectOfType<CharacterUI>();
			fieldManager = FindObjectOfType<FieldManager>();
			monsterSpawnManager = FindObjectOfType<MonsterSpawnManager>();
		}
		else if (scene.name == "Ui_Test_Scene")
		{
			uiManager = FindObjectOfType<UIManager>();
			uiManager.characterUI = FindObjectOfType<CharacterUI>();
			fieldManager = FindObjectOfType<FieldManager>();
		}
		else if (scene.name == "EndingScene")
		{
			// 엔딩씬 전용 로직
			Debug.Log("엔딩씬 전용 로직 실행!");
		}
	}



}
