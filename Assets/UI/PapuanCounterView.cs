using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PapuanCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameLoop _game;

    private void OnEnable()
    {
        _game.PapuasCountChanged += OnPapuasCountChanged;
    }

    private void OnDisable()
    {
        _game.PapuasCountChanged -= OnPapuasCountChanged;
    }

    public void OnPapuasCountChanged(int count)
    {
        _text.text = count + " x";
    }
}
