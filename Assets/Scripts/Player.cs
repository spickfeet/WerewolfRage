using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _speed;

    [SerializeField] private AudioClip _wolfFormHurtAudioClip;
    [SerializeField] private AudioClip _humanFormHurtAudioClip;
    [SerializeField] private AudioClip _deadAudioClip;

    [SerializeField] private TimeCycle _timeCycle;

    private Animator _animator;

    private AudioSource _audioSource;

    private int _currentHealth;

    private bool _wereWolfForm = false;
    public bool WereWolfForm
    {
        get { return _wereWolfForm; }
        set { _wereWolfForm = value; }
    }

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    public Action<float> HealthChanged;

    void Start()
    {
        _currentHealth = _maxHealth;

        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void ApplyHeal(int health)
    {
        _currentHealth += health;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        HealthChanged?.Invoke((float)_currentHealth / _maxHealth);
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth > 0)
        {
            if (_wereWolfForm == true) _audioSource.PlayOneShot(_wolfFormHurtAudioClip);
            if (_wereWolfForm == false) _audioSource.PlayOneShot(_humanFormHurtAudioClip);
        }
        HealthChanged?.Invoke((float)_currentHealth/_maxHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene("DeadScene");
    }

    private void Move()
    {
        if (_timeCycle.TimeOfDay == TimeOfDay.Day)
        {
            _animator.Play("A_Human_Run");
        }
        else
        {
            _animator.Play("A_Wolf_Run");
        }

        float moveVertical = Input.GetAxis("Vertical");
        float moveHorizontal = Input.GetAxis("Horizontal");
        FlipRotation(moveHorizontal);
        Vector3 movePos = new Vector3(moveHorizontal, moveVertical, 0);
        _rigidbody.MovePosition(transform.position + movePos * _speed * Time.deltaTime);
    }

    public void PushAway(Vector2 pushFrom, float pushPower)
    {
        pushFrom -= (Vector2)transform.position;
        _rigidbody.AddForce(pushFrom.normalized * pushPower);
    }

    private void FlipRotation(float moveHorizontal)
    {
        if (moveHorizontal < 0)
        {
            _spriteRenderer.flipX = true;
        }
        if (moveHorizontal > 0)
        {
            _spriteRenderer.flipX = false;
        }
    }
}
