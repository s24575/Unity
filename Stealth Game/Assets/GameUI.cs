using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;
    bool gameOver;

    void Start() {
        Guard.OnGuardHasSpottedPlayer += OnGameOver;
    }

    void Update() {
        if (gameOver) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                SceneManager.LoadScene(0);
            }
        }
    }

    void OnGameOver() {
        gameOverScreen.SetActive(true);
        gameOver = true;
    }

    void OnGameWon() {
        gameWinScreen.SetActive(true);
        gameOver = true;
    }
}
