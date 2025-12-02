using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Transform player;
    [SerializeField] private Image healthbarBlue;
    [SerializeField] private Image healthbarYellow;

    private float baseDistance;
    private Vector3 baseOffset;
    private Vector3 offset;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        baseDistance = Vector3.Distance(cam.transform.position, player.transform.position);
        baseOffset = transform.position - player.transform.position;
        offset = cam.transform.position - player.transform.position; 
        
    }

    // Update is called once per frame
    void Update()
    {
       CalculateRotation();
       CalculatePosition();

    }

    void CalculateRotation()
    {
		transform.rotation = Quaternion.LookRotation(player.position - cam.transform.position);
		Vector3 euler = transform.rotation.eulerAngles;
		euler.z = 0f;
		transform.rotation = Quaternion.Euler(euler);
	}
    void CalculatePosition()
    {
        cam.transform.position = player.transform.position - cam.transform.position;
    }
}
