using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;

    private int _health;

    public UnityAction<DamageReceiver> Destroyed;
    public UnityAction<DamageReceiver> Killed;
    public UnityAction<DamageReceiver> Disappeared;
    public UnityAction<int> HealthChanged;

    public bool IsDamaged => _health < _maxHealth;
    public int Health
    {
        set
        {
            _health = value;
            HealthChanged?.Invoke(_health);
        }

        get
        {
            return _health;
        }
    }
    public int MaxHealth => _maxHealth;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
        if (ShouldDestroy())
            StartCoroutine(DestroyInNextFrame());
    }

    virtual public void TakeShot(int damage)
    {
        TakeDamage(damage, true);
    }

    virtual public void TakeHit(int damage, Vector3 direction, bool fromPlayer = false)
    {
        TakeDamage(damage, fromPlayer);
    }

    virtual protected void TakeDamage(int damage, bool fromPlayer = false)
    {
        Health -= damage;
        if (_health <= 0)
        {
            if (fromPlayer)
                Killed?.Invoke(this);

            Destroy();
        }
    }

    virtual protected bool ShouldDestroy() => true;

    protected virtual void Awake()
    {
        Health = _maxHealth;
    }

    private void OnDestroy() => Disappeared?.Invoke(this);

    private IEnumerator DestroyInNextFrame()
    {
        yield return null;
        Destroy(gameObject);
    }
}
