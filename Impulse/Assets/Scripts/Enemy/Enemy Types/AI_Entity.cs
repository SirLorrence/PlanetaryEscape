using System;
using Mirror;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	public class AIEntity : GameEntity
	{
		/// <summary>
		/// this class works like an black board
		/// hold all the ai data
		/// </summary>
		[Range(0, 360)] public float fieldOfView;

		[HideInInspector] public NavMeshAgent navAgent;
		[HideInInspector] public Collider[] targets;
		public Transform targetPlayer;
		public float detectionRadius;
		public float combatDonut;
		public bool testAction;
		public Animator animator;
		public bool toggleBody;

		public bool useRandom;
		[Range(0,100)]public int aggressionLevel;
		
		protected Rigidbody[] rigidbodies;
		protected NpcState currentState;

		private int playerLayer = 1 << 9;

		public void SetState(NpcState aState) {
			currentState = aState;
		}

		public virtual void Update() {
			
			var vZ = Vector3.Dot(navAgent.velocity.normalized, transform.forward);
			var vX = Vector3.Dot(navAgent.velocity.normalized, transform.right);
		
			animator.SetFloat("xInput", vX, 0.1f, Time.deltaTime);
			animator.SetFloat("zInput", vZ, 0.1f, Time.deltaTime);
		}


		public bool InView(Collider[] targets) {
			foreach (var t in targets) {
				Vector3 dist = t.transform.position - transform.position;
				if (Vector3.Angle(transform.forward, dist) < fieldOfView / 2)
					return true;
			}
			return false;
		}

		public Transform ClosestTarget(Collider[] targets) {
			double dist = detectionRadius;
			Transform nearestTarget = null;
			foreach (var target in targets) {
				double d = target.transform.position.x - transform.position.x;
				if (d < dist) {
					dist = d;
					nearestTarget = target.transform;
				}
			}

			return nearestTarget;
		}

		public Collider[] PlayersInRange() => Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);


		public Vector3 DirFromAngle(float angleInDegrees) {
			angleInDegrees += transform.eulerAngles.y;
			//swapping around the sin and cos 
			return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
		}

		public void TakeDamage(float damageAmount) {
			GameObject go = ObjectPooler.Instance.GetGameObject(0);
			go.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
			DamagePopup damagePopup = go.GetComponent<DamagePopup>();
			damagePopup.gameObject.SetActive(true);
			damagePopup.Setup((int) damageAmount);
			health -= 1;
		}

		public void ToggleRagdoll() {
			if (rigidbodies != null || rigidbodies.Length != 0) {
				foreach (var rb in rigidbodies) {
					rb.isKinematic = animator.enabled;
				}
			}
		}

		[HideInInspector] public Vector3 coverPos;

		public virtual void OnDrawGizmosSelected() {
			Gizmos.color = Color.green;
			//radius  
			Gizmos.DrawWireSphere(transform.position, detectionRadius);

			var viewAngleA = DirFromAngle(-fieldOfView / 2);
			var viewAngleB = DirFromAngle(fieldOfView  / 2);

			Gizmos.DrawLine(transform.position, transform.position + viewAngleA * detectionRadius);
			Gizmos.DrawLine(transform.position, transform.position + viewAngleB * detectionRadius);

			if (targetPlayer != null) {
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(targetPlayer.position, combatDonut);
				Gizmos.color = Color.red;
			}

			if (coverPos != Vector3.zero) {
				Gizmos.color = Color.black;
				Gizmos.DrawSphere(coverPos, .25f);
			}
		}
	}

	public abstract class NpcState
	{
		protected readonly AIEntity entity;
		public NpcState(AIEntity aiEntity) => entity = aiEntity; // creates the states own constructor
		public abstract void DoActions();
	}
}