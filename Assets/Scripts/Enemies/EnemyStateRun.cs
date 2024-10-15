using UnityEngine;
using UnityEngine.AI;

public class EnemyStateRun : IState
{
    private Player _player;
    private NavMeshAgent _agent;
    private Animator _animator;

    public EnemyStateRun(Enemy enemy)
    {
        _player = enemy.Player;
        _agent = enemy.Agent;
        _animator = enemy.Animator;

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    public void Enter()
    {
        _animator.Play(Enemy.AnimationNames.Run);
    }

    public void Exit()
    {
    }

    public void Update()
    {
        Vector3 target = new Vector3(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z);

        _agent.SetDestination(target);
    }
}

