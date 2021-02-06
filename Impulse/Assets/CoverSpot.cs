using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverSpot : MonoBehaviour
{
    private Color spotColor;

    public bool inCover;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other) {
        inCover = true;
    }

    private void OnTriggerExit(Collider other) {
        inCover = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = (inCover)?Color.red: Color.green;
        Gizmos.DrawWireSphere(transform.position, .25f);
    }
}
