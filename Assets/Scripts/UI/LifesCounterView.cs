using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifesCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.HealthChanged += OnLivesChanged;
    }

    private void Start()
    {
        OnLivesChanged(_player.Health);
    }

    private void OnDisable()
    {
        _player.HealthChanged -= OnLivesChanged;
    }

    public void OnLivesChanged(int count)
    {
        _text.text = "x " + count;
    }
}
