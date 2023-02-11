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
    [SerializeField] private TextMeshProUGUI turnCounter, loseFinalScore, winFinalScore, winTurnCounter, progressCounter;
    [Foldout("UI")]
    [SerializeField] private GameObject winButton, winScreen, loseScreen, upgradeScreen;
    [Foldout("UI")]
    [SerializeField] public Tooltip tooltip;
    [SerializeField] private FullTilePool tilePool;

    [SerializeField, BoxGroup("Difficulty Parameters")] private int winningScore, upgradeIncrement, totalTurns, winningScoreIncrement;
    private int score;

    private List<Rule> rules = new List<Rule>();

    public List<GameObject> SetOffInEditor;

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
        #if UNITY_EDITOR
        foreach (var GO in SetOffInEditor)
        {
            GO.SetActive(false);
        }
        #endif
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
        foreach (KeyValuePair<string, int> pair in tilePool.startingDeck)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                DeckManager.Instance.AddToDeck(pair.Key);
            }
        }
        DeckManager.Instance.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {

        
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

    public void AddTurns(int numTurns)
    {
        Turns += numTurns;
    }

    public void Win()
    {
        winScreen.SetActive(true);
        winFinalScore.text = "Final Score: " + score + "/" + winningScore;
        winTurnCounter.text = "With " + turns + " turns to spare";
        winningScore += winningScoreIncrement;
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
        .setEaseOutSine()
        .setOnUpdate((val)=>{
            progressCounter.text = "Progress: " + (int)val+ "/" + winningScore; 
            if (progressBar != null){
                progressBar.fillAmount = val / winningScore;
            }
            }).setDelay(1.1f);
    }

}


