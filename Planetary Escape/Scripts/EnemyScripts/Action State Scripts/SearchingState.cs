using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Action/Search")]
public class SearchingState : AIState
{
    public override void DoState(EnemyController controller) => SearchArea(controller);

    void SearchArea(EnemyController controller)
    {
        
        var searchLoction = new Vector3(controller.lastKownPos.x, controller.transform.position.y, controller.lastKownPos.z);
        controller.searchTemp = searchLoction;
        controller.agent.SetDestination(controller.searchTemp);
    }
}