using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/CanAttack")]
    public class InAttackRange : Decision
    {
        public override bool Decide(EnemyController controller) => InRange(controller);
    
        bool InRange(EnemyController controller)
        {
            if (controller.attackInRange) return true;
            else return false;
        }
    }
