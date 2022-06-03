using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI scoreIndicator;
    public Gradient healthGradient;
    public Color healthColor;
    public float playerHealth = 100;
    public int score = 0;
    public bool gameOver = false;

    private void Update()
    {
        scoreIndicator.text = score.ToString();
        if(playerHealth > 100) { playerHealth = 100; }
        if(playerHealth < .1f) { GameOver(); }
        healthColor = healthGradient.Evaluate(playerHealth / 100);
    }

    public void UpdateScore(int scoreModifier)
    {
        score += scoreModifier;
    }

    public void UpdateHealth(int healthModifier)
    {
        playerHealth += healthModifier;
    }
    public void GameOver()
    {
        gameOver = true;
        playerHealth = 0;
    }
}
