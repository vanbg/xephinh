using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPupop : MonoBehaviour
{
    public GameObject gameOverPopup;
    
    public GameObject newBestScorePopup;

    void Start()
    {
        gameOverPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvent.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvent.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool newBestScore)
    {
        gameOverPopup.SetActive(true);
        
        newBestScorePopup.SetActive(true);
    }

}
