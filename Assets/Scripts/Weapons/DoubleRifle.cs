using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class DoubleRifle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _paticles;
    [SerializeField] private Transform[] _shotPoints;
    [SerializeField] private Camera _aimingCamera;
    [SerializeField] private BulletTrail _trailPrefab;
    [SerializeField] private LayerMask _shootable;
    [SerializeField] private float _shootDelay = 0.3f;

    private float _maxDistance = 100;
    private int _currentShotPointIndex = 0;
    private bool _canShoot = true;
    private AudioSource _shootSound;

    public UnityAction Shot;

    private void Awake()
    {
        _shootSound = GetComponent<AudioSource>();
    }

    public void TryShoot()
    {
        if (_canShoot == false)
            return;

        _canShoot = false;

        Shoot();
        StartCoroutine(WaitDelay());
    }

    public IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(_shootDelay);
        _canShoot = true;

        if (Input.GetMouseButton(0) && enabled)
            TryShoot();
    }

    public void Shoot()
    {
        _currentShotPointIndex++;
        if (_currentShotPointIndex == _shotPoints.Length)
            _currentShotPointIndex = 0;

        Vector3 shotPoint = _shotPoints[_currentShotPointIndex].position;
        Vector3 direction = transform.forward;
        if (Physics.Raycast(_aimingCamera.transform.position, _aimingCamera.transform.forward, out RaycastHit hit, _shootable))
        {
            direction = (hit.point - shotPoint).normalized;
        }


        Vector3 trailTarget;
        if (Physics.Raycast(new Ray(shotPoint, direction), out RaycastHit target, _shootable))
        {
            if (target.collider.gameObject.TryGetComponent(out DamageReceiver destroyable))
                destroyable.TakeShot(1);

            trailTarget = target.point;
        }
        else
        {
            trailTarget = shotPoint + direction * _maxDistance;
        }


        BulletTrail trail = Instantiate(_trailPrefab, shotPoint, Quaternion.identity);

        _paticles.transform.position = shotPoint;
        _paticles.Play();
        _shootSound.PlayOneShot(_shootSound.clip);

        Shot?.Invoke();
    }
}
