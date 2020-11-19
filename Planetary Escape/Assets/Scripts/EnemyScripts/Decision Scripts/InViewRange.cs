using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/InRange")]
public class InViewRange : Decision
{
    public override bool Decide(EnemyController controller) => InRange(controller);

    bool InRange(EnemyController controller)
    {
        RaycastHit hitInfo;
        if (controller.playerInRange)
        {
            Debug.Log("Player Spotted");
            if (Physics.Raycast(controller.transform.position + new Vector3(0, 2, 0),
                (controller.targetTransform.transform.position - (controller.transform.position + new Vector3(0, 2, 0)))
                .normalized, out hitInfo, controller.enemyStats.detectionRadius))
            {
                //Debug.Log($"Ray Info {hitInfo.collider.name}");
                if (hitInfo.collider.CompareTag("Player"))
                {
                    // Debug.Log("Player Spotted");
                    return true;
                }
            }
        }
        return false;
    }
}