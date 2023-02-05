using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Foldout("UI")]
    [SerializeField] private Image progressBar;
    [Foldout("UI")]
    [SerializeField] private Image ProgressBarShadow;
    [Foldout("UI")]
    [SerializeField] private TextMeshProUGUI turnCounter, loseFinalScore, winFinalScore, winTurnCounter, progressCounter, pointsDescription;
    [Foldout("UI")]
    [SerializeField] private GameObject winButton, winScreen, loseScreen, upgradeScreen;
    [Foldout("UI")]
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private SerializableDictionaryBase<string, int> startingDeck;

    [SerializeField] private int winningScore;
    public int upgradeIncrement;
    public int totalTurns;
    private int score;

    private List<Rule> rules;

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
            float oldScore = Score;
            score = value;
            if (score >= winningScore)
            {
                winButton.SetActive(true);
            }
            ScoreIncrementEffect(oldScore);  
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

    public void Awake()
    {
        rules = new();
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

    public void AddRule(Rule newRule)
    {
        if (rules.Any((value) => { return value.description == newRule.description; }))
        {
            //no duplicates
            return;
        }
        rules.Add(newRule);
        rules.Sort(); //sort as we add
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
            ITile hoveringOver = GridManager.Instance.GetTile(tilePos.x, tilePos.y);
            tooltip.Show(hoveringOver.GetType().ToString(), hoveringOver.CalculateScore()); 
            //tooltip.Show(hoveringOver.Type(), hoveringOver.CalculateScore()); 
            //tooltip.Hide();
        }
        else
        {
            tooltip.Hide();
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
        //Rules are sorted as they are added so we can just run through them here
        foreach (Rule rule in rules)
        {
            rule.action.Invoke();
        }
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
    public void ScoreIncrementEffect(float oldScore){
        LeanTween.value(gameObject, oldScore, score, 0.1f)
        .setEaseOutQuart()
        .setOnUpdate((val)=>{ProgressBarShadow.fillAmount = val / winningScore;});    

        LeanTween.value(gameObject, oldScore, score, 2f)
        .setEaseInOutQuart()
        .setOnUpdate(setProgress);
    }
    public void setProgress(float val){
        progressCounter.text = "Progress: " + (int)val+ "/" + winningScore;
        progressBar.fillAmount = val / winningScore;
    }
}


