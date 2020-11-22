using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PapuanSpawner : MonoBehaviour
{
    [SerializeField] private GameField _field;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private Papuan _papuanPrefab;

    private List<Papuan> _papuans = new List<Papuan>();

    public UnityAction AllPapuansDied;
    public UnityAction<int> PapuansCountChanged;
    public UnityAction<Papuan> PapuanKilled;
    public int MinRow => _field.MinRow;
    public int PlayerRows => _field.PlayerRows;

    public void SendAllPapuansDown()
    {
        foreach (Papuan papuan in _papuans)
            papuan.RunDown(_field.PlayerRect.min.y);
    }

    public void SpawnGroup(int col, int count)
    {
        float z = _field.Rect.max.y;
        float distance = 0.5f;
        Papuan _prevPapuan = null;
        for (int i = 0; i < count; i++)
        {
            Vector3 papuanPosition = new Vector3(col * _field.CellSize, 0, z + distance * i);
            papuanPosition = _field.GetPositionOnTerrain(papuanPosition);
            Papuan papuan = Instantiate(_papuanPrefab, papuanPosition, Quaternion.LookRotation(Vector3.back));
            papuan.Init(this, _field.MinRow + _field.Rows, col, _prevPapuan);

            papuan.Destroyed += OnPapuanDestroyed;
            papuan.Killed += OnPapuanKilled;
            papuan.Disappeared += OnPapuanDisappeared;

            _papuans.Add(papuan);
            _prevPapuan = papuan;
        }

        PapuansCountChanged?.Invoke(_papuans.Count);
    }

    public void GetAllowedDirections(int row, int col, out bool left, out bool right)
    {
        _obstacleSpawner.GetAllowedDirections(row, col, out left, out right);
    }

    public Vector3 GetPosition(int row, int col) => _field.GetPosition(row, col);

    private void OnPapuanKilled(DamageReceiver papuan)
    {
        PapuanKilled?.Invoke(papuan as Papuan);
    }

    private void OnPapuanDestroyed(DamageReceiver papuan)
    {
        Sounds.main.PlayPapuanHit(papuan.transform.position);
        Effects.main.PlayEffect(papuan.transform.position, ParticleEffectName.PapuanDestroy);
    }

    private void OnPapuanDisappeared(DamageReceiver papuan)
    {
        _papuans.Remove(papuan as Papuan);
        PapuansCountChanged?.Invoke(_papuans.Count);

        if (_papuans.Count == 0)
            AllPapuansDied?.Invoke();
    }
}
