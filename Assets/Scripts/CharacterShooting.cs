using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Gun> _weapons;
    [SerializeField] private Transform _weaponBone;
    [SerializeField] private LayerMask _shootableMask;

    private Gun _currentWeapon;
    private Animator _animator;
    private int _currentWeaponIndex = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _currentWeapon = _weapons[_currentWeaponIndex];
        TakeGun(_currentWeapon);
    }

    private void TakeNextGun()
    {
        _currentWeaponIndex++;
        if (_currentWeaponIndex > _weapons.Count - 1)
            _currentWeaponIndex = 0;

        TakeGun(_weapons[_currentWeaponIndex]);
    }

    private void TakePreviosGun()
    {
        _currentWeaponIndex--;
        if (_currentWeaponIndex < 0)
            _currentWeaponIndex = _weapons.Count - 1;

        TakeGun(_weapons[_currentWeaponIndex]);
    }

    private void TakeGun(Gun gun)
    {
        foreach (Gun weapon in _weapons)
        {
            weapon.gameObject.SetActive(weapon == gun);
            _animator.SetBool(weapon.AnimatorIdleBool, weapon == gun);

            if (weapon == gun)
                weapon.Shot += OnWeaponShot;
            else
                weapon.Shot -= OnWeaponShot;
        }

        _animator.SetTrigger(gun.AnimatorTakeTrigger);
        _currentWeapon = gun;
        gun.transform.parent = _weaponBone;
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
    }

    private void Update()
    {
        TurnWeaponToTarget();

        if (Input.GetMouseButtonDown(0))
            _currentWeapon.PushTrigger();

        if (Input.GetMouseButtonUp(0))
            _currentWeapon.ReleaseTrigger();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            TakeGun(_weapons[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            TakeGun(_weapons[1]);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            TakeGun(_weapons[2]);

        if (Input.mouseScrollDelta.y > 0)
            TakeNextGun();
        else if (Input.mouseScrollDelta.y < 0)
            TakePreviosGun();
    }

    private void TurnWeaponToTarget()
    {
        Vector3 shotPoint = _weaponBone.transform.position;
        Vector3 direction = _camera.transform.forward;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _shootableMask))
            direction = (hit.point - shotPoint).normalized;

        _weaponBone.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void OnWeaponShot()
    {
        _animator.SetTrigger("shoot");
    }
}
