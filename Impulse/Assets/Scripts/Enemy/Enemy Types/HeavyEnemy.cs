using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	public class HeavyEnemy : AIEntity
	{
		private void OnEnable() {
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			navAgent = GetComponent<NavMeshAgent>();
			animator = GetComponent<Animator>();
			weapon = GetComponentInChildren<Weapons>();
		}
	}
}