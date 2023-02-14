using System;
using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private Card template;
    [SerializeField] private GameObject parent;

    private List<KeyValuePair<string, Card>> discard;
    private List<KeyValuePair<string, Card>> deck;

    private void Awake()
    {
        discard = new();
        deck = new();
    }

    public void ShuffleBack()
    {
        deck.AddRange(discard);
        Shuffle();
    }

    public void Shuffle()
    {
        deck.Shuffle();
    }
    
    public void AddToDeck(string s)
    {
        Card newCard = Instantiate(template, parent.transform);
        newCard.CreateCardNewTile(s);
        deck.Add(new KeyValuePair<string, Card>(s, newCard));
    }

    public string Draw()
    {
        if(deck.Count == 0) ShuffleBack();
        
        KeyValuePair<string, Card> toReturn = deck[^1]; //index from end expression

        discard.Add(toReturn);
        deck.RemoveAt(deck.Count - 1);
        return toReturn.Key;
    }

    public void Print()
    {
 /*       Debug.Log("deck: \n");
        foreach (string card in deck)
        {
            Debug.Log(card);
        }
        
        Debug.Log("discard: \n");
        foreach (string card in discard)
        {
            Debug.Log(card);
        }*/
    }

}
