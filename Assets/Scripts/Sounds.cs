using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioSource _sourcePrefab;
    [SerializeField] private int _capacity;
    [SerializeField] private AudioClip[] _hitSounds;
    [SerializeField] private AudioClip[] _destroySounds;
    [SerializeField] private AudioClip[] _papuanHit;
    [SerializeField] private AudioClip[] _rollerDestroy;
    [SerializeField] private AudioClip[] _softHit;
    [SerializeField] private AudioClip[] _womanDestroy;
    [SerializeField] private AudioClip[] _coinSpawn;
    [SerializeField] private AudioClip[] _coinGone;
    [SerializeField] private AudioClip[] _explosion;
    [SerializeField] private AudioClip[] _woodHit;
    [SerializeField] private AudioClip[] _birdAppear;
    [SerializeField] private AudioClip[] _birdAttack;
    [SerializeField] private AudioClip[] _birdDestroy;
    [SerializeField] private AudioClip[] _bombFalling;
    [SerializeField] private AudioClip[] _jumperJump;
    [SerializeField] private AudioClip[] _jumperDestroy;

    private List<AudioSource> _buffer = new List<AudioSource>();

    public static Sounds main = null;

    void Start()
    {
        if (main == null)
            main = this;
        else if (main == this)
            Destroy(gameObject);
    }

    public void PlayHitSound(Vector3 position) => PlayRandomSound(position, _hitSounds);

    public void PlayDestroySound(Vector3 position) => PlayRandomSound(position, _destroySounds);

    public void PlayPapuanHit(Vector3 position) => PlayRandomSound(position, _papuanHit);

    public void PlayRollerDestroy(Vector3 position) => PlayRandomSound(position, _rollerDestroy);

    public void PlaySoftHit(Vector3 position) => PlayRandomSound(position, _softHit);

    public void PlayWomanDestroy(Vector3 position) => PlayRandomSound(position, _womanDestroy);

    public void PlayCoinSpawn(Vector3 position) => PlayRandomSound(position, _coinSpawn);

    public void PlayCoinGone(Vector3 position) => PlayRandomSound(position, _coinGone);

    public void PlayExplosion(Vector3 position) => PlayRandomSound(position, _explosion);

    public void PlayWoodHit(Vector3 position) => PlayRandomSound(position, _woodHit);

    public void PlayBirdAppear(Vector3 position) => PlayRandomSound(position, _birdAppear);

    public void PlayBirdAttack(Vector3 position) => PlayRandomSound(position, _birdAttack);

    public void PlayBirdDestroy(Vector3 position) => PlayRandomSound(position, _birdDestroy);

    public void PlayBombFalling(Vector3 position) => PlayRandomSound(position, _bombFalling);

    public void PlayJumperJump(Vector3 position) => PlayRandomSound(position, _jumperJump);

    public void PlayJumperDestroy(Vector3 position) => PlayRandomSound(position, _jumperDestroy);

    private void PlayRandomSound(Vector3 position, AudioClip[] clips) => PlaySound(position, clips[Random.Range(0, clips.Length)]);

    private void PlaySound(Vector3 position, AudioClip clip)
    {
        AudioSource source = GetSource();
        if (source != null)
        {
            source.transform.position = position;
            source.PlayOneShot(clip);
        }
    }

    private AudioSource GetSource()
    {
        AudioSource source = _buffer.Find(instance => instance.isPlaying == false);
        if (source == null && _buffer.Count < _capacity)
        {
            AudioSource newSource = Instantiate(_sourcePrefab);
            _buffer.Add(newSource);
            return newSource;
        }

        return source;
    }
}
