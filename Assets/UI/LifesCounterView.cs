using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifesCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameLoop _game;

    private void OnEnable()
    {
        _game.LivesChanged += OnLivesChanged;
    }

    private void OnDisable()
    {
        _game.LivesChanged -= OnLivesChanged;
    }

    public void OnLivesChanged(int count)
    {
        _text.text = "x " + count;
    }
}
