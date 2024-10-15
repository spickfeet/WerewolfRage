public class StateMachine : IStateMachine
{
    private IState _currentState;
    public IState CurrentState => _currentState;

    public void Initialize(IState state)
    {
        _currentState = state;
        CurrentState.Enter();
    }

    public void ChangeState(IState state)
    {
        CurrentState.Exit();
        _currentState = state;
        CurrentState.Enter();
    }
}