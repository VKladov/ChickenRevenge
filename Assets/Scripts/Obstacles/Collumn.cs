using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collumn : Obstacle
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out DamageReceiver destroyable))
        {
            destroyable.TakeDamage(10);
        }
    }
}
