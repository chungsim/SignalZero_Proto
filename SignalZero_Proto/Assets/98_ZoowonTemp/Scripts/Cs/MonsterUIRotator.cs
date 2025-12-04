using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUIRotator : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private Camera camera;
    Vector3 lookDirection;

    void Start()
    {
        camera = Camera.main;
    }

    void FixedUpdate()
    {
        LookCamera();
    }

    private void LookCamera()
    {
        canvas.transform.rotation = camera.transform.rotation;
    }

}
