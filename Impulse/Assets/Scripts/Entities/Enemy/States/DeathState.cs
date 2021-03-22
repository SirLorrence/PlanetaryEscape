using Entities.Enemy.Enemy_Types;
using Managers;
using UnityEngine;

namespace Entities.Enemy.States
{
	public class DeathState : NpcState
	{
		private float randomValue;
		private bool animTriggered;
		public bool deathTriggered;

		public DeathState(AIEntity aiEntity) : base(aiEntity) {
			randomValue = Random.value;
			animTriggered = false;
			deathTriggered = false;
			Debug.Log(randomValue);
			GameManager.Instance.ZombieKilled();
		}

		public override void DoActions() {
			if (!deathTriggered) DropItem();
		}

		public void DropItem() {
			SoundManager.Instance.PlayAudio(AudioTypes.SFX_COMMON_ZOMBIE_DEATH);
			if (!animTriggered) {
				aiEntity.animationHandler.animator.SetTrigger(ZombieAnimationHandler.DeathTrigger);

				animTriggered = !animTriggered;
			}

			aiEntity.StartCoroutine(WaitForAnimationFinish(aiEntity));
			if (randomValue <= (aiEntity.dropPercent * .01f)) {
				Debug.Log("Item Drop");
				var item = DropManager.dropManager.GetRandomItem();
				aiEntity.SpawnItem(item);
			}

			deathTriggered = true;
		}

		// 	aiEntity.navAgent.enabled = false;
		// 	aiEntity.animator.enabled = false;
		// 	aiEntity.ToggleRagdoll();
		// 	//method to de-spawn 
		// }
	}
}