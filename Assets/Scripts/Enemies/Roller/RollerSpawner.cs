using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RollerSpawner : EnemySpawner
{
    [SerializeField] private float _offset;

    protected override void InitItem(Enemy enemy, GameObject target)
    {
        bool toLeft = enemy.transform.position.x > _field.transform.position.x + _field.CellSize * _field.Cols * 0.5f;
        Vector3 direction = toLeft ? Vector3.left : Vector3.right;
        if (enemy is Roller)
            (enemy as Roller).Init(direction, target);
    }

    protected override bool CanDestroyItem(Enemy enemy)
    {
        return (_field.Rect.min.x - _offset * 1.1 > enemy.transform.position.x || enemy.transform.position.x > _field.Rect.max.x + _offset * 1.1) &&
            IsVisible(enemy.transform.position) == false;
    }

    protected override Vector3 GetRandomSpawnPosition()
    {
        bool toLeft = Random.Range(0.0f, 0.1f) > 0.5f;
        int col = toLeft ? _field.Cols - 1 : 0;
        int row = Random.Range(_field.MinRow, _field.MinRow + _field.PlayerRows);
        return _field.GetPosition(row, col) + (toLeft ? (Vector3.left * -_offset) : (Vector3.right * -_offset));
    }

    protected override void OnEnemyKilled(DamageReceiver enemy) { }

    protected override void OnEnemyDestroyed(Enemy enemy)
    {
        Sounds.main.PlayRollerDestroy(enemy.transform.position);
        Effects.main.PlayEffect(enemy.transform.position, ParticleEffectName.RollerDestroy);
    }
}