using System;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using Random = UnityEngine.Random;

namespace Enemy.Enemy_Types
{
	/// <summary>
	/// this class works like an black board
	/// hold all the ai data
	/// </summary>
	public class AIEntity : GameEntity
	{
		#region Fields

		[Header("Detection Values")] [Range(0, 360)]
		public float fieldOfView;

		public Transform targetPlayer;
		public float detectionRadius;
		public float viewDistance = 10;
		public double attackDist;

		[Header("Aggression")] public bool alwaysAggro;
		[Range(0, 100)] public int aggressionLevel;

		[Header("Speed Values")] public float wonderSpeed;
		public float followSpeed;
		public float chaseSpeed;
		public float speedMultiplier = 1;

		[Header("Body Debug")] public bool toggleBody;

		[Header("Visual Debug")] public bool showViewCast;
		public bool showRadius;

		public bool testStateChange;

		[HideInInspector] public Collider[] targets;
		[HideInInspector] public NavMeshAgent navAgent;
		public Animator animator;

		protected Rigidbody[] rigidbodies;

		protected NpcState currentState;
		protected NpcState pastState;

		[SerializeField] protected bool playerFound, inRange;

		public bool PlayerFound => playerFound;
		public bool InRange => inRange;

		private int playerLayer = 1 << 9;

		#endregion

		public virtual void Update() {
			var vZ = Vector3.Dot(navAgent.velocity.normalized, transform.forward);
			var vX = Vector3.Dot(navAgent.velocity.normalized, transform.right);

			animator.SetFloat("xInput", vX, 0.1f, Time.deltaTime);
			animator.SetFloat("zInput", vZ, 0.1f, Time.deltaTime);


			VisionArea();

			Debug.Log($"Player Found :{playerFound}");
			Debug.Log($"Can Attack :{inRange}");
		}

		public void SetInitState(NpcState aState) => currentState = aState;

		//for push-down automata
		public void PushState(NpcState state) {
			pastState = currentState;
			currentState = state;
		}

		public void PopState() => currentState = pastState;


		public void VisionArea() {
			for (float i = -fieldOfView; i < fieldOfView; i += 4) {
				Vector3 angle = DirFromAngle(i / 2);
				Vector3 origin = transform.position + Vector3.up;
				Vector3 dir = angle;

				Ray ray = new Ray(origin, dir);
				RaycastHit raycastHit;
				if (Physics.Raycast(ray, out raycastHit, viewDistance)) {
					if (raycastHit.transform.CompareTag("Player")) {
						playerFound = true;
						inRange = raycastHit.distance <= attackDist;
					}
				}
				else inRange = false;
			}
		}

		public void FindRandomTarget() => targetPlayer = GameObject.FindWithTag("Player").transform;

		// public bool InView() {
		// 	for (float i = -fieldOfView; i < fieldOfView; i += 4) {
		// 		Vector3 angle = DirFromAngle(i / 2);
		// 		Vector3 origin = transform.position + Vector3.up;
		// 		Vector3 dir = angle * viewDistance;
		//
		// 		Ray ray = new Ray(origin, dir);
		// 		RaycastHit raycastHit;
		// 		if (Physics.Raycast(ray, out raycastHit)) {
		// 			if (raycastHit.transform.CompareTag("Player")) return true;
		// 		}
		// 	}
		// 	return false;
		// }

		// public bool InRange() => handler.zoneOccupied;


		public int SetRandomLevel() => Random.Range(0, 100);
		public bool CheckIfAggro() => (aggressionLevel > 50);

		public Transform ClosestTarget(Collider[] targets) {
			double dist = detectionRadius;
			Transform nearestTarget = null;
			foreach (var target in targets) {
				double d = Vector3.Distance(target.transform.position, transform.position);
				if (d < dist) {
					dist = d;
					nearestTarget = target.transform;
				}
			}

			return nearestTarget;
		}

		public Collider[] PlayersInRange() =>
			Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

		public Vector3 DirFromAngle(float angleInDegrees) {
			angleInDegrees += transform.eulerAngles.y;
			//swapping around the sin and cos 
			return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,
				Mathf.Cos(angleInDegrees                * Mathf.Deg2Rad));
		}

		public void ToggleRagdoll() {
			if (rigidbodies != null || rigidbodies.Length != 0) {
				foreach (var rb in rigidbodies) {
					rb.isKinematic = animator.enabled;
				}
			}
		}


		public virtual void OnDrawGizmosSelected() {
			if (showRadius) {
				Gizmos.color = Color.green;
				Gizmos.DrawWireSphere(transform.position, detectionRadius);
			}

			if (showViewCast) {
				for (float i = -fieldOfView; i < fieldOfView; i += 4) {
					var angle = DirFromAngle(i / 2);
					RaycastHit hit;
					Ray ray = new Ray(transform.position + Vector3.up, Vector3.up + angle * viewDistance);
					if (Physics.Raycast(ray, out hit)) {
						Debug.DrawRay(transform.position + Vector3.up, angle * hit.distance, Color.red);
					}
					else {
						Debug.DrawRay(transform.position + Vector3.up, angle * viewDistance, Color.green);
						Debug.DrawRay(transform.position + Vector3.up, angle * (float) attackDist, Color.blue);
					}
				}
			}
		}
	}

	public abstract class NpcState
	{
		protected readonly AIEntity aiEntity;
		protected NpcState(AIEntity aiEntity) => this.aiEntity = aiEntity; // creates the states own constructor
		public abstract void DoActions();
	}
}