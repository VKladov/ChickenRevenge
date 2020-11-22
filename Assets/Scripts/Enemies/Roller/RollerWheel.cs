using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerWheel : DamageReceiver
{
    [SerializeField] private ParticleSystem _damageEffect;

    public override void TakeDamage(int damage, bool fromPlayer = false)
    {
        base.TakeDamage(damage, fromPlayer);

        if (_health > 0)
            _damageEffect.Play();
    }
}
