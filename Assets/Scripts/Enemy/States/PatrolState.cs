using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private int waypointIndex;
    private float waitTimer;
    private float timeToPatrolAtWaypoint = 3f;

    public override void Enter()
    {
        //throw new System.NotImplementedException();
    }

    public override void Perform()
    {
        PatrolCycle();

        if (Enemy.CanSeePlayer())
        {
            StateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {
        //throw new System.NotImplementedException();
    }

    public void PatrolCycle()
    {
        if (Enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > timeToPatrolAtWaypoint)
            {
                if (waypointIndex < Enemy.Path.Waypoints.Count - 1)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                }

                Enemy.Agent.SetDestination(Enemy.Path.Waypoints[waypointIndex].position);

                waitTimer = 0;
            }
        }
    }
}
