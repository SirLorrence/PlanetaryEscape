using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 currentRespawnPointPosition;
    public Quaternion currentRespawnPointRotation;
    // Start is called before the first frame update
    void Start()
    {
        currentRespawnPointPosition = gameObject.transform.position;
        currentRespawnPointRotation = gameObject.transform.rotation;
    }

    public void ReturnToCheckpoint()
    {
        gameObject.transform.position = currentRespawnPointPosition;
        gameObject.transform.rotation = currentRespawnPointRotation;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
