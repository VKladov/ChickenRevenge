using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameField _field;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private BonusSpawner _bonusSpawner;
    [SerializeField] private PapuanSpawner _papuanSpawner;
    [SerializeField] private RollerSpawner _rollerSpawner;
    [SerializeField] private LevelBuilder _levelBuilder;
    [SerializeField] private float _wavesDelay = 2f;
    [SerializeField] private int _lifes = 5;
    [SerializeField] private int _startWaveSize = 24;
    [SerializeField] private int _wavesPerLevel = 5;
    [SerializeField] private float _stanDuration = 3f;
    [SerializeField] private float _anyBonusChance = 0.1f;
    [SerializeField] private float _diamondChance = 0.2f;

    private int _currentWaveIndex = 0;
    private int _currentLevel = 0;

    public UnityAction<int> PapuasCountChanged;
    public UnityAction<int> LivesChanged;
    public UnityAction<int> WaveStarted;

    private void OnEnable()
    {
        _obstacleSpawner.CactusKilled += OnCactusKilled;
        _papuanSpawner.AllPapuansDied += AllPapuansDied;
        _papuanSpawner.PapuansCountChanged += OnPapuasCountChanged;
        _papuanSpawner.PapuanKilled += OnPapuanKilled;
        _player.MetPapuan += OnPlayerMetPapuan;
        _player.MetRoller += OnMetRoller;
        _player.MetWoman += OnMetWoman;
        _player.CaughtBonus += OnPlayerGetBonus;
    }

    private void OnDisable()
    {
        _obstacleSpawner.CactusKilled -= OnCactusKilled;
        _papuanSpawner.AllPapuansDied -= AllPapuansDied;
        _papuanSpawner.PapuansCountChanged -= OnPapuasCountChanged;
        _papuanSpawner.PapuanKilled -= OnPapuanKilled;
        _player.MetPapuan -= OnPlayerMetPapuan;
        _player.MetRoller -= OnMetRoller;
        _player.MetWoman -= OnMetWoman;
        _player.CaughtBonus -= OnPlayerGetBonus;
    }

    private void Start()
    {
        _field.SetLevel(0);
        _levelBuilder.SpawnObstacles(_obstacleSpawner);
        _levelBuilder.UpdatePlayerArea(_field);
        _obstacleSpawner.SpawnRandomCactuses();
        _obstacleSpawner.SpawnTrees();

        StartWave(0, false);
    }

    public void NextLevel()
    {
        Vector3 playerPositionInPlayerArea = new Vector3(_player.transform.position.x - _field.PlayerRect.min.x, 0, _player.transform.position.z - _field.PlayerRect.min.y);

        _currentLevel++;
        _field.SetLevel(_currentLevel);
        _levelBuilder.UpdatePlayerArea(_field);

        Vector3 position = new Vector3(_field.PlayerRect.min.x + playerPositionInPlayerArea.x, 0, _field.PlayerRect.min.y + playerPositionInPlayerArea.z);
        _player.GetComponent<CharacterMover>().SetTarget(_field.GetPositionOnTerrain(position));
    }

    private IEnumerator StartWave(int index, float delay)
    {
        if (index > 0 && index % _wavesPerLevel == 0)
            NextLevel();

        WaveStarted?.Invoke(index);
        _currentWaveIndex = index;
        yield return new WaitForSeconds(delay);

        List<int> emptyCols = new List<int>();
        for (int i = 0; i < _field.Cols; i++)
            emptyCols.Add(i);

        int count = _startWaveSize + index;
        int cols = (count - _startWaveSize) % _wavesPerLevel + 1;
        for (int i = 0; i < cols; i++)
        {
            int col = emptyCols[Random.Range(0, emptyCols.Count)];
            emptyCols.Remove(col);

            if (i == 0)
                _papuanSpawner.SpawnGroup(col, count - cols + 1);
            else
                _papuanSpawner.SpawnGroup(col, 1);
        }

        _rollerSpawner.SetSpawnDelay(10);
        _rollerSpawner.StartSpawn(index > 5);
    }

    private void StartWave(int index, bool delay)
    {
        if (delay)
            StartCoroutine(StartWave(index, _wavesDelay));
        else
            StartCoroutine(StartWave(index, 0));
    }

    private void AllPapuansDied()
    {
        FinishWave();
        if (_player.IsStanned)
            StartWave(_currentWaveIndex, true);
        else
            StartWave(_currentWaveIndex + 1, true);
    }

    private void FinishWave()
    {
        _rollerSpawner.DestroyAll();
        _rollerSpawner.StopSpawn();
        _obstacleSpawner.RestoreCactuses();
    }

    private void OnPlayerMetPapuan(Papuan papuan)
    {
        if (_player.IsStanned)
            return;

        papuan.CatchPlayer(_player);

        Lose();

        if (_lifes > 0)
            StartCoroutine(ReleaseChickenAfter(_stanDuration, papuan));
        else
            FinishGame();
    }

    private void OnMetRoller()
    {
        if (_player.IsStanned)
            return;

        Lose();

        if (_lifes > 0)
            StartCoroutine(UnstanChickenAfter(_stanDuration));
        else
            FinishGame();
    }

    private void OnMetWoman()
    {
        OnMetRoller();
    }

    private void Lose()
    {
        _player.Stan();
        _papuanSpawner.SendAllPapuansDown();
        _lifes--;

        LivesChanged?.Invoke(_lifes);
    }

    private void FinishGame() { }

    private IEnumerator UnstanChickenAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        _player.FinishStan();
    }

    private IEnumerator ReleaseChickenAfter(float delay, Papuan papuan)
    {
        yield return new WaitForSeconds(delay);
        ReleaseChicken(papuan);
    }

    private void ReleaseChicken(Papuan papuan)
    {
        papuan.Destroy();
        _player.FinishStan();
    }

    private void OnPapuasCountChanged(int count)
    {
        PapuasCountChanged?.Invoke(count);
    }

    private void OnCactusKilled(Vector3 position)
    {
        if (Random.Range(0.0f, 1f) <= _anyBonusChance)
            if (Random.Range(0.0f, 1f) <= _diamondChance)
                _bonusSpawner.SpawnDiamond(position);
            else
                _bonusSpawner.SpawnCoin(position);
    }

    private void OnPapuanKilled(Papuan papuan) => _obstacleSpawner.SpawnCactusNearby(papuan.transform.position);

    private void OnPlayerGetBonus(Bonus bonus)
    {
        switch (bonus.BonusName)
        {
            case BonusName.CactusesCleaner:
                _obstacleSpawner.RemoveCactuses(_field.MinRow, _field.MinRow + _field.PlayerRows * 2);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            NextLevel();
    }
}
