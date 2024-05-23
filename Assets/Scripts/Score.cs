using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int currentScore = 0;

    public int CurrentScore { get => currentScore; set => currentScore = value; }

    void Start()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = currentScore.ToString();
    }

    public void IncreaseScore (int addition)
    {
        currentScore += addition;
        DisplayScore();
    }
}
