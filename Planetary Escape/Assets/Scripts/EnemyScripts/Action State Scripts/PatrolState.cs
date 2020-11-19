using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Action/Patrol")]
public class PatrolState : AIState
{
   public override void DoState(EnemyController controller)=> Patrol(controller);
    void Patrol(EnemyController controller)
    {
        // controller.materialColor.color = Color.cyan;
        controller.agent.SetDestination(controller.partolPoints[controller.CurrentPoint].transform.position);
        //Debug.Log("Moving");
        if (controller.agent.remainingDistance <= controller.agent.stoppingDistance && !controller.agent.pathPending)
        {
            //Debug.Log("stop");
            while (true)
            {
                // int temp = ((1 + controller.CurrentPoint) + controller.partolPoints.Count) % controller.partolPoints.Count; // acts like a overflow handler || in order
                int temp = Random.Range(0, controller.partolPoints.Count);
                if (temp != controller.CurrentPoint)
                {
                    controller.CurrentPoint = temp;
                    break;
                }
            }
        }
        
    }


 
}