using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] protected GameField _field;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _target;

    private List<Enemy> _enemies = new List<Enemy>();
    private float _spawnDelay = 1;
    private float _spawnDelaySpread => _spawnDelay * 0.8f;
    private Coroutine _spawnWork;
    private bool _hasTarget = false;

    public List<Vector3> ItemsInCameraPositions
    {
        get
        {
            List<Vector3> positions = new List<Vector3>();
            foreach (Enemy enemy in _enemies)
                if (IsVisible(enemy.transform.position, out Vector3 positionInCamera) == false)
                    positions.Add(positionInCamera);

            return positions;
        }
    }

    protected bool IsVisible(Vector3 position, out Vector3 inCameraPosition)
    {
        inCameraPosition = _camera.WorldToViewportPoint(position);
        return inCameraPosition.x >= 0 && inCameraPosition.x <= 1 &&
            inCameraPosition.y >= 0 && inCameraPosition.y <= 1 &&
            inCameraPosition.z > 0;
    }

    protected bool IsVisible(Vector3 position) => IsVisible(position, out Vector3 inCameraPosition);

    public void SetSpawnDelay(float delay) => _spawnDelay = delay;

    public void DestroyAll()
    {
        while (_enemies.Count > 0)
            _enemies[0].Destroy();
    }

    public void StartSpawn(bool hasTarget)
    {
        _hasTarget = hasTarget;
        if (_spawnWork != null)
            StopCoroutine(_spawnWork);

        _spawnWork = StartCoroutine(SpawnLoop());
    }

    public void StopSpawn()
    {
        if (_spawnWork != null)
            StopCoroutine(_spawnWork);
    }

    public void Spawn()
    {
        Vector3 position = GetRandomSpawnPosition();
        int i = 0;
        while (IsVisible(position) && i < 10)
        {
            position = GetRandomSpawnPosition();
            i++;
        }

        Enemy enemy = Instantiate(_prefab, position, Quaternion.identity);
        InitItem(enemy, _hasTarget ? _target : null);
        _enemies.Add(enemy);

        enemy.Destroyed += OnEnemyDestroyed;
        enemy.Killed += OnEnemyKilled;
        enemy.Disappeared += OnEnemyDisappeared;
    }

    private IEnumerator SpawnLoop()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(_spawnDelay - _spawnDelaySpread, _spawnDelay + _spawnDelaySpread));
            Spawn();
        }
    }

    private void FixedUpdate()
    {
        _enemies.Where(item => CanDestroyItem(item))
            .ToList()
            .ForEach(item => {
                Destroy(item.gameObject);
            });
    }

    protected abstract void InitItem(Enemy enemy, GameObject target);

    protected abstract bool CanDestroyItem(Enemy enemy);

    protected abstract Vector3 GetRandomSpawnPosition();

    protected abstract void OnEnemyKilled(DamageReceiver enemy);

    protected abstract void OnEnemyDestroyed(Enemy enemy);

    private void OnEnemyDestroyed(DamageReceiver damageReceiver)
    {
        if (damageReceiver is Enemy)
        {
            Enemy enemy = damageReceiver as Enemy;
            OnEnemyDestroyed(enemy);
            _enemies.Remove(enemy);
        }
    }

    private void OnEnemyDisappeared(DamageReceiver enemy) => _enemies.Remove(enemy as Enemy);
}
