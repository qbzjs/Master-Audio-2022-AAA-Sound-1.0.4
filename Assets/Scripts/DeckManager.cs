using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] public GameObject DiscardButton, DrawButton, DeckButton;
    [SerializeField] private Card template;
    [SerializeField] private GameObject deckParent, discardParent, drawParent;

    private List<string> discard = new(), deck = new(), drawPile = new();
    private List<Card> cardDeck = new();
    private List<Card> discardDeck = new();
    private List<Card> drawDeck = new();
    
    public void ShuffleBack()
    {
        TweenManager.Instance.MoveCard(DiscardButton.transform, DrawButton.transform, discard.Count);
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
        cardDeck = createCardDeckFromDeck(deck, deckParent.transform);
    }

    public void LoadDiscardDeck()
    {
        foreach (var card in discardDeck)
        {
            Destroy(card.gameObject);
        }
        discardDeck.Clear();
        discardDeck = createCardDeckFromDeck(discard, discardParent.transform);
    }

    public void LoadDrawDeck()
    {
        foreach (var card in drawDeck)
        {
            Destroy(card.gameObject);
        }
        drawDeck.Clear();
        drawDeck = createCardDeckFromDeck(drawPile, drawParent.transform);
    }

    public string Draw()
    {
        if(drawPile.Count == 0) ShuffleBack();
        
        string toReturn = drawPile[^1]; //index from end expression
        
        drawPile.RemoveAt(drawPile.Count-1);
        return toReturn;
    }

    public void Place(string name)
    {
        discard.Add(name);

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

    private List<Card> createCardDeckFromDeck(List<string> deck, Transform parent)
    {
        List<Card> newDeck = new();
        foreach (var name in deck)
        {
            Card newCard = createCardFromTile(name, parent);
            newDeck.Add(newCard);
        }
        return newDeck;
    }
    private Card createCardFromTile(string name, Transform parent)
    {
        Card newCard = Instantiate(template, parent.transform);
        newCard.CreateCardNewTile(name);
        return newCard;
    }
    public void moveBlockCards(int blockSize)
    {
        TweenManager.Instance.MoveCard(DrawButton.transform, DiscardButton.transform, blockSize);
    }
}
