using UnityEngine;


public class TestDamage : MonoBehaviour
{
	public int damageAmount;

	private void OnTriggerEnter(Collider other) {
		if(other.CompareTag("Player")) other.GetComponent<GameEntity>().TakeDamage(damageAmount);
	}
}

