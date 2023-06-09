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
    [Foldout("Tutorial")]
    [SerializeField] public List<GameObject> TutorialScreens;
    [Foldout("Tutorial")]
    [SerializeField] public GameObject TutorialParent;
    public bool TutorialMode {get; set;}

    public bool Interactable
    {
        get
        {
            return !(winScreen.activeInHierarchy || endScreen.activeInHierarchy || upgradeScreen.activeInHierarchy || deckScreen.activeInHierarchy);
        }
    }

    public bool DeckScreenActive
    {
        get
        {
            return deckScreen.activeInHierarchy;
        }
    }

    private int tutorialIndex;

    [SerializeField] public Tooltip Tooltip;

    [Foldout("UI")]
    [SerializeField] private Image progressBar;
    [Foldout("UI")]
    [SerializeField] private Image ProgressBarShadow;
    [Foldout("UI")]
    [SerializeField] private TextMeshProUGUI turnCounter, loseFinalScore, nextRound, roundsLeftText, progressCounter;
    [Foldout("UI")]
    [SerializeField] private GameObject winButton, winScreen, loseScreen, upgradeScreen, deckScreen, tempBlocker, endScreen, upgradePointer;
    [Foldout("UI")]
    [SerializeField] private FullTilePool tilePool;

    public delegate void VoidDelegate();

    public static VoidDelegate gameStart;

    private Action ExecuteAfterScore;

    [SerializeField, BoxGroup("Difficulty Parameters")]
    private int winningScore, upgradeIncrement, totalTurns, winningScoreIncrement, totalRounds;
    private int roundsLeft;
    private int score;
    private List<GameObject> upgradePointers = new();

    private List<Rule> rules = new List<Rule>();

    public List<GameObject> SetOffInEditor;

    public bool dragging;

    public int Round => totalRounds - roundsLeft;

    public int Score
    {
        get => score;
        set
        {
            int nextUpgrade = CalculateNextUpgrade();
            if (Score < nextUpgrade && value >= nextUpgrade)
            {
                UpgradeManager.Instance.FirstTimeUpgrade();
                TweenManager.Instance.Emphasize(progressBar.transform.parent.gameObject);
                if (value < winningScore)
                {
                    //TODO do an effect on the appropriate upgrade pointer here
                    /*int index = (int)(value / upgradeIncrement) - 1;
                    TweenManager.Instance.Emphasize(upgradePointers[index]);*/
                }
                tempBlocker.SetActive(true);
                //upgradeScreen.SetActive(true);
                
            }
            float oldScore = Score;
            score = value;
            if (score >= winningScore)
            {
                winButton.SetActive(true);
            }
            ScoreIncrementEffect(oldScore, nextUpgrade);  
        }
    }

    private int CalculateNextUpgrade()
    {
        return (1 + score / upgradeIncrement) * upgradeIncrement;
    }
    
    private int turns;

    public int Turns
    {
        get => turns;
        set
        {
            if (turns != value)
            {
                TweenManager.Instance.Emphasize(turnCounter.GameObject(), value < 5 ? 1.5f : 1);
            //    TweenManager.Instance.Emphasize(turnCounter.GameObject(), new Color(0.62f, 0.09f, 0.11f));
            }
            turns = value;
            turnCounter.text = turns + " turns remaining.";
            if (turns <= 0)
            {
                ParticleManager.Instance.InstantiateEndParticles();
                if (score >= winningScore)
                {
                    ExecuteAfterScore = Win;
                }
                else
                {
                    ExecuteAfterScore = Lose;
                }
            }
        } 
    }

    public void Awake()
    {
        upgradeIncrement = winningScore / 2;
        roundsLeft = totalRounds;
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
        gameStart?.Invoke();
        gameStart = null;
        if(!TutorialParent.activeSelf)
        {
            BlockSpawner.Instance.GenerateBlock();
            BlockSpawner.Instance.GenerateMaw();
        }
    }

    public void NewBoard()
    {
        winButton.SetActive(false);
        winScreen.SetActive(false);
        upgradeScreen.SetActive(false);
        deckScreen.SetActive(false);
        loseScreen.SetActive(false);
        GridManager.Instance.Initialize();
        Score = 0;
        Turns = totalTurns;
        tutorialIndex = 0;
        SetUpgradePointers();
    }

    public void SetUpgradePointers()
    {
        upgradePointer.SetActive(true);
        foreach (GameObject pointer in upgradePointers)
        {
            Destroy(pointer);
        }
        RectTransform progressBarRect = ProgressBarShadow.GetComponent<RectTransform>();
        
        for (int i = upgradeIncrement; i < winningScore; i += upgradeIncrement)
        {
            RectTransform pointer = Instantiate(upgradePointer, upgradePointer.transform.parent).GetComponent<RectTransform>();
            upgradePointers.Add(pointer.gameObject);
            Vector3 tempPos = pointer.position;
            Vector3[] corners = new Vector3[4];
            progressBarRect.GetWorldCorners(corners);
            float width = corners[2].x - corners[0].x;
            tempPos.x = corners[0].x + ((float) i / winningScore) * width;
            pointer.position = tempPos;
        }
        upgradePointer.SetActive(false);
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
        if(TutorialMode && tutorialIndex < TutorialScreens.Count)
        {
            CanvasGroup canvas = TutorialScreens[tutorialIndex].AddComponent<CanvasGroup>();
            canvas.alpha = 0f;
            TutorialScreens[tutorialIndex].SetActive(true);
            LeanTween.alphaCanvas(canvas, 1f, 0.5f).setDelay(2f);
            tutorialIndex += 1;
        }
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
        roundsLeft--;
        if (roundsLeft <= 0)
        {
            TweenManager.Instance.AddCallbackToQueue(() =>
            {
                endScreen.SetActive(true);
                endScreen.GetComponent<EndScreen>().Won();
            });
            return;
        }
        TweenManager.Instance.AddCallbackToQueue(() =>
        {
            
            winScreen.SetActive(true);
            winningScore += winningScoreIncrement;

            if (roundsLeft == 1)
            {
                roundsLeftText.text = "";
                nextRound.text = $"Build me one last city worth {winningScore} points";
            }
            else
            {
                roundsLeftText.text = $"Well Done, But you have {roundsLeft} rounds remaining";
                nextRound.text = "Next Round, build me a city worth " + winningScore + " points";
            }
        
       
            upgradeIncrement = winningScore / 3;
            rules.Clear();
        });

    }

    public void Lose()
    {
        TweenManager.Instance.AddCallbackToQueue(() =>
        {
            endScreen.SetActive(true);
            endScreen.GetComponent<EndScreen>().Lose();
            /*loseScreen.SetActive(true);
            loseFinalScore.text = "Final Score: " + score + "/" + winningScore;*/
            rules.Clear();
        });
        

    }

    public void Reset()
    {
        SceneManager.GetActiveScene().Load();
    }
    
    public void ScoreIncrementEffect(float oldScore, float nextUpgrade)
    {
        bool willUpgrade = false;
        LeanTween.value(gameObject, oldScore, score, 0.1f)
        .setEaseOutQuart()
        .setOnUpdate((val)=>{ProgressBarShadow.fillAmount = val / winningScore;});    

        LeanTween.value(gameObject, oldScore, score, 2f)
        .setEaseOutSine()
        .setOnUpdate((val)=>
        {
            int untilNext = (int)nextUpgrade - (int) val;
            if (untilNext <= 0)
            {
                nextUpgrade += upgradeIncrement;
                willUpgrade = true;
            }
            if(TutorialMode)
            {
                progressCounter.text = $"{(int) val}/{winningScore}"; 
            }
            else 
            {
                 progressCounter.text = $"{(int) val}/{winningScore}";
                 /*if (willUpgrade)
                 {
                     progressCounter.text = "Prepare for a deal";
                 }*/
            }
            if (progressBar != null){
                progressBar.fillAmount = val / winningScore;
            }
            })
        .setDelay(0.5f).
        setOnComplete(() =>
        {

            progressCounter.text = $"{score}/{winningScore}";

            if (ExecuteAfterScore != null)
            {
                tempBlocker.SetActive(false);
                ExecuteAfterScore();
                ExecuteAfterScore = null;
            } else if (willUpgrade)
            {
                tempBlocker.SetActive(false);
                if (!TutorialMode){
                    upgradeScreen.SetActive(true);
                    TweenManager.Instance.SlideOnLocal(upgradeScreen);
                }
            }
        });
    }

    public void DestroyTile(Vector2Int location)
    {
        if (GridManager.Instance.GetTile(location.x, location.y).GetType().ToString() == "Wasteland")
        {
            return; //no effect if it's a wasteland
        }

        GameObject tileObject = GridManager.Instance.DestroyTile(location);
        TweenManager.Instance.DestroyEffect(location, () =>
        {
            Destroy(tileObject);
        });
        
    }

    public void TransformTile(Vector2Int location, string newTileType)
    {
        string calloutText =
            $"{GridManager.Instance.GetTile(location.x, location.y).GetType().ToString()} becomes {newTileType}!";
        GameObject oldTileObject = GridManager.Instance.DestroyTile(location, false);
        GameObject newTileObject = GridManager.Instance.PlaceTile(newTileType, location);
        
        newTileObject.transform.position = Vector3.one * 1000;
        TweenManager.Instance.TransformEffect(location, () =>
        {
            TweenManager.Instance.Callout(calloutText, location);
            Destroy(oldTileObject);
            if (newTileObject != null)
            {
                newTileObject.transform.position = GridManager.Instance.GridToWorldPos(location);
            }
        });
    }

    public void PlaceTile(string newTileType, Vector2Int location)
    {
        if (GridManager.Instance.GetTile(location.x, location.y).GetType().ToString() != "Wasteland")
        {
            Debug.Log("This tile is not a wasteland");
            return; //can't create a tile unless it's on a wasteland!
        }
        
        GameObject tileObject = GridManager.Instance.PlaceTile(newTileType, location);
        tileObject.transform.position = Vector3.one * 1000;
        TweenManager.Instance.CreateEffect(location, () =>
        {
            tileObject.transform.position = GridManager.Instance.GridToWorldPos(location);
        });
    }

}


