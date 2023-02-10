using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    private List<string> discard;
    private List<string> deck;

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
        deck.Add(s);
    }

    public string Draw()
    {
        if(deck.Count == 0) ShuffleBack();
        
        string toReturn = deck[^1]; //index from end expression

        discard.Add(toReturn);
        deck.RemoveAt(deck.Count - 1);
        return toReturn;
    }

    public void Print()
    {
        Debug.Log("deck: \n");
        foreach (string card in deck)
        {
            Debug.Log(card);
        }
        
        Debug.Log("discard: \n");
        foreach (string card in discard)
        {
            Debug.Log(card);
        }
    }

}
