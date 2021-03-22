using System;
using Entities.Player;
using UnityEngine;

namespace Items
{
	class PickUp : MonoBehaviour
	{
		private enum Items
		{
			Health,
			Shield,
			Ammo
		}

		[SerializeField] private Items itemType;
		[SerializeField] private int itemValue;

		private void OnTriggerEnter(Collider other) {
			if (other.CompareTag("Player")) {
				var playerScript = other.GetComponent<Player>();
				ActivateItem(itemType, playerScript);
				Destroy(gameObject);
			}
		}

		void ActivateItem(Items objectType, Player playerScript) {
			switch (objectType) {
				case Items.Health:
					playerScript.AddHealth(itemValue);
					break;
				case Items.Shield:
					playerScript.AddArmor(itemValue);
					break;
				case Items.Ammo:
					playerScript.PlayerShoot.AddToReserve(itemValue);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(objectType), objectType, null);
			}
		}
	}
}