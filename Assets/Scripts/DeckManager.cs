using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckManager : Singleton<DeckManager>
{
     [SerializeField] private FullTilePool tilePool;
    [SerializeField] public GameObject DiscardButton, DrawButton, DeckButton;
    [SerializeField] private TextMeshProUGUI DiscardText, DrawText;
    [SerializeField] private Card template;
    [SerializeField] private GameObject deckParent, discardParent, drawParent;

    private List<string> discard = new(), deck = new(), drawPile = new();
    private List<Card> cardDeck = new();
    private List<Card> discardDeck = new();
    private List<Card> drawDeck = new();
    
    public void ShuffleBack()
    {
        TweenManager.Instance.MoveCard(DiscardButton.transform, DrawButton.transform, discard.Count);
        UpdateDeckCounts();
        drawPile.AddRange(discard);
        discard.Clear();
        Shuffle();
    }

    public void Shuffle()
    {
        drawPile.Shuffle();

    }
    
    public void AddToDeck(string s)
    {
        drawPile.Add(s);
        deck.Add(s);

    }

    public void LoadCardDeck()
    {
        foreach (var card in cardDeck)
        {
            Destroy(card.gameObject);
        }
        cardDeck.Clear();
        cardDeck = createCardDictFromList(deck, deckParent.transform);
    }

    public void LoadDiscardDeck()
    {
        foreach (var card in discardDeck)
        {
            Destroy(card.gameObject);
        }
        discardDeck.Clear();
        discardDeck = createCardDictFromList(discard, discardParent.transform);
    }

    public void LoadDrawDeck()
    {
        foreach (var card in drawDeck)
        {
            Destroy(card.gameObject);
        }
        drawDeck.Clear();
        drawDeck = createCardDictFromList(drawPile, drawParent.transform);
    }

    public string Draw()
    {
        if(drawPile.Count == 0) ShuffleBack();
        
        string toReturn = drawPile[^1]; //index from end expression
        
        drawPile.RemoveAt(drawPile.Count-1);
        UpdateDeckCounts();
        return toReturn;
    }

    public void Place(string name)
    {
        discard.Add(name);
        UpdateDeckCounts();

    }
    public string GetRandomCard()
    {
        deck.Shuffle();
        return deck[0];
    }

    public void Remove(string toRemove)
    {
        if (deck.Exists((pair) => pair == toRemove))
        {
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i] == toRemove)
                {
                    deck.Remove(toRemove);
                    drawPile.Remove(toRemove);
                    break;
                }
            }
        } else
        if (discard.Exists((pair) => pair == toRemove))
        {
            for (int i = 0; i < deck.Count; i++)
            {
                if (deck[i] == toRemove)
                {
                    deck.Remove(toRemove);
                    discard.Remove(toRemove);
                    break;
                }
            }
        }
        else
        {
            Debug.Log($"no card found in deck with name \"{toRemove}\"");
        }
    }

    private List<Card> createCardDictFromList(List<string> deck, Transform parent)
    {
        Dictionary<string, int> newDeck = new Dictionary<string, int>();
        List<Card> returnCards = new();
        foreach(var cardName in deck)
        {
            if(!newDeck.ContainsKey(cardName))
            {  
                newDeck.Add(cardName, 0);
            }
            newDeck[cardName]++;
        }
        foreach(var cardName in newDeck.Keys)
        {
            Card newCard = createCardFromTile(cardName, parent);
            returnCards.Add(newCard);
            newCard.GetComponent<Image>().raycastTarget=true;
            for (int i = 1; i < newDeck[cardName]; i++)
            {
                Card innerCard = createCardFromTile(cardName, newCard.gameObject.transform);
                returnCards.Add(innerCard);
                RectTransform rt = innerCard.gameObject.GetComponent<RectTransform>();
                SetCardAnchoredSize(rt);
                LeanTween.moveLocalY(innerCard.gameObject, -(i*15f), 0.75f);
            } 
            CreateCardToolTips(newCard, newCard.transform);
            
        }
        return returnCards;
    }
    private Card createCardFromTile(string name, Transform parent)
    {
        Card newCard = Instantiate(template, parent.transform);
        newCard.CreateCardNewTile(name);
        newCard.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = UpgradeManager.Instance.FindColor(name);
        return newCard;
    }
    public void moveBlockCards(int blockSize)
    {
        TweenManager.Instance.MoveCard(DrawButton.transform, DiscardButton.transform, blockSize);
    }
    private void UpdateDeckCounts()
    {
        float discardNum = float.Parse(DiscardText.text);
        float newDiscardNum = (float)discard.Count;
        LeanTween.value(gameObject, discardNum, newDiscardNum, 1.5f)
        .setEaseOutSine()
        .setOnUpdate(
            (val)=>
            { 
                DiscardText.text = $"{(int)val}"; 
            });  
        float drawNum = float.Parse(DrawText.text);
        float newDrawNum = (float)drawPile.Count;
        LeanTween.value(gameObject, drawNum, newDrawNum, 1.5f)
        .setEaseOutSine()
        .setOnUpdate(
            (val)=>
            { 
                DrawText.text = $"{(int)val}"; 
            });  
    }

    public void SetCardAnchoredSize(RectTransform _mRect)
    {
        _mRect.sizeDelta = new Vector2(0f, 0f);
        _mRect.anchoredPosition = new Vector2(0f, 0f);
        _mRect.anchorMin = new Vector2(0, 0);
        _mRect.anchorMax = new Vector2(1, 1);
        _mRect.pivot = new Vector2(0.5f, 0.5f);
    }

    public void CreateCardToolTips(Card card, Transform parent)
    {
        string description = card.descriptionText.text;
        List<string> keys = new List<string>(GameManager.Instance.Keywords.Keys);
        foreach(string key in keys)
        {
            if (description.Contains(key))
            {
                GameObject newOb = Instantiate(GameManager.Instance.tooltipPrefab, parent);
                TextMeshProUGUI newTextMesh = newOb.GetComponentInChildren<TextMeshProUGUI>();
                newTextMesh.text = key + ": " + GameManager.Instance.Keywords[key];
                card.toolTips.Add(newOb);
                newOb.SetActive(false);

            }
        }
        TMP_TextInfo textInfo = card.descriptionText.GetTextInfo(description);
        int linkCount = textInfo.linkCount;
        for(int i = 0; i < linkCount; i++)
        {
            string cardName = textInfo.linkInfo[i].GetLinkText();
            Card newCard = createCardFromTile(cardName, parent);
            newCard.gameObject.SetActive(false);
            RectTransform rt = newCard.gameObject.GetComponent<RectTransform>();
            SetCardAnchoredSize(rt);
            rt.anchoredPosition = new Vector2(0f, 350f);
            card.toolTips.Add(newCard.gameObject);
        }
    }
}