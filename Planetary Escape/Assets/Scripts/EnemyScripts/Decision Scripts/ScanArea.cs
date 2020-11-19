using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/Scan")]
public class ScanArea : Decision
{
    public override bool Decide(EnemyController controller) => Scan(controller);

    private bool Scan(EnemyController controller)
    {
        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
        {
            Debug.Log("Last Known pos");
            controller.transform.Rotate(0, 90 * Time.deltaTime, 0);
            Debug.DrawRay(controller.transform.position ,controller.transform.forward * controller.enemyStats.detectionRadius,Color.blue);
            return controller.CheckIfCountDownElapsed(3f);
        }
        return false;
    }
    
  
}