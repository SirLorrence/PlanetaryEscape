using Enemy.States;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	[RequireComponent(typeof(NavMeshAgent))]
	public class ZombieCommon : AIEntity
	{
		public bool testDebug;
		private void OnEnable() {
			SetState(new IdleState(this));
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			navAgent = GetComponent<NavMeshAgent>();
			animator = GetComponent<Animator>();
		}
		public override void Update() {
			
			base.Update();
		}
	}
}