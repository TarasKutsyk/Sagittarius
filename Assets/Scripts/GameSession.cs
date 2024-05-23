using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    int score = 0;

    public int Score { get => score; set => score = value; }

    private void Awake()
    {
        SetUpSingleton();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game Over")
        {
            var UI_textboxes = FindObjectsOfType<TextMeshProUGUI>();
            int n = FindObjectsOfType<TextMeshProUGUI>().Length;
            
            for (int i = 0; i < n; i++)
            {
                if (UI_textboxes[i].tag == "Score Text")
                {
                    UI_textboxes[i].text = score.ToString();
                    break;
                }
            }
        }
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GetScore()
    {
        score = FindObjectOfType<Score>().CurrentScore;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

}