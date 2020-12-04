using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CompletionManager : MonoBehaviour
{
    public NavMeshAgent[] enemies;
    public GameObject doorCollider;
    public Transform door;

    private bool started = false;
    private bool activated = false;

    private void OnEnable()
    {
        //-------Auto fill array
        enemies = new NavMeshAgent[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemies[i] = transform.GetChild(i).gameObject.GetComponent<NavMeshAgent>();
            enemies[i].isStopped = true;
        }

        GameManager.Instance.enemiesRemaining = transform.childCount;
    }

    void FixedUpdate()
    {
        if (started && IsCompletedCheck()) AdvanceLevel();
    }

    bool IsCompletedCheck()
    {
        foreach (var go in enemies)
            if (go.gameObject.activeSelf)
                return false;
        return true;
    }

    public void StartGame()
    {
        foreach (var enemy in enemies)
            enemy.isStopped = false;

        started = true;
    }

    void AdvanceLevel()
    {
        if (!activated)
        {
            activated = true;
            //Put Advanced Level Code Here
            Debug.Log("AdvancedLevel");
            GameManager.Instance.UnLoadLastLevel();
            GameManager.Instance.LoadNextLevel();
            //StartCoroutine(DoorAction());
        }
    }

    private IEnumerator DoorAction()
    {
        StartCoroutine(OpenDoor());
        yield return new WaitForSeconds(.25f);
        StopCoroutine(OpenDoor());

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