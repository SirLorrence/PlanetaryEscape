using System.Collections;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public GameObject door;
    private bool activated = false;
    void OnTriggerEnter(Collider c)
    {
        if (!activated)
        {
            if (c.CompareTag("Player"))
            {
                activated = true;
                StartCoroutine(DoorAction());
            }
        }
    }
    private IEnumerator DoorAction()
    {
        StartCoroutine(CloseDoor());
        yield return new WaitForSeconds(.25f);
        StopCoroutine(CloseDoor());
        //gameObject.SetActive(false);

    }

    IEnumerator CloseDoor()
    {
        var newPos = new Vector3(door.transform.position.x + 7, door.transform.position.y, door.transform.position.z);
        while (true)
        {
            door.transform.position = Vector3.Lerp(door.transform.position, newPos, Time.deltaTime * .75f);
            yield return 0;
        }
    }
}
