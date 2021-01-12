using System.Collections;
using UnityEngine;

public class StartEnemies : MonoBehaviour
{
    public GameObject frontDoor;
    public GameObject backDoor;
    public CompletionManager cm;
    private bool activated = false;
    void OnTriggerEnter(Collider c)
    {
        if (!activated)
        {
            if (c.CompareTag("Player"))
            {
                cm.StartGame();
                activated = true;
                StartCoroutine(DoorAction());
            }
        }
    }
    private IEnumerator DoorAction()
    {
        StartCoroutine(OpenDoor(frontDoor));
        yield return new WaitForSeconds(.25f);
        StopCoroutine(OpenDoor(frontDoor));
        StartCoroutine(OpenDoor(backDoor));
        yield return new WaitForSeconds(.25f);
        StopCoroutine(OpenDoor(backDoor));
    }

    IEnumerator OpenDoor(GameObject go)
    {
        var newPos = new Vector3(go.transform.position.x + 4.3f, go.transform.position.y, go.transform.position.z);
        while (true)
        {
            go.transform.position = Vector3.Lerp(go.transform.position, newPos, Time.deltaTime);
            yield return 0;
        }
    }
}
