using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterAiming))]
public class Player : Character
{
    [SerializeField] private SkinnedMeshRenderer _mesh;
    [SerializeField] private Material _damageMaterial;
    [SerializeField] private float _afterDamageDelay;
    [SerializeField] private Collider _collider;

    private Material _defaultMaterial;
    private Animator _animator;
    private bool _couldGetDamage = true;
    private CharacterAiming _aiming;

    public UnityAction<Papuan> MetPapuan;
    public UnityAction MetRoller;
    public UnityAction MetWoman;
    public UnityAction<Bonus> CaughtBonus;

    public UnityAction<int> HealthChanged;

    protected override void Awake()
    {
        base.Awake();
        _defaultMaterial = _mesh.material;
        _animator = GetComponent<Animator>();
        _aiming = GetComponent<CharacterAiming>();
    }

    private void Start()
    {
        CharacterGroups.main.AddItemToGroupA(this);
    }

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        if (_couldGetDamage)
        {
            base.TakeDamage(damage, fromPlayer);
            StartCoroutine(ShowDamage());
            StartCoroutine(SetImmortal());
            HealthChanged?.Invoke(Health);
        }
    }

    public void EnableAim() => _aiming.enabled = true;

    public void DisableAim() => _aiming.enabled = false;

    public override bool ShouldDestroy()
    {
        _animator.SetBool("IsAlive", false);
        return false;
    }

    private IEnumerator ShowDamage()
    {
        _mesh.material = _damageMaterial;
        yield return new WaitForSeconds(0.1f);
        _mesh.material = _defaultMaterial;
    }

    private IEnumerator SetImmortal()
    {
        _couldGetDamage = false;
        yield return new WaitForSeconds(_afterDamageDelay);
        _couldGetDamage = true;
    }
}
