using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI turnCounter, loseFinalScore, winFinalScore, winTurnCounter;
    [SerializeField] private GameObject winButton, winScreen, loseScreen; 
    public int winningScore;
    public int totalTurns;

    private int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            if (score >= winningScore)
            {
                winButton.SetActive(true);
            }
            progressBar.fillAmount = (float) score / winningScore;
        }
    }
    
    private int turns; 

    public int Turns
    {
        get => turns;
        set
        {
            turns = value;
            turnCounter.text = turns + " turns remaining.";
            if (turns <= 0)
            {
                Lose();
            }
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        winButton.SetActive(false);
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        Score = 0;
        Turns = totalTurns;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [NaughtyAttributes.Button]
    public void IncrementScore()
    {
        Score += 50;
    }
    
    
    [NaughtyAttributes.Button]
    public void DecrementTurns()
    {
        Turns--;
    }

    public void Win()
    {
        winScreen.SetActive(true);
        winFinalScore.text = "Final Score: " + score + "/" + winningScore;
        winTurnCounter.text = "With " + turns + " turns to spare";
    }

    public void Lose()
    {
        loseScreen.SetActive(true);
        loseFinalScore.text = "Final Score: " + score + "/" + winningScore;
    }

    public void Reset()
    {
        SceneManager.GetActiveScene().Load();
    }
}
