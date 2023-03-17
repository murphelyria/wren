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

    void Start()
    {
        // Set initial state to main menu
        SetState(GameState.MainMenu);
    }

    void Update()
    {
        // Update the state and perform any actions related to the new state
        CurrentState = newState;
        switch (newState)
        {
            case GameState.MainMenu:
                // Display the main menu UI
                break;
            case GameState.Playing:
                // Start the game
                break;
            case GameState.UpgradesMenu:
                // Open upgrades menu UI
                break;
            case GameState.GameOver:
                // Display the game over UI
                break;
        }
    }
}
