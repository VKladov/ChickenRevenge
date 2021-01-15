using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PapuanSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Papuan _papuanPrefab;
    [SerializeField] private Gate _gate;

    private int _currentSpawnIndex = 0;
    private List<Papuan> _papuans = new List<Papuan>();

    public UnityAction AllPapuansDied;
    public UnityAction<int> PapuansCountChanged;
    public UnityAction<Papuan> PapuanKilled;

    public void Spawn()
    {
        Papuan papuan = Instantiate(_papuanPrefab, _spawnPoints[_currentSpawnIndex].position, Quaternion.LookRotation(Vector3.back));
        _currentSpawnIndex = (_currentSpawnIndex + 1) % _spawnPoints.Length;

        papuan.Init(_gate, FindObjectOfType<Player>());

        papuan.Destroyed += OnPapuanDestroyed;
        papuan.Killed += OnPapuanKilled;
        papuan.Disappeared += OnPapuanDisappeared;

        _papuans.Add(papuan);

        CharacterGroups.main.AddItemToGroupB(papuan);

        PapuansCountChanged?.Invoke(_papuans.Count);
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Spawn();
    }
}
