using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;

    protected int _health;

    public UnityAction<DamageReceiver> Destroyed;
    public UnityAction<DamageReceiver> Killed;
    public UnityAction<DamageReceiver> Disappeared;
    public bool IsDamaged => _health < _maxHealth;

    protected virtual void Awake()
    {
        _health = _maxHealth;
    }

    virtual public void TakeDamage(int damage, bool fromPlayer = false)
    {
        _health -= damage;

        if (_health <= 0)
        {
            if (fromPlayer)
                Killed?.Invoke(this);

            Destroy();
        }
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
        StartCoroutine(DestroyInNextFrame());
    }

    virtual public void Recover()
    {
        _health = _maxHealth;
    }

    private IEnumerator DestroyInNextFrame()
    {
        yield return null;
        Destroy(gameObject);
    }

    private void OnDestroy() => Disappeared?.Invoke(this);
}
