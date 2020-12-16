using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WomanSpawner : EnemySpawner
{
    [SerializeField] private float _offset = 5;

    protected override void InitItem(Enemy enemy, GameObject target)
    {
        if (enemy is Woman)
            (enemy as Woman).SetTarget(target);
    }

    protected override bool CanDestroyItem(Enemy enemy)
    {
        return _field.Rect.min.y > enemy.transform.position.z && IsVisible(enemy.transform.position);
    }

    protected override Vector3 GetRandomSpawnPosition()
    {
        int col = Random.Range(0, _field.Cols);
        return _field.GetPosition(_field.MinRow + _field.Rows, col) + Vector3.forward * _offset;
    }

    protected override void OnEnemyKilled(DamageReceiver enemy)
    {
        Sounds.main.PlayWomanDestroy(enemy.transform.position);
    }

    protected override void OnEnemyDestroyed(Enemy enemy)
    {
        Effects.main.PlayEffect(enemy.transform.position, ParticleEffectName.PapuanDestroy);
    }
}
