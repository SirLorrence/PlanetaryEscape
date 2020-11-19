using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/State")]
public class State : ScriptableObject
{
    public AIState[] actions;
    public Transition[] transitions;

    public void UpdateState(EnemyController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }
    private void DoActions(EnemyController controller)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].DoState(controller);
        }
    }
    private void CheckTransitions(EnemyController controller)
    {
        for (int i = 0; i < transitions.Length; i++) 
        {
            bool decisionSucceeded = transitions [i].decision.Decide (controller);

            if (decisionSucceeded) {
                controller.TransitionToState (transitions [i].trueState);
            }
            else 
            {
                controller.TransitionToState (transitions [i].falseState);
            }
        }
    }

}
