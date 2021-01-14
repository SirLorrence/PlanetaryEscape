using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public void TakeDamage(float damageAmount)
    {
        GameObject go = ObjectPooler.Instance.GetGameObject(0);
        go.transform.position = gameObject.transform.position + new Vector3(0,2,0);
        DamagePopup damagePopup = go.GetComponent<DamagePopup>();
        damagePopup.gameObject.SetActive(true);
        damagePopup.Setup((int)damageAmount);
    }   
}
