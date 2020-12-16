using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : EnemySpawner
{
    [SerializeField] private float _offset;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnHeightSpread;

    protected override void InitItem(Enemy enemy, GameObject target)
    {
        bool toLeft = enemy.transform.position.x > _field.transform.position.x + _field.CellSize * _field.Cols * 0.5f;
        Vector3 direction = toLeft ? Vector3.left : Vector3.right;
        enemy.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        Sounds.main.PlayBirdAppear(enemy.transform.position);
    }

    protected override bool CanDestroyItem(Enemy enemy)
    {
        return (enemy.transform.position.x > _field.Rect.max.x + _offset * 1.1 || enemy.transform.position.x < _field.Rect.min.x - _offset * 1.1) &&
            IsVisible(enemy.transform.position) == false;
    }

    protected override Vector3 GetRandomSpawnPosition()
    {
        bool toLeft = Random.Range(0, 1) > 0.5f;
        int col = toLeft ? _field.Cols - 1 : 0;
        int row = Random.Range(_field.MinRow, _field.MinRow + _field.PlayerRows);
        float height = Random.Range(_spawnHeight - _spawnHeightSpread / 2, _spawnHeight + _spawnHeightSpread / 2);
        return _field.GetPosition(row, col) + (toLeft ? (Vector3.left * -_offset) : (Vector3.right * -_offset)) + Vector3.up * height;
    }

    protected override void OnEnemyKilled(DamageReceiver enemy) { }

    protected override void OnEnemyDestroyed(Enemy enemy)
    {
        Sounds.main.PlayBirdDestroy(enemy.transform.position);
        Effects.main.PlayEffect(enemy.transform.position, ParticleEffectName.CactusDestroy);
    }
}
