
using UnityEngine;
using Managers;

public class ZombieSpawnPoint : MonoBehaviour
{
    [ReadOnly] [SerializeField] private bool isBeingUsed = false;
    [ReadOnly] [SerializeField] private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        if (player == null) { Debug.LogWarning("No Player Found");  return; }

        float xDiff = player.transform.position.x - transform.position.x;
        float yDiff = player.transform.position.z - transform.position.z;
        float hypotenuse = (xDiff * xDiff) + (yDiff * yDiff);
        if (hypotenuse > WaveManager.Instance.spawnPointRangeMin * WaveManager.Instance.spawnPointRangeMin && hypotenuse < WaveManager.Instance.spawnPointRangeMax * WaveManager.Instance.spawnPointRangeMax)
        {
            if (isBeingUsed) return;

            WaveManager.Instance.AddSpawnPoint(this.gameObject);
            isBeingUsed = true;
        }
        else
        {
            if (!isBeingUsed) return;

            isBeingUsed = false;

            if (WaveManager.Instance.spawnPoints.Contains(this.gameObject))
                WaveManager.Instance.spawnPoints.Remove(this.gameObject);
        }
    }
}
