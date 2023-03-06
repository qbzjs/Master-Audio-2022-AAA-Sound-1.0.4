using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private Card template;
    [SerializeField] private GameObject parent;

    private List<string> discard = new(), deck = new(), drawPile = new();
    private List<Card> cards = new();
    
    public void ShuffleBack()
    {
        drawPile.Clear();
        discard.Clear();
        drawPile.AddRange(deck);
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

    public void LoadDeck()
    {
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();
        foreach (var name in deck)
        {
            Card newCard = Instantiate(template, parent.transform);
            newCard.CreateCardNewTile(name);
            cards.Add(newCard);
        }
    }

    public string Draw()
    {
        if(drawPile.Count == 0) ShuffleBack();
        
        string toReturn = drawPile[^1]; //index from end expression

        discard.Add(toReturn);
        drawPile.RemoveAt(drawPile.Count - 1);
        return toReturn;
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
                    discard.RemoveAt(i);
                    break;
                }
            }
        }
        else
        {
            Debug.Log($"no card found in deck with name \"{toRemove}\"");
        }
    }

}
