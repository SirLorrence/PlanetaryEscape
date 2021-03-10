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
		private void OnEnable() {
			SetAggroLevel();
			navAgent.speed = wonderSpeed * speedMultiplier;
		}
		public override void Update() {
			currentState.DoActions();
			base.Update();
		}
	}
}