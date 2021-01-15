using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class CharacterShooting : MonoBehaviour
{
    [SerializeField] private GameInput _input;
    [SerializeField] private Camera _camera;
    [SerializeField] private Gun _weapon;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _weapon.Shot += OnWeaponShot;
    }

    private void OnDisable()
    {
        _weapon.ReleaseTrigger();
        _weapon.Shot -= OnWeaponShot;
    }

    private void Update()
    {
        if (_input.WasShootPressed)
            _weapon.PushTrigger();

        if (_input.WasShootReleased)
            _weapon.ReleaseTrigger();
    }

    private void OnWeaponShot()
    {
        _animator.SetTrigger("shoot");
    }
}
