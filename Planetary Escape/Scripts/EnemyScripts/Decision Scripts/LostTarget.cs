using UnityEngine;
[CreateAssetMenu(menuName = "EnemyAI/Decisions/Lost")]

public class LostTarget : Decision
{
    public override bool Decide(EnemyController controller) => TargetLost(controller);

   private bool TargetLost(EnemyController controller)
   {
       RaycastHit hitInfo;
       bool inSight = false;
        if (controller.playerInRange)
        {
            if (Physics.Raycast(controller.transform.position, (controller.targetTransform.transform.position - controller.transform.position).normalized, out hitInfo, controller.enemyStats.detectionRadius))
            {
                Debug.Log($"Ray Info {hitInfo.collider.name}");
                if (hitInfo.collider.CompareTag("Player"))
                {
                    inSight = true;
                }
            }
        } 
        if (inSight) return false;
        else
        {
            controller.lastKownPos = new Vector3(controller.targetTransform.position.x, controller.transform.position.y + 0.5f, controller.targetTransform.position.z);
            return true;
        }
    }
}