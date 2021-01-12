using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public Transform resetTransform;
    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            PositionReset(c.gameObject);
        }
    }

    void PositionReset(GameObject objectToReset)
    {
        objectToReset.transform.position = resetTransform.position;
        objectToReset.transform.rotation = resetTransform.rotation;
        objectToReset.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    //void OnDrawGizmos()
    //{
        //Gizmos.DrawIcon(resetTransform.position, "Reset Position");
    //}
}
