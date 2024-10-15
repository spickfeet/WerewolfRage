using Assets.Scripts;
using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private float _offset = 0;

    [SerializeField] private AudioClip _bulletAudioClip;
    [SerializeField] private AudioClip _reloadTwoAmmoAudioClip;
    [SerializeField] private AudioClip _reloadOneAmmoAudioClip;
    private AudioSource _audioSource;

    [SerializeField] private float _shotWidth = 0.8f;
    [SerializeField] private float _shotLength = 3.0f;

    [SerializeField] private Transform _bulletSpawner;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _fireRate = 1;
    [SerializeField] private int _damage = 3;

    [SerializeField] private Player _player;

    [SerializeField] private int _maxAmmunitionInClip = 2;
    [SerializeField] private float _reloadTime;
    private int _currentAmmunitionInClips;

    private float _nextFire = 0;

    private WeaponRotation _weaponRotation = new(false);
    private SpriteRenderer _sr;


    public Action<int> AmmoChanged;

    public void Start()
    {
        _currentAmmunitionInClips = _maxAmmunitionInClip;
        _sr = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        (transform.rotation, _sr.flipY) = _weaponRotation.ChangeRotation(transform.position, transform.rotation, _offset);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        if (Input.GetMouseButtonDown(0) && Time.time >= _nextFire)
        {
            if (_currentAmmunitionInClips <= 0)
            {
                Reload();
                return;
            }
            _nextFire = Time.time + 1.0f / _fireRate;
            Shoot();
        }
    }


    private void Reload()
    {
        if (_currentAmmunitionInClips == _maxAmmunitionInClip) return;
        if (_currentAmmunitionInClips == 0)
        {
            _audioSource.PlayOneShot(_reloadTwoAmmoAudioClip);
            _nextFire = Time.time + _reloadTime;
        }
        if (_currentAmmunitionInClips == 1)
        {
            _audioSource.PlayOneShot(_reloadOneAmmoAudioClip);
            _nextFire = Time.time + _reloadTime * 0.75f;
        }
        _currentAmmunitionInClips = _maxAmmunitionInClip;
        AmmoChanged?.Invoke(_currentAmmunitionInClips);
    }

    private void Shoot()
    {
        _audioSource.PlayOneShot(_bulletAudioClip);

        _currentAmmunitionInClips -= 1;
        AmmoChanged?.Invoke(_currentAmmunitionInClips);

        StartCoroutine(BulletEffect());
        _bulletSpawner.localPosition += new Vector3(_shotLength / 2 - 0.5f, 0, 0);

        _player.PushAway(_bulletSpawner.position, -1000);

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(_bulletSpawner.position, new Vector2(_shotLength, _shotWidth), transform.rotation.eulerAngles.z);
        _bulletSpawner.localPosition -= new Vector3(_shotLength / 2 - 0.5f, 0, 0);

        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                enemy.ApplyDamage(_damage);
                enemy.PushAway(transform.position, -10000);
            }
        }
    }

    private IEnumerator BulletEffect()
    {
        GameObject bullet = Instantiate(_bullet, _bulletSpawner.position, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        Destroy(bullet);
    }
}
