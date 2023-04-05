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
    [SerializeField] private GameObject deckParent;
    [SerializeField] public RectTransform topRightBoundary;

    private List<string> discard = new(), deck = new(), drawPile = new();
    
    public void ShuffleBack()
    {
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
        Transform deckTransform = deckParent.transform;
        while (deckTransform.childCount > 0) {
            DestroyImmediate(deckTransform.GetChild(0).gameObject);
        }
        createCardDictFromList(deck, deckTransform);
    }

    public void LoadDiscardDeck()
    {
       Transform deckTransform = deckParent.transform;
        while (deckTransform.childCount > 0) {
            DestroyImmediate(deckTransform.transform.GetChild(0).gameObject);
        }
        createCardDictFromList(discard, deckTransform);
    }

    public void LoadDrawDeck()
    {
        Transform deckTransform = deckParent.transform;
        while (deckTransform.childCount > 0) {
            DestroyImmediate(deckTransform.GetChild(0).gameObject);
        }
        createCardDictFromList(drawPile, deckTransform);
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

    private void createCardDictFromList(List<string> deck, Transform parent)
    {
        Dictionary<string, int> newDeck = new Dictionary<string, int>();
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
            newCard.GetComponent<Image>().raycastTarget=true;
            for (int i = 1; i < newDeck[cardName]; i++)
            {
                Card innerCard = createCardFromTile(cardName, newCard.gameObject.transform);
                RectTransform rt = innerCard.gameObject.GetComponent<RectTransform>();
                SetCardAnchoredSize(rt);
                LeanTween.moveLocalY(innerCard.gameObject, -(i*15f), 0f);
            } 
            CreateCardToolTips(newCard);
            newCard.tooltipParent.transform.SetAsLastSibling();
            
        }
    }

    private Card createCardFromTile(string name, Transform parent)
    {
        Card newCard = Instantiate(template, parent.transform);
        newCard.CreateCardNewTile(name);
        newCard.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = UpgradeManager.Instance.FindColor(name);
        return newCard;
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

        Toggle drawToggle = DrawButton.GetComponent<Toggle>();
        if (drawToggle.isOn)
        {
            LoadDrawDeck();
        }
        Toggle discardToggle = DiscardButton.GetComponent<Toggle>();
        if (discardToggle.isOn)
        {
            LoadDiscardDeck();
        }
    }

    public void SetCardAnchoredSize(RectTransform _mRect)
    {
        _mRect.sizeDelta = new Vector2(0f, 0f);
        _mRect.anchoredPosition = new Vector2(0f, 0f);
        _mRect.anchorMin = new Vector2(0, 0);
        _mRect.anchorMax = new Vector2(1, 1);
        _mRect.pivot = new Vector2(0.5f, 0.5f);
    }

    public void CreateCardToolTips(Card card)
    {
        string description = card.descriptionText.text;
        List<string> keys = new List<string>(GameManager.Instance.Keywords.Keys);
        foreach(string key in keys)
        {
            if (description.Contains(key))
            {
                GameObject newOb = Instantiate(GameManager.Instance.tooltipPrefab, card.tooltipParent.transform);
                TextMeshProUGUI newTextMesh = newOb.GetComponentInChildren<TextMeshProUGUI>();
                newTextMesh.text = key + ": " + GameManager.Instance.Keywords[key];
                card.toolTips.Add(newOb);
            }
        }
        TMP_TextInfo textInfo = card.descriptionText.GetTextInfo(description);
        int linkCount = textInfo.linkCount;
        for(int i = 0; i < linkCount; i++)
        {
            string cardName = textInfo.linkInfo[i].GetLinkText();
            Card newCard = createCardFromTile(cardName, card.transform);
            newCard.gameObject.SetActive(false);
            RectTransform rt = newCard.gameObject.GetComponent<RectTransform>();
            SetCardAnchoredSize(rt);
            rt.anchoredPosition = new Vector2(0f, 350f);
            card.cardRefs.Add(newCard.gameObject);
        }
    }
}