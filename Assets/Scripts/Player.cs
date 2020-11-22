using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterAiming))]
[RequireComponent(typeof(CharacterShooting))]
[RequireComponent(typeof(CharacterMover))]
public class Player : MonoBehaviour
{
    [SerializeField] PapuanMeetCollider _papuanTrigger;
    [SerializeField] private GameField _field;
    private Animator _animator;
    private CharacterAiming _aiming;
    private CharacterMover _mover;
    private CharacterShooting _shooter;

    public bool IsStanned { get; private set; } = false;
    public UnityAction<Papuan> MetPapuan;
    public UnityAction MetRoller;
    public UnityAction MetWoman;
    public UnityAction<Bonus> CaughtBonus;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _aiming = GetComponent<CharacterAiming>();
        _mover = GetComponent<CharacterMover>();
        _shooter = GetComponent<CharacterShooting>();
    }

    private void OnEnable()
    {
        _papuanTrigger.MetPapuan += OnMetPapuan;
    }

    private void OnDisable()
    {
        _papuanTrigger.MetPapuan -= OnMetPapuan;
    }

    public void Stan()
    {
        IsStanned = true;
        int bottomLayerIndex = _animator.GetLayerIndex("BottomLayer");
        int topLayerIndex = _animator.GetLayerIndex("TopLayer");
        int commonLayerIndex = _animator.GetLayerIndex("CommonLayer");
        _animator.SetLayerWeight(bottomLayerIndex, 0);
        _animator.SetLayerWeight(topLayerIndex, 0);
        _animator.SetLayerWeight(commonLayerIndex, 1);

        _aiming.enabled = false;
        _mover.enabled = false;
        _shooter.enabled = false;
        transform.position = _field.GetPositionOnTerrain(transform.position);
        _animator.SetBool("IsStanned", true);
    }

    public void FinishStan()
    {
        IsStanned = false;
        _animator.SetBool("IsStanned", false);

        StartCoroutine(ReturnControl());
    }

    private IEnumerator ReturnControl()
    {
        yield return new WaitForSeconds(2);
        int bottomLayerIndex = _animator.GetLayerIndex("BottomLayer");
        int topLayerIndex = _animator.GetLayerIndex("TopLayer");
        int commonLayerIndex = _animator.GetLayerIndex("CommonLayer");
        _animator.SetLayerWeight(bottomLayerIndex, 1);
        _animator.SetLayerWeight(topLayerIndex, 1);
        _animator.SetLayerWeight(commonLayerIndex, 0);

        _aiming.enabled = true;
        _mover.enabled = true;
        _shooter.enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (IsStanned)
            return;

        if (other.gameObject.TryGetComponent(out Bonus bonus))
        {
            CaughtBonus?.Invoke(bonus);
            Destroy(bonus.gameObject);
        }
        else if (other.gameObject.GetComponent<RollerWheel>())
        {
            MetRoller?.Invoke();
        }
        else if (other.gameObject.GetComponent<Woman>())
        {
            MetWoman?.Invoke();
        }
    }

    private void OnMetPapuan(Papuan papuan) => MetPapuan?.Invoke(papuan);
}
