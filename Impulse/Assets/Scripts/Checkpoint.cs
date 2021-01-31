using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform checkpointPosition;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject go = other.gameObject;
            go.GetComponent<Player>().currentRespawnPointPosition = checkpointPosition.position;
            go.GetComponent<Player>().currentRespawnPointRotation = checkpointPosition.rotation;
        }
    }
}