using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraChanger : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _player.DisableAim();
        if (!GameData.FirstPlay)
            SwitchToPlayer();
    }

    private void Start()
    {
        _animator.SetTrigger("start");
    }

    public void SwitchToGate()
    {
        _animator.SetTrigger("showGate");
        _player.DisableAim();
    }

    public void SwitchToPlayer()
    {
        _animator.SetBool("isFollowingPlayer", true);
        StartCoroutine(EnableAimerAfterTransition());
    }

    private IEnumerator EnableAimerAfterTransition()
    {
        yield return null;
        while (_animator.IsInTransition(0))
            yield return null;

        _player.EnableAim();
    }
}
