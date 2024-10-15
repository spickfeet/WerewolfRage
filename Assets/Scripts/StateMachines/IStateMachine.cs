public interface IStateMachine
{
    void Initialize(IState state);

    void ChangeState(IState state);
}
