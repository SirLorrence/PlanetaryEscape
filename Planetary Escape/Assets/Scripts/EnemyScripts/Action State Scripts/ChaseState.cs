using UnityEngine;
[CreateAssetMenu(menuName = "EnemyAI/Action/Chase")]

public class ChaseState : AIState
{
    protected static readonly int ChaseAnim = Animator.StringToHash("ChaseAnim");
    public override void DoState(EnemyController controller) => Chase(controller);
    void Chase(EnemyController controller)
    {
        // controller.materialColor.color = Color.yellow;
        controller.anim.SetBool(ChaseAnim , true);
        if (Vector3.Distance(controller.transform.position, controller.targetTransform.position) < controller.enemyStats.attackRangeRadius)
            controller.agent.SetDestination(controller.transform.position);
        else
            controller.agent.SetDestination(controller.targetTransform.position);

        Debug.DrawRay(controller.transform.position , (controller.targetTransform.transform.position - controller.transform.position).normalized * controller.enemyStats.detectionRadius, Color.green);
    }
   
}