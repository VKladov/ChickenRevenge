using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _subtitle;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    public UnityAction ResumePressed;
    public UnityAction RestartPressed;
    public UnityAction ExitPressed;

    public string TitleText
    {
        set
        {
            _title.text = value;
        }
    }

    public string SubtitleText
    {
        set
        {
            _subtitle.text = value;
        }
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    private void OnRestartButtonClicked() => RestartPressed?.Invoke();

    private void OnExitButtonClicked() => ExitPressed?.Invoke();
}
