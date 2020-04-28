using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    // class responsible for scores, lives and etc

    // defines the initial score at 0
    private int score = 0;

    public int sceneIndex = default;



    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {

    }

    // singleton pattern to make sure the score will go on through the next level
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public void ChildrenReset()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
    }
}
