using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Image healthbarBlue;
    [SerializeField] private Image healthbarYellow;


    private float playerHealt;


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
       CalculateRotation();
    }

    void CalculateRotation()
    {
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, player.transform.position);
		this.GetComponent<RectTransform>().position = screenPoint;
	}
}
