using System;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	public class ZombieHeavy : AIEntity
	{
		private void OnEnable() {
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			navAgent = GetComponent<NavMeshAgent>();
		}

		public override void Update() {
			base.Update();
		}
	}
}