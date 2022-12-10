using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI turnCounter, loseFinalScore, winFinalScore, winTurnCounter, progressCounter, pointsDescription;
    [SerializeField] private GameObject winButton, winScreen, loseScreen;
    [SerializeField] private SerializableDictionaryBase<string, int> startingDeck;

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
            progressCounter.text = "Progress: " + score + "/" + winningScore;
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
                if (score >= winningScore)
                {
                    Win();
                }
                else
                {
                    Lose();
                }
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
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        foreach (KeyValuePair<string, int> pair in startingDeck)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                DeckManager.Instance.AddToDeck(pair.Key);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (GridManager.Instance.OverGrid(mousePos))
        {
            Vector2Int tilePos = GridManager.Instance.WorldToGridPos(mousePos);
            string tileDesc = GridManager.Instance.GetTileDescription(tilePos.x, tilePos.y);
               
            pointsDescription.text = tileDesc;
        }
        
#if !UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    public void PlacedBlock()
    {
        GridManager.Instance.UpdateBlood();
        UpdateScore();
        DecrementTurns();
    }

    [NaughtyAttributes.Button]
    public void UpdateScore()
    {
        Score = GridManager.Instance.UpdateScore();
    }

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
