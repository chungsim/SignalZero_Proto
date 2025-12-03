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


    private float updateHealt;

	public float offsetY = 1.5f;


	public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void Init()
    {
        player = GameManager.Instance.characterManager.GetPlayerTransform();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

	private void LateUpdate()
	{
		CalculateRotation();
	}

	void CalculateRotation()
    {
		transform.LookAt(cam.transform);
	}

    void HealthUI()
    {
        
    }

    void BoostUI()
    {

    }
}
