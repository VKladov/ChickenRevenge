using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperSpawner : EnemySpawner
{
    [SerializeField] private float _offset = 5;

    protected override bool CanDestroyItem(Enemy enemy)
    {
        return (enemy.transform.position.x > _field.Rect.max.x + _offset * 1.1 || enemy.transform.position.x < _field.Rect.min.x - _offset * 1.1) &&
            IsVisible(enemy.transform.position) == false;
    }

    protected override Vector3 GetRandomSpawnPosition()
    {
        bool toLeft = Random.Range(0f, 2f) > 1;
        int col = toLeft ? _field.Cols - 1 : 0;
        int row = Random.Range(_field.MinRow, _field.MinRow + _field.PlayerRows);
        return _field.GetPosition(row, col) + (toLeft ? (Vector3.left * -_offset) : (Vector3.right * -_offset));
    }

    protected override void InitItem(Enemy enemy, GameObject target)
    {
        bool toLeft = enemy.transform.position.x > _field.transform.position.x + _field.CellSize * _field.Cols * 0.5f;
        Vector3 direction = toLeft ? Vector3.left : Vector3.right;
        (enemy as Jumper).Init(direction, _field.PlayerRect.min.y, _field.PlayerRect.max.y);
    }

    protected override void OnEnemyDestroyed(Enemy enemy)
    {
        Sounds.main.PlayJumperDestroy(enemy.transform.position);
        Effects.main.PlayEffect(enemy.transform.position, ParticleEffectName.PapuanDestroy);
    }

    protected override void OnEnemyKilled(DamageReceiver enemy) { }
}
