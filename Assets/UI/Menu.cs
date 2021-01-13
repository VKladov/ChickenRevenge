using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameLoop _game;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseScreen _pauseScreen;
    [SerializeField] private GameOverScreen _gameOverScreen;
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private MenuInput _menuInput;

    private void OnEnable()
    {
        _game.PlayerLost += OnPlayerLost;
        _game.PlayerWon += OnPlayerWon;
        _game.PausePressed += OnPressPause;
        _mainMenu.StartPressed += OnPressStart;
        _mainMenu.ExitPressed += OnPressExit;
        _pauseScreen.ResumePressed += OnPressResume;
        _pauseScreen.RestartPressed += OnPressRestart;
        _pauseScreen.ExitPressed += OnPressExit;
        _gameOverScreen.RestartPressed += OnPressRestart;
        _gameOverScreen.ExitPressed += OnPressExit;
    }

    private void OnDisable()
    {
        _game.PlayerLost -= OnPlayerLost;
        _game.PlayerWon -= OnPlayerWon;
        _game.PausePressed -= OnPressPause;
        _mainMenu.StartPressed -= OnPressStart;
        _mainMenu.ExitPressed -= OnPressExit;
        _pauseScreen.ResumePressed -= OnPressResume;
        _pauseScreen.RestartPressed -= OnPressRestart;
        _pauseScreen.ExitPressed -= OnPressExit;
        _gameOverScreen.RestartPressed -= OnPressRestart;
        _gameOverScreen.ExitPressed -= OnPressExit;
    }

    private void Start()
    {
        if (GameData.FirstPlay)
        {
            GameData.FirstPlay = false;
            _mainMenu.gameObject.SetActive(true);
            _pauseScreen.gameObject.SetActive(false);
            _gameOverScreen.gameObject.SetActive(false);
            _gameUI.gameObject.SetActive(false);
            SwitchInputToMenu();
        }
        else
        {
            Time.timeScale = 1;
            _mainMenu.gameObject.SetActive(false);
            _pauseScreen.gameObject.SetActive(false);
            _gameOverScreen.gameObject.SetActive(false);
            _gameUI.gameObject.SetActive(true);
            _game.StartGame();
            SwitchInputToGame();
        }
    }

    private void Update()
    {
        if (_pauseScreen.isActiveAndEnabled && _menuInput.WasBackPressed)
            OnPressResume();
    }

    private void OnPressStart()
    {
        _mainMenu.gameObject.SetActive(false);
        _gameUI.gameObject.SetActive(true);
        _game.StartGame();
        SwitchInputToGame();
    }

    private void OnPressRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnPressPause()
    {
        SwitchInputToMenu();
        Time.timeScale = 0;
        _gameUI.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(true);
    }

    private void OnPressResume()
    {
        Time.timeScale = 1;
        _pauseScreen.gameObject.SetActive(false);
        _gameUI.gameObject.SetActive(true);
        SwitchInputToGame();
    }

    private void OnPressExit() => Application.Quit();

    private void OnPlayerLost(LostReason reason)
    {
        string subtitle = "";
        switch (reason)
        {
            case LostReason.GateIsBroken:
                subtitle = "Ворота сломаны";
                break;
            case LostReason.PlayerIsDead:
                subtitle = "Вы погибли";
                break;
        }
        ShowGameOverScreen("Поражение", subtitle);
    }

    private void OnPlayerWon()
    {
        ShowGameOverScreen("Победа!", "");
    }

    private void ShowGameOverScreen(string title, string subtitle)
    {
        SwitchInputToMenu();
        _gameOverScreen.TitleText = title;
        _gameOverScreen.SubtitleText = subtitle;
        _gameOverScreen.gameObject.SetActive(true);
        _gameUI.gameObject.SetActive(false);
    }

    private void SwitchInputToMenu()
    {
        _game.DisableControl();
        _menuInput.enabled = true;
    }

    private void SwitchInputToGame()
    {
        _game.EnableControl();
        _menuInput.enabled = false;
    }
}
