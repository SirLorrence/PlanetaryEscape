using System;
using UnityEngine;


public class BodyAnimation : MonoBehaviour
{
	public ActiveAnimator activeAnimator;
	public Animator fpsAnimator;
	public Animator bodyAnimator;

	//body specific 
	private static readonly int XInput = Animator.StringToHash("xInput");
	private static readonly int ZInput = Animator.StringToHash("zInput");
	private static readonly int IsCrouch = Animator.StringToHash("isCrouch");
	private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

	//arms specific
	private static readonly int Walking = Animator.StringToHash("Walk");


	//universal 
	private static readonly int IsRunning = Animator.StringToHash("isSprinting");
	private static readonly int IsAiming = Animator.StringToHash("Aim");
	private static readonly int Reloading = Animator.StringToHash("Reload");

	private void Update()
	{
		 fpsAnimator = activeAnimator.anim;
	}

	public void CrouchAnim(bool crouched) => bodyAnimator.SetBool(IsCrouch, crouched);
	public void InAirAnim(bool grounded) => bodyAnimator.SetBool(IsGrounded, grounded);

	public void MovementAnim(float x, float z) {
		bodyAnimator.SetFloat(XInput, x, 0.1f, Time.deltaTime);
		bodyAnimator.SetFloat(ZInput, z, 0.1f, Time.deltaTime);

		if (bodyAnimator.GetFloat(XInput) > 0.2f || bodyAnimator.GetFloat(XInput) < -0.2f ||
			bodyAnimator.GetFloat(ZInput) > 0.2f || bodyAnimator.GetFloat(ZInput) < -0.2f)
		{
			fpsAnimator.SetBool(Walking, true);
		}
		else
		{
			if (fpsAnimator != null) fpsAnimator.SetBool(Walking, false);
		}
	}

	public void AimDownAnim(bool aiming) {
		if (fpsAnimator != null) fpsAnimator.SetBool(IsAiming, aiming);
		bodyAnimator.SetBool(IsAiming, aiming);
	}

	public void ReloadAnim(bool reloading) {
		if (reloading) {
			fpsAnimator.SetTrigger(Reloading);
			reloading = false;
		}
	}


	public void SprintAnim(bool sprinting) {
		if (fpsAnimator != null) fpsAnimator.SetBool(IsRunning, sprinting);
		bodyAnimator.SetBool(IsRunning, sprinting);
	}
}