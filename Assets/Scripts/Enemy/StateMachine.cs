using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private BaseState activeState;
    public BaseState ActiveState { get { return activeState; } set { activeState = value; } }

    public void Initialise()
    {
        ChangeState(new PatrolState());
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }

        activeState = newState;

        if (activeState != null)
        {
            activeState.StateMachine = this;
            activeState.Enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
