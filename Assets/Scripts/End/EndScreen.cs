using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject deckListParent, deckLabel, points, summaryLabel, playAgain, feedback, round;
    [SerializeField] private TextMeshProUGUI deckItem, pointValue, title, roundValue;

    [SerializeField] private float shortWait, longWait;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [Button()]
    public void Lose()
    {
        StartCoroutine(Ending(false));
    }


    [Button()]
    public void Won()
    {
        StartCoroutine(Ending(true));
    }

    [Button()]
    public void Reset()
    {
        GameManager.Instance.Reset();
    }
    
    public IEnumerator Ending(bool won)
    {
        deckLabel.SetActive(false);
        points.SetActive(false);
        summaryLabel.SetActive(false);
        playAgain.SetActive(false);
        feedback.SetActive(false);
        round.SetActive(false);
        deckListParent.transform.DestroyAllChildren();
        
        title.text = won ? "You Won!" : "You have failed.";
        yield return new WaitForSeconds(longWait);
        MasterAudio.PlaySound("Click");
        summaryLabel.SetActive(true);
        
        yield return new WaitForSeconds(longWait);
        MasterAudio.PlaySound("Click");
        round.SetActive(true);
        if (won)
        {
            roundValue.text = "!!!";
        }
        else
        {
            roundValue.text = (GameManager.Instance.Round + 1).ToString();
        }
        
        yield return new WaitForSeconds(longWait);
        MasterAudio.PlaySound("Click");
        points.SetActive(true);
        pointValue.text = GameManager.Instance.Score.ToString();
        
        yield return new WaitForSeconds(longWait);
        MasterAudio.PlaySound("Click");
        deckLabel.SetActive(true);

        Dictionary<string, int> deckList = DeckManager.Instance.GetDeckList();
        foreach (KeyValuePair<string, int> pair in deckList)
        {
            yield return new WaitForSeconds(shortWait);
            MasterAudio.PlaySound("Click");
            TextMeshProUGUI newDeckItem = Instantiate(deckItem, deckListParent.transform);
            newDeckItem.text = $"{pair.Value}x {pair.Key}";
        }
                
        yield return new WaitForSeconds(longWait);
        MasterAudio.PlaySound("Click");
        playAgain.SetActive(true);
        
        yield return new WaitForSeconds(longWait);
        MasterAudio.PlaySound("Click");
        feedback.SetActive(true);
    }
}
