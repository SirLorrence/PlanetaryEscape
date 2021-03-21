using System;
using Managers;
using UnityEngine;

namespace UI
{
	public class ButtonHandler : MonoBehaviour
	{
		private enum ButtonActions
		{
			Play,
			Options,
			Credits,
			Quit,
		}

		[SerializeField] private ButtonActions buttonAction;

		public void PerformButtonAction() {
			switch (buttonAction) {
				case ButtonActions.Play:
					GameManager.Instance.LoadGame();
					break;
				case ButtonActions.Options:
					break;
				case ButtonActions.Credits:
					break;
				case ButtonActions.Quit:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}