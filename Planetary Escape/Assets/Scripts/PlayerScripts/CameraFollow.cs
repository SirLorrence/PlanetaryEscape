using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 positionOffset;
    public float smooth = 5.0f;

    void Start()
    {
        positionOffset = gameObject.transform.position - target.position;
    }

    void LateUpdate()
    {
        // transform.position = Vector3.Lerp(
        //     transform.position, new Vector3(target.position.x + positionOffset.x, target.position.y + positionOffset.y, target.position.z + positionOffset.z),
        //     Time.deltaTime * smooth);

        //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + positionOffset.x, transform.position.y, target.position.z + positionOffset.z),  Time.deltaTime * smooth);
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, target.position.z + positionOffset.z),  Time.deltaTime * smooth);
    }
}