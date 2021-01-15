using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ParticleEffectName
{
    CactusDestroy,
    PapuanDestroy,
    CoinDestroy,
    RollerDestroy
}

[System.Serializable]
public class ParticleEffectConfig
{
    public ParticleEffectName Name;
    public ParticleSystem Prefab;
}

public class Effects : MonoBehaviour
{
    [SerializeField] private int _maxCapacity;
    [SerializeField] private ParticleEffectConfig[] _configs;

    private Dictionary<ParticleEffectName, ParticleSystem> _prefabs = new Dictionary<ParticleEffectName, ParticleSystem>();
    private Dictionary<ParticleEffectName, List<ParticleSystem>> _buffers = new Dictionary<ParticleEffectName, List<ParticleSystem>>();

    public static Effects main = null;

    private void Awake()
    {
        foreach (ParticleEffectConfig config in _configs)
        {
            _prefabs[config.Name] = config.Prefab;
            _buffers[config.Name] = new List<ParticleSystem>();
        }
    }

    void Start()
    {
        if (main == null)
            main = this;
        else if (main == this)
            Destroy(gameObject);
    }

    private bool TryGetItem(ParticleEffectName name, out ParticleSystem particle)
    {
        particle = null;
        if (_buffers.ContainsKey(name) == false || _prefabs.ContainsKey(name) == false)
            return false;

        List<ParticleSystem> buffer = _buffers[name];
        foreach (ParticleSystem item in buffer)
        {
            if (item.isPlaying == false)
            {
                particle = item;
                return true;
            }
        }

        if (buffer.Count < _maxCapacity)
        {
            ParticleSystem newItem = Instantiate(_prefabs[name]);
            buffer.Add(newItem);
            particle = newItem;
            return true;
        }

        return false;
    }

    public void PlayEffect(Vector3 position, ParticleEffectName name)
    {
        if (TryGetItem(name, out ParticleSystem particle))
        {
            particle.transform.position = position;
            particle.Play();
        }
    }
}
