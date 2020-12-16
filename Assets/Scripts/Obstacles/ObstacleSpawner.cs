using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameField _field;
    [SerializeField] private Cactus[] _cactusPrefabs;
    [SerializeField] private Stone[] _stonePrefabs;
    [SerializeField] private LayerMask _groundMask;

    [Header("Dry Trees")]
    [SerializeField] private int _treesCount;
    [SerializeField] private int _treeMinRow;
    [SerializeField] private int _treeMaxRow;
    [SerializeField] private Tree[] _treePrefabs;

    [Header("Green Trees")]
    [SerializeField] private int _greenTreesCount;
    [SerializeField] private int _greenTreeMinRow;
    [SerializeField] private int _greenTreeMaxRow;
    [SerializeField] private Tree[] _greenTreePrefabs;

    private Dictionary<int, Dictionary<int, Obstacle>> _obstacles = new Dictionary<int, Dictionary<int, Obstacle>>();

    private List<Obstacle> _obstaclesFlatList
    {
        get
        {
            List<Obstacle> result = new List<Obstacle>();
            foreach (var row in _obstacles)
                foreach (var keyValue in row.Value)
                    result.Add(keyValue.Value);

            return result;
        }
    }

    public UnityAction<Vector3> CactusKilled;

    public void SpawnRandomCactuses()
    {
        SpawnCactuses(_field.CellsCount / 5, _field.MinRow + _field.PlayerRows, _field.MinRow + _field.Rows * 2, -_field.Cols, _field.Cols * 2);
    }

    public void SpawnTrees()
    {
        SpawnTrees(_treesCount, _treeMinRow, _treeMaxRow, _treePrefabs);
        SpawnTrees(_greenTreesCount, _greenTreeMinRow, _greenTreeMaxRow, _greenTreePrefabs);
    }

    public void RestoreCactuses() => StartCoroutine(RestoreCactuses(1, 0.1f));

    public void GetAllowedDirections(int row, int col, out bool left, out bool right)
    {
        left = col > 0 && IsCellEmpty(row, col - 1);
        right = col < _field.Cols && IsCellEmpty(row, col);
    }

    private bool GetRandomEmptyPoint(out int row, out int col)
    {
        row = 0;
        col = 0;
        List<Vector2> emptyCells = GetEmptyCells(_field.MinRow, _field.MinRow + _field.Rows, 0, _field.Cols);
        if (emptyCells.Count > 0)
        {
            Vector2 cell = emptyCells[Random.Range(0, emptyCells.Count)];
            row = (int)cell.x;
            col = (int)cell.y;
            return true;
        }
        else
            return false;
    }

    private Obstacle GetObstacle(int row, int col)
    {
        if (_obstacles.ContainsKey(row) && _obstacles[row].ContainsKey(col))
            return _obstacles[row][col];

        return null;
    }

    private void SetObstacle(int row, int col, Obstacle value)
    {
        if (_obstacles.ContainsKey(row) == false)
            _obstacles[row] = new Dictionary<int, Obstacle>();

        _obstacles[row][col] = value;
    }
    private void RemoveObstacle(Obstacle value)
    {
        int row = 0;
        int col = 0;
        foreach (var obstaclesRow in _obstacles)
            foreach (var obstaclesCol in obstaclesRow.Value)
                if (obstaclesCol.Value == value)
                {
                    row = obstaclesRow.Key;
                    col = obstaclesCol.Key;
                    break;
                }

        if (_obstacles.ContainsKey(row) && _obstacles[row].ContainsKey(col))
            _obstacles[row].Remove(col);
    }

    public void SpawnCactusNearby(Vector3 position)
    {
        if (_field.TryGetCoordinates(position, out int row, out int col) && GetObstacle(row, col) == null)
            SpawnCactus(row, col);
    }

    private void SpawnCactuses(int count, int minRow, int maxRow, int minCol, int maxCol)
    {
        List<Vector2> emptyCells = GetEmptyCells(minRow, maxRow, minCol, maxCol);
        for (int i = 0; i < count; i++)
        {
            if (emptyCells.Count == 0)
                return;

            Vector2 cell = emptyCells[Random.Range(0, emptyCells.Count)];
            SpawnCactus((int)cell.x, (int)cell.y);
            emptyCells.Remove(cell);
        }
    }

    private void SpawnTrees(int count, int minRow, int maxRow, Tree[] prefabs)
    {
        List<Vector2> emptyCells = GetEmptyCells(minRow, maxRow, -_field.Cols/2, (int)(_field.Cols * 1.5));
        for (int i = 0; i < count; i++)
        {
            if (emptyCells.Count == 0)
                return;

            Vector2 cell = emptyCells[Random.Range(0, emptyCells.Count)];
            SpawnTree((int)cell.x, (int)cell.y, prefabs[Random.Range(0, prefabs.Length)]);
            emptyCells.Remove(cell);
        }
    }

    private List<Vector2> GetEmptyCells(int minRow, int maxRow, int minCol, int maxCol)
    {
        List<Vector2> emptyCells = new List<Vector2>();
        for (int i = minRow; i < maxRow; i++)
            for (int j = minCol; j < maxCol; j++)
                if (GetObstacle(i, j) == null)
                    emptyCells.Add(new Vector2(i, j));

        return emptyCells;
    }

    private void DestroyCactus(int row, int col)
    {
        Obstacle obstacle = GetObstacle(row, col);
        if (obstacle != null && obstacle is Cactus)
        {
            Cactus cactus = obstacle as Cactus;
            _obstacles[row].Remove(col);
            Destroy(cactus.gameObject);
            ShowSpawnEffect(row, col);
        }
    }

    private void ShowSpawnEffect(int row, int col)
    {
        Effects.main.PlayEffect(GetObstaclePosition(row, col), ParticleEffectName.CactusDestroy);
    }

    private IEnumerator RestoreCactuses(float maxDuration, float maxDelay)
    {

        List<Cactus> damagedCactuses = _obstaclesFlatList.Where(obstacle => obstacle.IsDamaged && obstacle is Cactus).Select(obstacle => obstacle as Cactus).ToList();
        float delay = Mathf.Min(maxDuration / damagedCactuses.Count, maxDelay);
        while (damagedCactuses.Count > 0)
        {
            Cactus cactus = damagedCactuses[Random.Range(0, damagedCactuses.Count)];
            cactus.Recover();
            damagedCactuses.Remove(cactus);

            Effects.main.PlayEffect(cactus.transform.position, ParticleEffectName.CactusDestroy);
            Sounds.main.PlayDestroySound(cactus.transform.position);

            yield return new WaitForSeconds(delay);
        }
    }

    public void RemoveCactuses(int minRow, int maxRow)
    {
        List<Cactus> cactuses = new List<Cactus>();
        foreach (var obstaclesRow in _obstacles)
            if (obstaclesRow.Key >= minRow && obstaclesRow.Key <= maxRow)
                foreach (var obstaclesCol in obstaclesRow.Value)
                    if (obstaclesCol.Value is Cactus)
                        cactuses.Add(obstaclesCol.Value as Cactus);

        StartCoroutine(RemoveCactuses(cactuses));
    }

    private IEnumerator RemoveCactuses(List<Cactus> cactuses)
    {
        float maxDuration = 1f;
        float delay = Mathf.Max(maxDuration / cactuses.Count, 0.1f);
        int cactusesPerStep = (int)(cactuses.Count / (maxDuration / delay));
        while (cactuses.Count > 0)
        {
            for (int i = 0; i < cactusesPerStep && cactuses.Count > 0; i++)
            {
                Cactus cactus = cactuses[Random.Range(0, cactuses.Count)];
                cactuses.Remove(cactus);
                cactus.Destroy();
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void SpawnCactus(int row, int col)
    {
        Vector3 position = GetObstaclePosition(row, col);
        position = _field.GetPositionOnTerrain(position);

        Cactus cactus = Instantiate(_cactusPrefabs[Random.Range(0, _cactusPrefabs.Length)], position, Quaternion.identity);
        cactus.Killed += OnCactusKilled;
        cactus.Destroyed += OnCactusDestroyed;
        cactus.Disappeared += OnObstacleDisappeared;

        SetObstacle(row, col, cactus);
        ShowSpawnEffect(row, col);
    }

    private void SpawnTree(int row, int col, Tree prefab)
    {
        Vector3 position = GetObstaclePosition(row, col);
        position = _field.GetPositionOnTerrain(position);

        Tree tree = Instantiate(prefab, position, Quaternion.Euler(new Vector3(-90, Random.Range(0, 360), 0)));
        tree.Destroyed += OnTreeDestroyed;
        tree.Disappeared += OnObstacleDisappeared;

        SetObstacle(row, col, tree);
    }

    public void SpawnObstacle(Obstacle obstaclePrefab, int row, int col, int sizeX, int sizeY)
    {
        Vector3 position = GetObstaclePosition(row, col);
        Obstacle obstacle = Instantiate(obstaclePrefab);
        obstacle.transform.position = position;

        obstacle.Disappeared += OnObstacleDisappeared;
        SetObstacle(row, col, obstacle);
    }

    private Vector3 GetObstaclePosition(int row, int col)
    {
        return _field.GetPosition(row, col) + new Vector3(_field.CellSize / 2, 0, 0);
    }

    private void OnCactusKilled(DamageReceiver cactus)
    {
        CactusKilled?.Invoke(cactus.transform.position);
    }

    private void OnObstacleDisappeared(DamageReceiver obstacle)
    {
        RemoveObstacle(obstacle as Obstacle);
    }

    private void OnCactusDestroyed(DamageReceiver cactus)
    {
        Sounds.main.PlayDestroySound(cactus.transform.position);
        Effects.main.PlayEffect(cactus.transform.position, ParticleEffectName.CactusDestroy);
    }

    private void OnTreeDestroyed(DamageReceiver tree)
    {
        Sounds.main.PlayRollerDestroy(tree.transform.position);
        Effects.main.PlayEffect(tree.transform.position, ParticleEffectName.RollerDestroy);
    }

    private bool IsCellEmpty(int row, int col) => GetObstacle(row, col) == null;

    public void SpawnStone()
    {
        if (GetRandomEmptyPoint(out int row, out int col))
            SpawnObstacle(_stonePrefabs[Random.Range(0, _stonePrefabs.Length)], row, col, 1, 1);
    }
}
