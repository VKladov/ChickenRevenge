using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private DoubleRifle _weapon;

    private Vector3 _cameraLookPointDefaultPosition;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _weapon.TryShoot();
    }

    private void OnEnable()
    {
        _weapon.Shot += OnWeaponShot;
        _weapon.enabled = true;
    }

    private void OnDisable()
    {
        _weapon.Shot -= OnWeaponShot;
        _weapon.enabled = false;
    }

    private void OnWeaponShot()
    {
        _animator.SetTrigger("shoot");
    }
}
