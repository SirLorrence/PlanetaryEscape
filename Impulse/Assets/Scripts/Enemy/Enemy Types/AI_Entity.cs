using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Enemy_Types
{
	public class AIEntity : MonoBehaviour
	{
		/// <summary>
		/// this class works like an black board
		/// hold all the ai data
		/// </summary>
		public int health;

		public int shield;
		public float detectionRadius;
		private int playerLayer = 1 << 9;
		[Range(0, 360)] public float fieldOfView;

		public Collider[] targets;

		protected NpcState currentState;
		public Weapons weapon;


		public NavMeshAgent navAgent;

		public Transform targetPlayer;

		// public Transform Hand;
		public float combatDonut;
		public float exclusionZone;

		public void SetState(NpcState aState) {
			currentState = aState;
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

		public bool testAction;
		protected Rigidbody[] rigidbodies;
		public Animator animator;


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


		public virtual void OnDrawGizmosSelected() {
			Gizmos.color = Color.green;
			//radius  
			Gizmos.DrawWireSphere(transform.position, detectionRadius);

			// // Gizmos.DrawLine(transform.position, transform.position + Vector3.right);
			// var origin = new Vector3(transform.position.x, transform.position.y + 1.4f, transform.position.z);
			// var dir = transform.TransformDirection(Vector3.forward) * 5;
			// Debug.DrawRay(origin, dir, Color.magenta);

			var viewAngleA = DirFromAngle(-fieldOfView / 2);
			var viewAngleB = DirFromAngle(fieldOfView  / 2);

			Gizmos.DrawLine(transform.position, transform.position + viewAngleA * detectionRadius);
			Gizmos.DrawLine(transform.position, transform.position + viewAngleB * detectionRadius);

			if (targetPlayer != null) {
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(targetPlayer.position, combatDonut);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(targetPlayer.position, exclusionZone);
			}
		}

		public void ToggleRagdoll() {
			if (rigidbodies != null || rigidbodies.Length != 0) {
				foreach (var rb in rigidbodies) {
					rb.isKinematic = animator.enabled;
				}
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