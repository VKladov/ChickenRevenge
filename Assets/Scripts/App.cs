using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
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
        ClosePauseScreen();
        CloseGameOverScreen();

        if (GameData.FirstPlay)
        {
            GameData.FirstPlay = false;
            OpenMainMenu();
            CloseGameUI();
            SwitchInputToMenu();
        }
        else
        {
            CloseMainMenu();
            OpenGameUI();
            _game.StartGame();
            SwitchInputToGame();
        }
    }

    // Events handling

    private void OnPressStart()
    {
        CloseMainMenu();
        OpenGameUI();
        _game.StartGame();
        SwitchInputToGame();
    }

    private void OnPressRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Time.timeScale = 1;
    }

    private void OnPressPause()
    {
        SwitchInputToMenu();
        CloseGameUI();
        OpenPauseScreen();

        Time.timeScale = 0;
    }

    private void OnPressResume()
    {
        SwitchInputToGame();
        OpenPauseScreen();
        _pauseScreen.gameObject.SetActive(false);
        OpenGameUI();

        Time.timeScale = 1;
    }

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

        SwitchInputToMenu();
        ShowGameOverScreen("Поражение", subtitle);
        CloseGameUI();
    }

    private void OnPlayerWon()
    {
        SwitchInputToMenu();
        CloseGameUI();
        ShowGameOverScreen("Победа!", "");
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

    private void OnPressExit() => Application.Quit();

    // Menus control

    private void OpenMainMenu() => _mainMenu.gameObject.SetActive(true);

    private void CloseMainMenu() => _mainMenu.gameObject.SetActive(false);

    private void OpenGameUI() => _gameUI.gameObject.SetActive(true);

    private void CloseGameUI() => _gameUI.gameObject.SetActive(false);

    private void OpenPauseScreen() => _pauseScreen.gameObject.SetActive(true);

    private void ClosePauseScreen() => _pauseScreen.gameObject.SetActive(false);

    private void CloseGameOverScreen() => _gameOverScreen.gameObject.SetActive(false);

    private void ShowGameOverScreen(string title, string subtitle)
    {
        _gameOverScreen.TitleText = title;
        _gameOverScreen.SubtitleText = subtitle;
        _gameOverScreen.gameObject.SetActive(true);
    }
}
