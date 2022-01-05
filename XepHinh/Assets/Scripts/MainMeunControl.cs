using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMeunControl : MonoBehaviour
{
    public void Game()
    {
        Application.LoadLevel("Game");

    }
    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
