using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public abstract class Gun : MonoBehaviour
{
    [SerializeField] private float _shootDelay = 0.3f;
    [SerializeField] private Transform[] _shootPoints;
    [SerializeField] private ParticleSystem _shotParticles;
    [SerializeField] private string _animatorTakeTrigger;
    [SerializeField] private string _animatorIdleBool;

    private AudioSource _audioSource;
    private int _currentShotPointIndex = 0;
    private bool _canShoot = true;
    private bool _triggerPushed = false;

    public string AnimatorTakeTrigger => _animatorTakeTrigger;

    public string AnimatorIdleBool => _animatorIdleBool;

    public UnityAction Shot;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PushTrigger()
    {
        _triggerPushed = true;
        TryShoot();
    }

    public void ReleaseTrigger() => _triggerPushed = false;

    private void TryShoot()
    {
        if (_canShoot == false)
            return;

        _canShoot = false;

        Shoot();
        StartCoroutine(WaitDelay());
    }

    private IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(_shootDelay);
        _canShoot = true;

        if (_triggerPushed && enabled)
            TryShoot();
    }

    private void Shoot()
    {
        _currentShotPointIndex = (_currentShotPointIndex + 1) % _shootPoints.Length;
        Transform shotPoint = _shootPoints[_currentShotPointIndex];
        Shoot(shotPoint);
        _shotParticles.transform.position = shotPoint.position;
        _shotParticles.Play();
        _audioSource.PlayOneShot(_audioSource.clip);
        Shot?.Invoke();
    }

    abstract protected void Shoot(Transform shootPoint);
}
