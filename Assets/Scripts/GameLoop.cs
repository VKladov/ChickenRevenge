using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LostReason
{
    PlayerIsDead,
    GateIsBroken
}

public class GameLoop : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Gate _gate;
    [SerializeField] private PapuanSpawner _papuanSpawner;
    [SerializeField] private CameraChanger _cameraChanger;
    [SerializeField] private GameInput _input;
    [SerializeField] private float _wavesDelay = 2f;
    [SerializeField] private int _totalWavesCount = 5;
    [SerializeField] private int _startWaveSize = 10;
    [SerializeField] private int _addPapuansPerWave = 2;
    [SerializeField] private float _spawnDuration = 20;

    private int _currentWaveIndex = 0;
    private int _papuansToSpawnCount;
    private float _spawnDelay;

    public UnityAction<int> PapuansCountChanged;
    public UnityAction<int> LivesChanged;
    public UnityAction<int> WaveStarted;
    public UnityAction<LostReason> PlayerLost;
    public UnityAction PlayerWon;
    public UnityAction PausePressed;

    public void StartGame()
    {
        _cameraChanger.SwitchToPlayer();
        StartWave(0);
    }

    public void EnableControl()
    {
        _input.enabled = true;
        _player.EnableAim();
    }

    public void DisableControl()
    {
        _input.enabled = false;
        _player.DisableAim();
    }

    private void OnEnable()
    {
        _papuanSpawner.AllPapuansDied += OnAllPapuansDied;
        _papuanSpawner.PapuansCountChanged += OnPapuasCountChanged;
        _player.Destroyed += OnPlayerDestroyed;
        _gate.Destroyed += OnGateDestroyed;
    }

    private void OnDisable()
    {
        _papuanSpawner.AllPapuansDied -= OnAllPapuansDied;
        _papuanSpawner.PapuansCountChanged -= OnPapuasCountChanged;
        _player.Destroyed -= OnPlayerDestroyed;
        _gate.Destroyed -= OnGateDestroyed;
    }

    private void Update()
    {
        if (_input.WasPausePressed)
            PausePressed?.Invoke();
    }

    private void StartWave(int index)
    {
        _currentWaveIndex = index;
        _papuansToSpawnCount = _startWaveSize + _addPapuansPerWave * _currentWaveIndex;
        _spawnDelay = _spawnDuration / _papuansToSpawnCount;

        WaveStarted?.Invoke(index);

        PapuansCountChanged?.Invoke(_papuansToSpawnCount);
        StartCoroutine(SpawnPapuans());
    }

    private IEnumerator SpawnPapuans()
    {
        while (_papuansToSpawnCount > 0)
        {
            yield return new WaitForSeconds(_spawnDelay);
            _papuansToSpawnCount--;
            _papuanSpawner.Spawn();
        }
    }

    private IEnumerator DelayLost(LostReason reason)
    {
        yield return new WaitForSeconds(2);
        PlayerLost?.Invoke(reason);
    }

    // Events handling
    private void OnPlayerDestroyed(DamageReceiver player)
    {
        DisableControl();
        StartCoroutine(DelayLost(LostReason.PlayerIsDead));
    }

    private void OnGateDestroyed(DamageReceiver gate)
    {
        _cameraChanger.SwitchToGate();
        StartCoroutine(DelayLost(LostReason.GateIsBroken));
    }

    private void OnPapuasCountChanged(int count)
    {
        PapuansCountChanged?.Invoke(_papuansToSpawnCount + count);
    }

    private void OnAllPapuansDied()
    {
        if (_papuansToSpawnCount == 0)
            if (_currentWaveIndex < _totalWavesCount - 1)
                StartWave(_currentWaveIndex + 1);
            else
                PlayerWon?.Invoke();
    }
}
