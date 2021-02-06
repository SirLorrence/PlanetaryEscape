using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CoverBlock : MonoBehaviour
{
	public Transform player;
	private Vector3 pos;
	public double DistBo;

	private void Update() {

		var ToOb = Vector3.Normalize(transform.position - player.position);
		pos = (new Vector3(ToOb.x, 0, ToOb.z) * (float) DistBo) + transform.position;
	}

	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.black;
		Gizmos.DrawSphere(pos, 0.25f);
		
		Gizmos.color = Color.green;
		Gizmos.DrawLine(pos, player.position);
	}
}