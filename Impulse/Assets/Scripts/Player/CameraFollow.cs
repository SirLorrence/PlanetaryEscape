using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5.0f;

    public float sensitivityX = 10f;
    public float sensitivityY = 10f;

    //Vertical Lock Degree
    private float minX = -60f;
    private float maxX = 60f;

    private float rotationY;
    private float rotationX;

    private Vector3 positionOffset;


    void Start()
    {
        positionOffset = gameObject.transform.position - target.position;
    }

    public void UpdateCamera()
    {
        rotationX += Input.GetAxisRaw("Mouse Y") * sensitivityX;
        rotationY += Input.GetAxisRaw("Mouse X") * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minX, maxX);

        target.localEulerAngles = new Vector3(0, rotationY, 0);
        gameObject.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);
    }

    void LateUpdate()
    {
        gameObject.transform.position = target.position + positionOffset;
    }
}
