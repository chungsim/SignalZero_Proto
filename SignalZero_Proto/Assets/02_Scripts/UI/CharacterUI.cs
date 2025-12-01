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

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        baseDistance = Vector3.Distance(camera.transform.position, player.transform.position);
        baseOffset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       CalculateRotate();
    }

    void CalculateRotate()
    {
		transform.rotation = Quaternion.LookRotation(player.position - camera.transform.position);
		Vector3 euler = transform.rotation.eulerAngles;
		euler.z = 0f;
		transform.rotation = Quaternion.Euler(euler);
	}
}
