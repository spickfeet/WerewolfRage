using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Firefly : MonoBehaviour
{
    [SerializeField] private NavMeshPath _navMeshPath;
    [SerializeField] private float _radius;

    private NavMeshAgent _agent;

    private Vector3 _target;

    private Vector3 _startPosition;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _navMeshPath = new NavMeshPath();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _startPosition = transform.position;

        SetRandomTarget();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _target) <= 0.1f)
        {
            SetRandomTarget();
        }
    }

    private void SetRandomTarget()
    {        
        Vector3 randomPoint = Vector3.zero;
        bool isCorrectPath = false;
        while (isCorrectPath == false)
        {
            _target = _startPosition + GetRandomDirection();
            NavMesh.SamplePosition(_target, out NavMeshHit hit, _radius, NavMesh.AllAreas);
            randomPoint = hit.position;

            _agent.CalculatePath(randomPoint, _navMeshPath);
            if (_navMeshPath.status == NavMeshPathStatus.PathComplete) isCorrectPath = true;
        }
        _target = randomPoint;
        _agent.SetDestination(_target);
    }

    private Vector3 GetRandomDirection()
    {
        return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_startPosition, Vector3.one * _radius);
    }
}
