using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out DamageReceiver destroyable))
            destroyable.TakeDamage(_damage);
    }
}
