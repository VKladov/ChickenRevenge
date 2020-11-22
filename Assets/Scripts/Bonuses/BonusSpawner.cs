using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    [SerializeField] private GameField _field;
    [SerializeField] private Bonus[] _coinPrefabs;
    [SerializeField] private Bonus[] _diamondPrefabs;

    public void SpawnCoin(Vector3 position) => Spawn(position, _coinPrefabs[Random.Range(0, _coinPrefabs.Length)]);

    public void SpawnDiamond(Vector3 position) => Spawn(position, _diamondPrefabs[Random.Range(0, _diamondPrefabs.Length)]);

    private void Spawn(Vector3 position, Bonus prefab)
    {
        Sounds.main.PlayCoinSpawn(position);
        Bonus bonus = Instantiate(prefab, position, Quaternion.identity);
        bonus.SetJumpTarget(new Vector3(Random.Range(_field.PlayerRect.min.x, _field.PlayerRect.max.x), 0, Random.Range(_field.PlayerRect.min.y, _field.PlayerRect.max.y)));
        bonus.Killed += OnBonusKilled;
    }

    private void OnBonusKilled(DamageReceiver bonus) => Explode(bonus as Bonus);

    private void Explode(Bonus bonus)
    {
        float radius = bonus.ExplosionRadius;
        RaycastHit[] raycastHits = Physics.BoxCastAll(bonus.transform.position + Vector3.up * 0.5f, new Vector3(radius, radius, radius), Vector3.down, Quaternion.identity, 2);
        foreach (RaycastHit hit in raycastHits)
            if (hit.transform.TryGetComponent(out DamageReceiver destroyable))
                destroyable.TakeDamage(10);

        Sounds.main.PlayExplosion(bonus.transform.position);
        Effects.main.PlayEffect(bonus.transform.position, ParticleEffectName.CoinDestroy);
    }
}
