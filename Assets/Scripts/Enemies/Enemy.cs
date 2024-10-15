using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent), typeof(BoxCollider2D), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioClip _attackAudioClip;
    [SerializeField] private AudioClip _hurtAudioClip;
    [SerializeField] private AudioClip _deadAudioClip;
    private AudioSource _audioSource;

    [SerializeField] private int _health = 10;
    public int Health => _health;

    [SerializeField] private int _damage;
    public int Damage => _damage;

    [SerializeField] private float _attackDelay = 2f;
    public float AttackDelay => _attackDelay;

    [SerializeField] private float _attackRadius;
    public float AttackRadius => _attackRadius;

    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    [SerializeField] private Transform _attackPoint;
    public Transform AttackPoint => _attackPoint;

    private Rigidbody2D _rigidbody;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    private Animator _animator;
    public Animator Animator => _animator;

    private StateMachine _stateMachine;

    private EnemyStateRun _stateRun;
    private EnemyStateAttack _stateAttack;

    public Action<Enemy> Dead;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _stateMachine = new StateMachine();
        _stateRun = new EnemyStateRun(this);
        _stateAttack = new EnemyStateAttack(this);

        _stateAttack.Attacked += OnAttacked;

        _stateMachine.Initialize(_stateRun);
        _agent.SetDestination(new Vector3(_player.transform.position.x, _player.transform.position.y, _player.transform.position.z));
    }

    private void Update()
    {
        _stateMachine.CurrentState.Update();

        FlipToTarget(_player.transform.position);

        if (Vector3.Distance(transform.position, _player.transform.position) <= _attackPoint.localPosition.x + _attackRadius)
        {
            if (_stateMachine.CurrentState == _stateAttack)
                return;
            _audioSource.PlayOneShot(_attackAudioClip);
            _stateMachine.ChangeState(_stateAttack);
        }
    }

    private void FlipToTarget(Vector3 target)
    {
        if (target.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), 
                transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), 
                transform.localScale.y, transform.localScale.z);
        }
    }

    public void ApplyDamage(int damage)
    {
        _health -= damage;
        if (_health > 0)
        {
            _audioSource.PlayOneShot(_hurtAudioClip);
        }
        if (_health <= 0)
        {
            StartCoroutine(Die());
            Dead?.Invoke(this);
        }
    }

    public void PushAway(Vector2 pushFrom, float pushPower)
    {
        pushFrom -= (Vector2)transform.position;
        _rigidbody.AddForce(pushFrom.normalized * pushPower);
    }

    private IEnumerator Die()
    {
        _audioSource.PlayOneShot(_deadAudioClip);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private void OnAttacked()
    {
        _stateMachine.ChangeState(_stateRun);
    }

    private void OnDrawGizmosSelected()
    {
        _agent = GetComponent<NavMeshAgent>();

        Gizmos.DrawWireSphere(transform.position, _agent.stoppingDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
    }

    public class AnimationNames
    {
        public const string Run = "A_Enemy_Run";
        public const string Attack = "A_Enemy_Attack";
    }
}