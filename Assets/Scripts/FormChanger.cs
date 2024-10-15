using UnityEngine;

public class FormChanger : MonoBehaviour
{
    [SerializeField] private TimeCycle _timeCycle;

    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private Sprite[] _forms;
    [SerializeField] private SpriteRenderer _playerSprite;

    [SerializeField] private AudioClip _wereWolfFormAudioClip;
    [SerializeField] private AudioClip _humanFormAudioClip;
    [SerializeField] private AudioSource _audioSource;


    private Player _player;

    private void ChangeForm(TimeOfDay timeOfDay)
    {
        if (timeOfDay == TimeOfDay.Day)
        {
            _player.WereWolfForm = false;
            _audioSource.PlayOneShot(_humanFormAudioClip);
            _playerSprite.sprite = _forms[0];
            _weapons[0].SetActive(true);
            _weapons[1].SetActive(false);
        }
        if (timeOfDay == TimeOfDay.Night)
        {
            _player.WereWolfForm = true;
            _audioSource.PlayOneShot(_wereWolfFormAudioClip);
            _playerSprite.sprite = _forms[1];
            _weapons[0].SetActive(false);
            _weapons[1].SetActive(true);
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        _player = GetComponent<Player>();
        _timeCycle.TimeOfDayChanged += ChangeForm;
    }
}
