using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BaseState
{
    private float searchTimer;
    private float moveTimer;

    public override void Enter()
    {
        Enemy.Agent.SetDestination(Enemy.LastKnownPosition);
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (Enemy.CanSeePlayer())
        {
            StateMachine.ChangeState(new AttackState());
        }

        if (Enemy.Agent.remainingDistance < Enemy.Agent.stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 5))
            {
                Enemy.Agent.SetDestination(Enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }

            if (searchTimer > 5.0f) 
            {
                StateMachine.ChangeState(new PatrolState());
            }
        }
    }
}
