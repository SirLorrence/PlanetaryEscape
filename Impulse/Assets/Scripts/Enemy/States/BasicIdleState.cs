using Enemy.Enemy_Types;
using UnityEngine;

namespace Enemy.States
{
	public class BasicIdleState : NpcState
	{
		public BasicIdleState(AIEntity aiEntity) : base(aiEntity) {
		}

		public override void DoActions() {
			entity.targets = entity.PlayersInRange();
			if (entity.InView(entity.targets))
				entity.SetState(new BasicCombatState(entity));
			Idle();
		}

		public void Idle() {
			Debug.Log("Idle");
			
		}
	}
}