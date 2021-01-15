using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GateHeathBar : MonoBehaviour
{
    [SerializeField] private Gate _gate;

    private int _maxHealth = 1;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        OnGateHealthChanged(_gate.Health);
    }

    private void OnEnable()
    {
        _maxHealth = _gate.MaxHealth;
        _gate.HealthChanged += OnGateHealthChanged;
    }

    private void OnDisable()
    {
        _gate.HealthChanged -= OnGateHealthChanged;
    }

    private void OnGateHealthChanged(int health)
    {
        _slider.value = (float)health / (float)_maxHealth;
    }
}
