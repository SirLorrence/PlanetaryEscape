using System;
using UnityEngine;


public class BodyAnimation : MonoBehaviour
{
	public ActiveAnimator activeAnimator;
	public Animator fpsAnimator;
	public Animator bodyAnimator;

//body
	private static readonly int XInput = Animator.StringToHash("xInput");
	private static readonly int ZInput = Animator.StringToHash("zInput");
	private static readonly int IsCrouch = Animator.StringToHash("isCrouch");
	private static readonly int IsSliding = Animator.StringToHash("isSliding");
	private static readonly int IsRunning = Animator.StringToHash("isSprinting");

	//arms

	private static readonly int walking = Animator.StringToHash("Walk");
	// private static readonly int ZInput = Animator.StringToHash("zInput");
	// private static readonly int IsCrouch = Animator.StringToHash("isCrouch");
	// private static readonly int IsSliding = Animator.StringToHash("isSliding");
	// private static readonly int IsRunning = Animator.StringToHash("isSprinting");

	private void Update() => fpsAnimator = activeAnimator.anim;

	public void MovementAnim(float x, float z) {
		bodyAnimator.SetFloat(XInput, x, 0.1f, Time.deltaTime);
		bodyAnimator.SetFloat(ZInput, z, 0.1f, Time.deltaTime);

		if (bodyAnimator.GetFloat(XInput) > 0.2f || bodyAnimator.GetFloat(XInput) < -0.2f ||
		    bodyAnimator.GetFloat(ZInput) > 0.2f || bodyAnimator.GetFloat(ZInput) < -0.2f) {
		
			fpsAnimator.SetBool(walking, true);
		}
		else {
			fpsAnimator.SetBool(walking, false);
		}
	}
	public void CrouchAnim(bool isCrouched) => bodyAnimator.SetBool(IsCrouch, isCrouched);
}