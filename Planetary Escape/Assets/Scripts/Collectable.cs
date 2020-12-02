using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public AudioSource pickupSound;

    private MeshRenderer mr;
    private BoxCollider bc;
    public Animation anim;
    public ParticleSystem ps;

    private float originalY;
    public float floatDist = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
        mr = gameObject.GetComponent<MeshRenderer>();
        bc = gameObject.GetComponent<BoxCollider>(); ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.transform.Rotate(0, 0.3f, 0);

        transform.position = new Vector3(transform.position.x, originalY + (Mathf.Sin(Time.time) * floatDist), transform.position.z);
    }

    public void Collected()
    {
        anim.Play();
        pickupSound.Play();
        ps.Play();
        StartCoroutine(Shrink());
        StartCoroutine(CollectableRecovery());
    }

    private IEnumerator CollectableRecovery()
    {
        yield return new WaitForSeconds(3f);
        bc.enabled = true;
        mr.enabled = true;
        transform.localScale = new Vector3(1, 1, 1);
    }
    private IEnumerator Shrink()
    {
        yield return new WaitForSeconds(0.5f);
        transform.localScale = new Vector3(1, 1, 1);
        bc.enabled = false;
        mr.enabled = false;
    }
}
