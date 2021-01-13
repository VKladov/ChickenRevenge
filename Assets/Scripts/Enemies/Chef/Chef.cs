using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chef : MonoBehaviour
{
    [SerializeField] public BodyPartCollider TopCollider;
    [SerializeField] public BodyPartCollider BottomCollider;

    [SerializeField] private ParticleSystem _blockParticles;
    [SerializeField] private MittenThrower _thrower;
    [SerializeField] private float _throwDelay;
    [SerializeField] private float _standDelay;

    private Animator _animator;
    private Player _target;
    private string[] _dodgeTriggers = new string[] { "dodge_left", "dodge_right" };
    private bool _stanned = false;

    public const string AnimatorTriggerBlockTop = "block_top";
    public const string AnimatorTriggerBlockBottom = "block_bottom";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _target = FindObjectOfType<Player>();
    }

    private void Start()
    {
        TopCollider.Reflective = _thrower.HasMitten;
        BottomCollider.Reflective = _thrower.HasMitten;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position, Vector3.up);

        if (Input.GetKeyDown(KeyCode.L))
            Attack();
    }

    private void OnEnable()
    {
        TopCollider.GotShot += OnGotTopShot;
        BottomCollider.GotShot += OnGotBottomShot;
        _thrower.Сaught += OnСaught;
    }

    private void OnDisable()
    {
        TopCollider.GotShot -= OnGotTopShot;
        BottomCollider.GotShot -= OnGotBottomShot;
        _thrower.Сaught -= OnСaught;
    }

    private void OnGotTopShot(Vector3 hitPoint, Vector3 direction)
    {
        if (_thrower.HasMitten)
        {
            _animator.SetTrigger("block_top");
            _blockParticles.Play();
        }
        else
        {
            _animator.SetTrigger(_dodgeTriggers[Random.Range(0, _dodgeTriggers.Length)]);
        }
    }

    private void OnGotBottomShot(Vector3 hitPoint, Vector3 direction)
    {
        if (_thrower.HasMitten)
        {
            _animator.SetTrigger("block_bottom");
            _blockParticles.Play();
        }
        else
        {
            Stan();
        }
    }

    private void Attack()
    {
        if (_thrower.HasMitten == false)
            return;

        _animator.SetTrigger("throw");
        StartCoroutine(ThrowMitten(_throwDelay));
    }

    private IEnumerator ThrowMitten(float delay)
    {
        yield return new WaitForSeconds(delay);

        TopCollider.Reflective = false;
        BottomCollider.Reflective = false;
        _thrower.Throw(_target.transform.position + Vector3.up * 0.5f);
    }

    private void OnСaught()
    {
        _animator.SetTrigger("catch");
        TopCollider.Reflective = true;
        BottomCollider.Reflective = true;
    }

    private void Stan()
    {
        _animator.SetBool("stanned", true);
        StartCoroutine(StandUp(_standDelay));
    }

    private IEnumerator StandUp(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.SetBool("stanned", false);
    }

    public void SetAnimationTrigger(string name)
    {
        _animator.SetTrigger(name);
    }

    public void SetAnimationBool(string name, bool value)
    {
        _animator.SetBool(name, value);
    }
}
