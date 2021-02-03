using Enemy.Enemy_Types;
using Enemy.States;
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
	}
	private void Update() {
		currentState.DoActions();

		var vZ = Vector3.Dot(navAgent.velocity.normalized, transform.forward);
		var vX = Vector3.Dot(navAgent.velocity.normalized, transform.right);
		animator.SetFloat("xInput", vX, 0.1f, Time.deltaTime);
		animator.SetFloat("zInput", vZ, 0.1f, Time.deltaTime);

		if (health <= 0)
			SetState(new DeathState(this));
	}
}