using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Palm : Obstacle
{
    private Animator _animator;

    override protected void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        _animator.SetTrigger("hit");
        Sounds.main.PlayWoodHit(transform.position);
    }
}
