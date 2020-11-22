using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RollerSpawner : MonoBehaviour
{
    [SerializeField] private Roller _rollerPrefab;
    [SerializeField] private GameField _field;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _target;
    [SerializeField] private float _offset;

    private List<Roller> _rollers = new List<Roller>();
    private float _spawnDelay = 1;
    private float _spawnDelaySpread => _spawnDelay * 0.8f;
    private Coroutine _spawnWork;
    private bool _hasTarget = false;

    public List<Vector3> RollersInCameraPositions => _rollers.Where(roller => roller.VisibleInCamera == false).Select(roller => roller.PositionInCamera).ToList();

    public void SetSpawnDelay(float delay) => _spawnDelay = delay;

    public void DestroyAll()
    {
        while (_rollers.Count > 0)
            _rollers[0].Destroy();

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

    private void SpawnRoller()
    {
        bool toLeft = Random.Range(0.0f, 1.0f) < 0.5f;
        Vector3 position = GetRandomSpawnPosition(toLeft);
        if (IsSeeableByCamera(position))
        {
            toLeft = !toLeft;
            position = GetRandomSpawnPosition(toLeft);
        }

        Vector3 direction = toLeft ? Vector3.left : Vector3.right;
        Roller roller = Instantiate(_rollerPrefab, position, Quaternion.LookRotation(direction, Vector3.up));
        roller.Init(direction, _hasTarget ? _target : null);

        roller.Destroyed += OnRollerDestroyed;

        _rollers.Add(roller);
    }

    private IEnumerator Spawn()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(Random.Range(_spawnDelay - _spawnDelaySpread, _spawnDelay + _spawnDelaySpread));
            SpawnRoller();
        }
    }

    private bool IsSeeableByCamera(Vector3 point)
    {
        Vector3 viewportPoint = _camera.WorldToViewportPoint(point);
        return viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
            viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
            viewportPoint.z > 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SpawnRoller();
    }

    private void FixedUpdate()
    {
        _rollers.Where(roller => (_field.Rect.min.x - _offset * 1.1 > roller.transform.position.x || roller.transform.position.x > _field.Rect.max.x + _offset * 1.1) && roller.VisibleInCamera == false)
            .ToList()
            .ForEach(roller => {
                _rollers.Remove(roller);
                Destroy(roller.gameObject);
            });
    }

    private Vector3 GetRandomSpawnPosition(bool toLeft)
    {
        int col = toLeft ? _field.Cols - 1 : 0;
        int row = Random.Range(_field.MinRow, _field.MinRow + _field.PlayerRows);
        return  _field.GetPosition(row, col) + (toLeft ? (Vector3.left * -_offset) : (Vector3.right * -_offset));
    }

    private void OnRollerDestroyed(DamageReceiver roller)
    {
        _rollers.Remove(roller as Roller);
        Sounds.main.PlayRollerDestroy(roller.transform.position);
        Effects.main.PlayEffect(roller.transform.position, ParticleEffectName.RollerDestroy);
    }
}