using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CompletionManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject enemyHolder;
    public GameObject doorCollider;
    public Transform door;

    private void OnEnable()
    {
        //-------Auto fill array
        enemies = new GameObject[enemyHolder.transform.childCount];
        for (int i = 0; i < enemyHolder.transform.childCount; i++)
        {
            enemies[i] = enemyHolder.transform.GetChild(i).gameObject;
        }

        //GameManager.Instance.enemiesRemaining = enemies.Length;
    }


    void FixedUpdate()
    {
        if (IsCompletedCheck()) AdvanceLevel();
    }

    bool IsCompletedCheck()
    {
        foreach (var go in enemies)
            if (go.activeSelf)
                return false;
        return true;
    }

    void AdvanceLevel()
    {
        //Put Advanced Level Code Here
        Debug.Log("AdvancedLevel");
        doorCollider.SetActive(true);
        StartCoroutine(DoorAction());
        // gameObject.SetActive(false);
    }

    private IEnumerator DoorAction()
    {
        StartCoroutine("OpenDoor");
        yield return new WaitForSeconds(.25f);
        StopCoroutine("OpenDoor");
        gameObject.SetActive(false);

    }

    IEnumerator OpenDoor()
    {
        var newPos = new Vector3(door.position.x - 4, door.position.y, door.position.z);
        while (true)
        {
            door.position = Vector3.Lerp(door.position, newPos, Time.deltaTime * .75f);
            yield return null;
        }
    }
}