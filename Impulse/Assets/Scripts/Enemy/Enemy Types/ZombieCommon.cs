using System;
using Enemy.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class ZombieCommon : AIEntity
	{
		private void Start() {
			SetInitState(new FollowState(this));
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			navAgent = GetComponent<NavMeshAgent>();

			// targets = PlayersInRange();
			// targetPlayer = ClosestTarget(targets);
		}
		private void OnEnable() {
			// navAgent.speed = wonderSpeed * speedMultiplier;
		}

		public override void Update() {
			currentState.DoActions();
			base.Update();
		}
	}
}