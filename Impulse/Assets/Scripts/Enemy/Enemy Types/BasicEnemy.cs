using System;
using System.Collections;
using System.Collections.Generic;
using Enemy.Enemy_Types;
using Enemy.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BasicEnemy : AIEntity
{
	public bool toggleBody;

	private void OnEnable() {
		SetState(new BasicIdleState(this));
		rigidbodies = GetComponentsInChildren<Rigidbody>();
		navAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		weapon = GetComponentInChildren<Weapons>();
		// ToggleRagdoll(toggleBody);
	}

	private void Update() {
		currentState.DoActions();

		var vZ = Vector3.Dot(navAgent.velocity.normalized, transform.forward);
		var vX = Vector3.Dot(navAgent.velocity.normalized, transform.right);

		//
		animator.SetFloat("xInput", vX, 0.1f, Time.deltaTime);
		animator.SetFloat("zInput", vZ, 0.1f, Time.deltaTime);


		if (health <= 0)
			SetState(new DeathState(this));
	}


	// public override void OnDrawGizmosSelected() {
	// 	Gizmos.color = Color.green;
	// 	//radius  
	// 	Gizmos.DrawWireSphere(transform.position, detectionRadius);
	//
	// 	//
	// 	// var viewAngleA = DirFromAngle(-fieldOfView / 2);
	// 	// var viewAngleB = DirFromAngle(fieldOfView  / 2);
	// 	//
	// 	// Gizmos.DrawLine(transform.position, transform.position + viewAngleA * detectionRadius  );
	// 	// Gizmos.DrawLine(transform.position, transform.position + viewAngleB * detectionRadius  );
	// }
}