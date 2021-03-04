using Enemy.Enemy_Types;
using UnityEngine;

namespace Enemy.States
{
	public class FollowState : NpcState
	{
		public FollowState(AIEntity aiEntity) : base(aiEntity) {
		}

		public override void DoActions() {
			entity.targets = entity.PlayersInRange();
			if (entity.InView(entity.targets)) entity.SetState(new AttackState(entity));
			Idle();
		}

		public void Idle() {
			Debug.Log("Idle");
		}
	}
}