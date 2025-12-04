using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Transform player;
	public RectTransform canvasRect;
	public RectTransform uiRect;
	[SerializeField] private Image healthbarBlue;
    [SerializeField] private Image healthbarYellow;

    private float currentHealth;
    private float maxHealth;

    public float currentGauge;
    public float maxGauge;

	public float offsetY = 1.5f;


	public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Init()
    {
        player = GameManager.Instance.characterManager.GetPlayerTransform();
        cam = Camera.main;
        maxHealth = GameManager.Instance.characterManager.playerController.GetMaxHp();
        maxGauge = GameManager.Instance.characterManager.playerController.GetMaxGauge();
    }

    // Update is called once per frame
    void Update()
    {
		HealthUpdate();
		BoostUpdate();
	}

	private void LateUpdate()
	{
		CalculateRotation();
		
	}

	void CalculateRotation()
    {
		if (cam == null) return;

		// 카메라가 보고 있는 방향(forward)과 위쪽(up)을 그대로 따라가는 빌보드
		Vector3 forward = cam.transform.rotation * Vector3.forward;
		Vector3 up = cam.transform.rotation * Vector3.up;

		// 이 둘을 기준으로 카메라를 정면으로 바라보게 회전
		transform.LookAt(transform.position + forward, up);
	}

    void HealthUpdate()
    {
		if (GameManager.Instance == null)
		{
			Debug.LogError("[CharacterUI] GameManager.Instance == null");
			return;
		}

		if (GameManager.Instance.characterManager == null)
		{
			Debug.LogError("[CharacterUI] characterManager == null");
			return;
		}

		if (GameManager.Instance.characterManager.playerController == null)
		{
			Debug.LogError("[CharacterUI] playerController == null");
			return;
		}
		currentHealth = GameManager.Instance.characterManager.playerController.GetCurrentHp();
        healthbarBlue.fillAmount = currentHealth/maxHealth;
    }

    void BoostUpdate()
    {
        currentGauge = GameManager.Instance.characterManager.playerController.GetCurrentGauge();
		healthbarYellow.fillAmount =currentGauge/maxGauge;
	}
}
