using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 positionOffset;
    public float smooth = 5.0f;


    public float sensitivityX = 10f;

    public float sensitivityY = 10f;

    //how high and low your head can move
    private float minX = -60f;
    private float maxX = 60f;

    private float rotationY;
    private float rotationX;


    void Start()
    {
        
        positionOffset = gameObject.transform.position - target.position;
    }

    public void CameraMove(float mouseX, float mouseY)
    {
        rotationX += mouseY * sensitivityX;
        rotationY += mouseX * sensitivityY;
        rotationX = Mathf.Clamp(rotationX, minX, maxX);

        target.localEulerAngles = new Vector3(0, rotationY, 0);
        gameObject.transform.localEulerAngles = new Vector3(-rotationX, rotationY, 0);
    }

    void LateUpdate()
    {
        //----------------- old
        //transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, target.position.z + positionOffset.z),  Time.deltaTime * smooth);
        //----------------

        //new (fps)
        gameObject.transform.position = target.position + positionOffset;
    }
}