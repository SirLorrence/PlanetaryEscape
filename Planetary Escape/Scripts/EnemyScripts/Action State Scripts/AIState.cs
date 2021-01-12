using UnityEngine;

    public abstract class AIState : ScriptableObject 
    {
        public abstract void DoState(EnemyController controller);
    }
