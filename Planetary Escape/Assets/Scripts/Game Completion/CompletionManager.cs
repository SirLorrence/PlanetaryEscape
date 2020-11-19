using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionManager : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject doorCollider;

    void FixedUpdate()
    {
        if(IsCompletedCheck()) AdvanceLevel();
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
        gameObject.SetActive(false);
    }
}
