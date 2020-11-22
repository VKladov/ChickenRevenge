using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameField : MonoBehaviour
{
    [SerializeField] private float _cellSize = 1;
    [SerializeField] private int _cols = 10;
    [SerializeField] private int _rows = 10;
    [SerializeField] private int _rowsPerLevel = 5;
    [SerializeField] private int _playerRows = 5;
    [SerializeField] private Bonus[] _bonusPrefabs;
    [SerializeField] private float _coinSpawnChance = 0.1f;
    [SerializeField] private LayerMask _groundMask;

    private int _currentLevel = 0;

    public int CellsCount => _cols * _rows;
    public int PlayerRows => _playerRows;
    public int MinRow => _currentLevel * _rowsPerLevel;
    public int Cols => _cols;
    public int Rows => _rows;
    public float CellSize => _cellSize;
    public Rect Rect => new Rect(
        transform.position + new Vector3(0, MinRow * _cellSize, 0),
        new Vector2((_cols - 1) * _cellSize, (_rows - 1) * _cellSize)
        );
    public Rect PlayerRect => new Rect(
        transform.position + new Vector3(0, MinRow * _cellSize, 0),
        new Vector2(_cols * _cellSize, _playerRows * _cellSize)
        );

    private bool TryGetCoordinates(int index, out int row, out int col)
    {
        col = 0;
        row = 0;
        if (index < 0 || index > _cols * _rows - 1)
        {
            return false;
        }
        else
        {
            col = index % _cols;
            row = index / _cols;

            return true;
        }
    }

    public int GetRandomIndex() => Random.Range(0, CellsCount);

    public Vector3 GetPosition(int index)
    {
        if (TryGetCoordinates(index, out int row, out int col))
            return transform.position + new Vector3(col * _cellSize, 0, row * _cellSize);
        else
            return transform.position;
    }

    public bool TryGetCoordinates(Vector3 position, out int row, out int col)
    {
        Vector3 localPosition = position - transform.position;
        row = (int)(localPosition.z / CellSize);
        col = (int)(localPosition.x / CellSize);

        return true;
    }

    public Vector3 GetPosition(int row, int col)
    {
        Vector3 position = transform.position + new Vector3(col * _cellSize, 0, row * _cellSize);
        position = GetPositionOnTerrain(position);
        return position;
    }

    public void SetLevel(int level)
    {
        _currentLevel = level;
    }

    public Vector3 GetPositionOnTerrain(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up * 3, Vector3.down, out RaycastHit hit, 10, _groundMask))
            position.y = hit.point.y;

        return position;
    }
}
