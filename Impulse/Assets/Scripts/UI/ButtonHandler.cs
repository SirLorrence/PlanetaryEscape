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
			QuitGame,
			QuitToMenu,
			Restart,
			Back,
		}

		[SerializeField] private ButtonActions buttonAction;

		public void PerformButtonAction() {
			switch (buttonAction) {
				case ButtonActions.Play:
					GameManager.Instance.LoadGame();
					break;
				case ButtonActions.Options:
					GameManager.Instance.ToggleOptions();
					break;
				case ButtonActions.Credits:
					GameManager.Instance.ToggleCredits();
					break;
				case ButtonActions.QuitGame:
					Application.Quit(0);
					break;
				case ButtonActions.QuitToMenu:
					GameManager.Instance.LoadMenu();
					break;
				case ButtonActions.Restart:
					GameManager.Instance.ResetGame();
					break;
				case ButtonActions.Back:
					GameManager.Instance.ToggleBack();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}