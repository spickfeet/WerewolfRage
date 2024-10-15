using Assets.Scripts;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float _offset;

    [SerializeField] private AudioClip _attackClip;
    private AudioSource _attackAudioSource;

    [SerializeField] private Transform _attackPos;
    [SerializeField] private float _radiusAttack = 1.5f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _attackSpeed = 2f;

    [SerializeField] private Player _player;


    [SerializeField] private GameObject _attack;

    private float _nextAttack = 0;


    private WeaponRotation weaponRotation = new(false);
    private SpriteRenderer _sr;

    // Start is called before the first frame update
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _attackAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        (transform.rotation, _sr.flipY) = weaponRotation.ChangeRotation(transform.position, transform.rotation, _offset);

        if (Input.GetMouseButton(0) && Time.time >= _nextAttack)
        {
            _nextAttack = Time.time + 1.0f / _attackSpeed;
            Attack();
        }
    }

    private void Attack()
    {
        _attackAudioSource.PlayOneShot(_attackClip);
        AttackEffect();
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_attackPos.position, _radiusAttack);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_damage);
                if (enemy.Health == 0)
                {
                    _player.ApplyHeal(1);
                }
            }
        }
    }

    private void AttackEffect()
    {
        GameObject attack = Instantiate(_attack, _attackPos.position, transform.rotation);
        Destroy(attack, 0.2f);
    }
}
