using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Playing,
    UpgradesMenu,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public GameState CurrentState { get; private set; }

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("Game Manager is NULL.");
            }
            return _instance;
        }
    }

    public GameObject MainMenuUI;
    public GameObject PlayerUI;
    public GameObject UpgradesMenuUI;
    public GameObject GameOverUI;

    public GameObject PlayerCharacter;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        // Set initial state to main menu
        // SetGameState(GameState.MainMenu);
        SetGameState(GameState.Playing); // change this later
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                // Display the main menu UI
                MainMenuUI.SetActive(true);
                PlayerCharacter.SetActive(false);
                PlayerUI.SetActive(false);
                UpgradesMenuUI.SetActive(false);
                GameOverUI.SetActive(false);
                break;
            case GameState.Playing:
                // Start the game
                MainMenuUI.SetActive(false);
                PlayerCharacter.SetActive(true);
                PlayerUI.SetActive(true);
                UpgradesMenuUI.SetActive(false);
                GameOverUI.SetActive(false);
                break;
            case GameState.UpgradesMenu:
                // Open upgrades menu UI
                MainMenuUI.SetActive(false);
                PlayerCharacter.SetActive(true);
                PlayerUI.SetActive(false);
                UpgradesMenuUI.SetActive(true);
                GameOverUI.SetActive(false);
                break;
            case GameState.GameOver:
                // Display the game over UI
                MainMenuUI.SetActive(false);
                PlayerCharacter.SetActive(true);
                PlayerUI.SetActive(false);
                UpgradesMenuUI.SetActive(false);
                GameOverUI.SetActive(true);
                break;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public bool hasBeatenGame;
    // Add any other data you want to save here
}
