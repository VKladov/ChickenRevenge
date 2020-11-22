using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : DamageReceiver
{
    [SerializeField] private ParticleSystem _damageParticle;

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        base.TakeDamage(damage, fromPlayer);
        _damageParticle.Play();

        if (_health > 0)
            Sounds.main.PlayHitSound(transform.position);
    }
}