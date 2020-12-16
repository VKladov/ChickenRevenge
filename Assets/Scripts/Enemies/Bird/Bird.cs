using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : Enemy
{
    [SerializeField] private float _speed;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _attackDelay;

    private void FixedUpdate()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void Start()
    {
        StartCoroutine(StartAttack(_attackDelay));
    }

    private IEnumerator StartAttack(float delay)
    {
        while (enabled)
        {
            yield return new WaitForSeconds(delay);
            Attack();
        }
    }

    private void Attack()
    {
        Bomb bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity);
        bomb.Init(transform.forward * _speed);
        Sounds.main.PlayBirdAttack(transform.position);
        Sounds.main.PlayBombFalling(transform.position);
    }
}
