using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WomanSpawner : MonoBehaviour
{
    [SerializeField] private Woman _womanPrefab;
    [SerializeField] private GameField _field;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _offset = 5;

    private List<Woman> _women = new List<Woman>();
    private float _spawnDelay = 1;
    private float _spawnDelaySpread => _spawnDelay * 0.8f;
    private Coroutine _spawnWork;
    private bool _hasTarget = true;

    public void SetSpawnDelay(float delay) => _spawnDelay = delay;

    public void DestroyAll()
    {
        while (_women.Count > 0)
            _women[0].Destroy();

    }

    public void StartSpawn(bool hasTarget)
    {
        _hasTarget = hasTarget;
        if (_spawnWork != null)
            StopCoroutine(_spawnWork);

        _spawnWork = StartCoroutine(Spawn());
    }

    public void StopSpawn()
    {
        if (_spawnWork != null)
            StopCoroutine(_spawnWork);
    }

    private void SpawnWoman()
    {
        Vector3 position = new Vector3(Random.Range(_field.Rect.min.x, _field.Rect.max.x), 0, _field.Rect.max.y + _offset);
        position = _field.GetPositionOnTerrain(position);
        Woman woman = Instantiate(_womanPrefab, position, Quaternion.LookRotation(Vector3.back, Vector3.up));
        if (_hasTarget)
            woman.SetTarget(_target);

        woman.Destroyed += OnWomanDestroyed;
        woman.Killed += OnWomanKilled;
        _women.Add(woman);
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(_spawnDelay - _spawnDelaySpread, _spawnDelay + _spawnDelaySpread));
            SpawnWoman();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            SpawnWoman();
    }

    private void FixedUpdate()
    {
        _women.Where(woman => _field.Rect.min.y > woman.transform.position.z)
            .ToList()
            .ForEach(woman => {
                _women.Remove(woman);
                Destroy(woman.gameObject);
            });
    }

    private void OnWomanDestroyed(DamageReceiver woman)
    {
        _women.Remove(woman as Woman);
        Effects.main.PlayEffect(woman.transform.position, ParticleEffectName.PapuanDestroy);
    }

    private void OnWomanKilled(DamageReceiver woman) => Sounds.main.PlayWomanDestroy(woman.transform.position);
}
