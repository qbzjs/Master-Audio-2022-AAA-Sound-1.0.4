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
    [SerializeField] private GameObject winButton, winScreen, loseScreen, upgradeScreen;
    [SerializeField] private SerializableDictionaryBase<string, int> startingDeck;

    [SerializeField] private int winningScore;
    public int upgradeIncrement;
    public int totalTurns;

    private int score;

    public int Score
    {
        get => score;
        set
        {
            int nextUpgrade = (1 + score / upgradeIncrement) * upgradeIncrement;
            if (score < nextUpgrade && value > nextUpgrade)
            {
                UpgradeManager.Instance.PopulateUpgrades();
                upgradeScreen.SetActive(true);
                
            }
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
        NewBoard();
        InitializeDeck();
    }

    public void NewBoard()
    {
        winButton.SetActive(false);
        winScreen.SetActive(false);
        upgradeScreen.SetActive(false);
        loseScreen.SetActive(false);
        GridManager.Instance.Initialize();
        Score = 0;
        Turns = totalTurns;
    }

    public void ChangeTurns(int delta)
    {
        totalTurns += delta;
    }

    public void ChangeUpgradeTiming(int delta)
    {
        upgradeIncrement += delta;
        if (upgradeIncrement < 5) upgradeIncrement = 5;
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
        Debug.Log("placed a block");
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
