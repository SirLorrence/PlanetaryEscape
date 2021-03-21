using UnityEngine;

namespace Entities.Enemy.Enemy_Types
{
	public class DamageBox : MonoBehaviour
	{
		[ReadOnly]public int damageAmount;
		private void OnTriggerEnter(Collider other) {
			if (other.CompareTag("Player")) other.GetComponent<Player.Player>().TakeDamage(damageAmount);
		}
	}
}
