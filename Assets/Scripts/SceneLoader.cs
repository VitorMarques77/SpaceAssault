using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 2f;
    GameSession gameSession;
    PlayerPowerUps playerPowerUps;

    private void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        playerPowerUps = FindObjectOfType<PlayerPowerUps>();
    }

    private void Update()
    {
        if (gameSession == null)
        {
            gameSession = FindObjectOfType<GameSession>();
        }
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitForGameOverScreen());
        
    }

    IEnumerator WaitForGameOverScreen()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene("GameOver");
        playerPowerUps.ResetPowerUps();
        gameSession.ChildrenReset();
    }

    public void LoadGameScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("MainMenu");
        gameSession.ResetGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        gameSession.ResetGame();

        if(playerPowerUps != null)
        {
            playerPowerUps.ResetPowerUps();
        }
        
        int sceneIndex = PlayerPrefs.GetInt("LastScene");
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadCurrentScene()
    {
        StartCoroutine(WaitForLoadCurrentScene());
    }

    IEnumerator WaitForLoadCurrentScene()
    {
        yield return new WaitForSeconds(delayInSeconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadWinScreen()
    {

        if (SceneManager.GetActiveScene().name.Equals("LastLevel"))
        {
            if (FindObjectOfType<PlayerPowerUps>() != null)
            {
                if (playerPowerUps.GetInstanceID() != FindObjectOfType<PlayerPowerUps>().GetInstanceID())
                {
                    playerPowerUps = FindObjectOfType<PlayerPowerUps>();
                }
            }
            if(FindObjectOfType<GameSession>() != null)
            {
                if(gameSession.GetInstanceID() != FindObjectOfType<GameSession>().GetInstanceID())
                {
                    gameSession = FindObjectOfType<GameSession>();
                }
            }
            SceneManager.LoadScene("WinScreen");
            playerPowerUps.ResetPowerUps();
            gameSession.ChildrenReset();
        }
        else
        {
            LoadGameScene();
        }


    }
}
