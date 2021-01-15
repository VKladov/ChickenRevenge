using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private MenuInput _input;

    public UnityAction ResumePressed;
    public UnityAction RestartPressed;
    public UnityAction ExitPressed;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(OnResumeButtonClicked);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    private void Update()
    {
        if (_input.WasBackPressed)
            ResumePressed?.Invoke();
    }

    private void OnResumeButtonClicked() => ResumePressed?.Invoke();

    private void OnRestartButtonClicked() => RestartPressed?.Invoke();

    private void OnExitButtonClicked() => ExitPressed?.Invoke();
}
