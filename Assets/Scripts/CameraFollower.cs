using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed = 3.0f;

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        var nextPosition = Vector3.Lerp(transform.position, _target.position + _offset, Time.deltaTime * _speed);
        transform.position = nextPosition;
    }
}
