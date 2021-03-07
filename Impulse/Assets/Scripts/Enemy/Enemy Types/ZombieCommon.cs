using System;
using Enemy.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(Rigidbody))]
	public class ZombieCommon : AIEntity
	{
		private void Start() {
			SetInitState(new FollowState(this));
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			navAgent = GetComponentInChildren<NavMeshAgent>();
		}

		private void OnEnable() {
			navAgent.speed = wonderSpeed * speedMultiplier;
		}

		public override void Update() {
			currentState.DoActions();
			base.Update();
		}
	}
}