public abstract class BaseState 
{
    private StateMachine stateMachine;
    public StateMachine StateMachine {  get { return stateMachine; } set { stateMachine = value; } }

    private Enemy enemy;
    public Enemy Enemy { get { return enemy; } set { enemy = value; } }

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();
}
