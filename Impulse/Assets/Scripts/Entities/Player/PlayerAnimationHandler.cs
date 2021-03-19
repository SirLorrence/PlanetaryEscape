using UnityEngine;

namespace Entities.Player
{
	public class PlayerAnimationHandler : MonoBehaviour
	{
		public ActiveAnimator activeAnimator;
		public Animator fpsAnimator;

		private static readonly int _IsWalking = Animator.StringToHash("isWalking");
		private static readonly int _IsRunning = Animator.StringToHash("isSprinting");
		private static readonly int _IsAiming = Animator.StringToHash("isAiming");
		private static readonly int _Reloading = Animator.StringToHash("Reload");

		private void Update() {
			fpsAnimator = activeAnimator.anim;
		}
		public void MovementAnim(float x, float y) => fpsAnimator.SetBool(_IsWalking, (x != 0f || y != 0f));
		public void SprintAnim(bool sprinting) => fpsAnimator.SetBool(_IsRunning, sprinting);
		public void AimDownAnim(bool aiming) => fpsAnimator.SetBool(_IsAiming, aiming);
		public void ReloadAnim() => fpsAnimator.SetTrigger(_Reloading);

		// }
		//
		//
	}
}