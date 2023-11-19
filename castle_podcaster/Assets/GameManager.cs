using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            RestartGame();
        }
    }

    [SerializeField] GameObject gameOverScreen;
    bool isGameOver;
    public void SetGameOver()
    {
        gameOverScreen.SetActive(true);
        isGameOver = true;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
