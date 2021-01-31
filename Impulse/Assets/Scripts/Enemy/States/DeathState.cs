using Enemy.Enemy_Types;
using UnityEngine.UI;

namespace Enemy.States
{
	public class DeathState : NpcState
	{
		public DeathState(AIEntity aiEntity) : base(aiEntity) {
		}

		public override void DoActions() {
			entity.navAgent.enabled = false;
			entity.animator.enabled = false;
			entity.ToggleRagdoll();
			//method to de-spawn 
		}
	}
}