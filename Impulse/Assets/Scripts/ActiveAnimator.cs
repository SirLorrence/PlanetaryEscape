using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  System.Linq;

public class ActiveAnimator : MonoBehaviour
{
	public Animator anim;
	private void LateUpdate() {
		foreach (Transform o in transform) {
			if (o.gameObject.activeInHierarchy) anim = o.GetComponent<Animator>();
		}
	}
}