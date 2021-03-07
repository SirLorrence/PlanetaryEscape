using Enemy.Enemy_Types;
using UnityEngine.UI;

namespace Enemy.States
{
	public class DeathState : NpcState
	{
		public DeathState(AIEntity aiEntity) : base(aiEntity) {
		}

		public override void DoActions() {
			aiEntity.navAgent.enabled = false;
			aiEntity.animator.enabled = false;
			aiEntity.ToggleRagdoll();
			//method to de-spawn 
		}
	}
}