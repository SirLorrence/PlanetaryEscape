using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemy.States;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entities.Enemy.Enemy_Types
{
	/// <summary>
	/// this class works like an black board
	/// hold all the ai data
	/// </summary>
	[RequireComponent(typeof(NavMeshAgent))]
	public class AIEntity : GameEntity
	{
		#region Fields

		[Header("Drop Chance")] [Range(0, 100)]
		public int dropPercent;

		[Header("Detection Values")] [Range(0, 360)]
		public float fieldOfView;

		[ReadOnly] public Transform targetPlayer;
		public float detectionRadius;
		public float viewDistance = 10;
		public double attackDist;

		[Header("Aggression")] [Range(0, 100)] public int aggressionLevel;
		public bool alwaysAggro;
		[ReadOnly] [SerializeField] private bool inAggroMode;

		[Header("Damage Value")] public int dealAmount;
		[SerializeField] private AttackHandler attackHandler;

		[Header("Speed Values")] public float wonderSpeed;
		public float followSpeed;
		public float chaseSpeed;
		public float speedMultiplier = 1;

		[Header("Visual Debug and Developer Override")]
		public bool toggleBody;

		public bool showViewCast;
		public bool showRadius;
		public bool testStateChange;

		public enum StateOverride
		{
			Follow,
			Attack,
			Death
		}

		public StateOverride stateOverride;


		[HideInInspector] public Collider[] targets;

		[HideInInspector] public NavMeshAgent navAgent;

		[HideInInspector] public ZombieAnimationHandler animationHandler;


		[SerializeField] protected Rigidbody[] rigidbodies;

		protected NpcState currentState;
		protected NpcState pastState;

		protected bool playerFound, inRange;

		public bool PlayerFound => playerFound;
		public bool InRange => inRange;

		private int playerLayer = 1 << 9;

		#endregion

		protected override void Awake() {
			navAgent = GetComponentInChildren<NavMeshAgent>();
			animationHandler = gameObject.AddComponent<ZombieAnimationHandler>();
			attackHandler.amount = dealAmount;
			rigidbodies = GetComponentsInChildren<Rigidbody>();
			base.Awake();
		}

		public virtual void Start() {
			dropPercent = Random.Range(0, 100);
			ToggleRagdoll(true);
			if (testStateChange) {
				switch (stateOverride) {
					case StateOverride.Follow:
						SetState(new FollowState(this));
						break;
					case StateOverride.Attack:
						SetState(new AttackState(this));
						break;
					case StateOverride.Death:
						SetState(new DeathState(this));
						break;
				}
			}
			else SetState(new FollowState(this));
		}

		public virtual void Update() {
			var vZ = Vector3.Dot(navAgent.velocity.normalized, transform.forward);
			var vX = Vector3.Dot(navAgent.velocity.normalized, transform.right);
			animationHandler.AnimHandler(vX, vZ, inAggroMode);

			VisionArea();

			// Debug.Log($"Player Found :{playerFound}");
			// Debug.Log($"Can Attack :{inRange}");
		}

		#region Push Down Automata

		public void SetState(NpcState aState) => currentState = aState;

		//for push-down automata
		public void PushState(NpcState state) {
			pastState = currentState;
			SetState(state);
		}

		public void PopState() => currentState = pastState;

		#endregion


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


		public int SetRandomLevel() => Random.Range(0, 100);

		public int GetRandomAttack() => Random.Range(0, 2);

		public void SpawnItem(GameObject item) =>
			Instantiate(item, transform.position + Vector3.up, transform.rotation);

		public void SetAggroLevel() =>
			aggressionLevel = (alwaysAggro) ? 100 : aggressionLevel = SetRandomLevel();

		public bool CheckIfAggro() {
			inAggroMode = (aggressionLevel > 50);
			return inAggroMode;
		}

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

		public void ToggleRagdoll(bool value) {
			if (rigidbodies != null && rigidbodies.Length != 0) {
				foreach (var rigid in rigidbodies) {
					rigid.isKinematic = value;
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

		public virtual IEnumerator WaitForAnimationFinish(AIEntity entity) {
			float animationTime = entity.animationHandler.animator.GetCurrentAnimatorStateInfo(0).length;
			yield return new WaitForSeconds(animationTime);
		}
	}
}