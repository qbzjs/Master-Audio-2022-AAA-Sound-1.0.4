using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DarkTonic.MasterAudio;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using Unity.VisualScripting;

public class DeckManager : Singleton<DeckManager>
{

    [SerializeField] private FullTilePool tilePool;
    [SerializeField] public GameObject DiscardButton, DrawButton, DeckButton;
    [SerializeField] private TextMeshProUGUI DiscardText, DrawText;
    [SerializeField] private Card template;
    [SerializeField] private GameObject deckParent;
    [SerializeField] public RectTransform topRightBoundary;

    [Foldout("Tooltip")]
    [SerializeField] public SerializableDictionaryBase<string, string> Keywords;
    [Foldout("Tooltip")]
    [SerializeField] public GameObject tooltipPrefab;

    private List<string> discard = new(), deck = new(), drawPile = new();
    
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
         MasterAudio.PlaySound("CardsCreak");
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
            newCard.tooltipParent.SetActive(false);
            if(newCard.cardRef)
            {
                newCard.cardRef.gameObject.SetActive(false);
            } 
            newCard.GetComponent<Image>().raycastTarget=true;
            for (int i = 1; i < newDeck[cardName]; i++)
            {
                Card innerCard = createCardFromTile(cardName, newCard.gameObject.transform);
                RectTransform rt = innerCard.gameObject.GetComponent<RectTransform>();
                innerCard.SetCardAnchoredSize(rt);
                LeanTween.moveLocalY(innerCard.gameObject, -(i*15f), 0.75f).setDelay(0.5f);
                innerCard.tooltipParent.SetActive(false);
                if(innerCard.cardRef)
                {
                    innerCard.cardRef.gameObject.SetActive(false);
                } 
            } 
            
        }
    }

    public Dictionary<string, int> GetDeckList()
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

        return newDeck;
    }

    public Card createCardFromTile(string name, Transform parent)
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
    }
}