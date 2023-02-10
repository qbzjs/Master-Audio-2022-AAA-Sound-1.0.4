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
        deck.Shuffle();
    }
    
    public void AddToDeck(string s)
    {
        deck.Add(s);
        //deck.Shuffle();
    }

    public string Draw()
    {
        if(deck.Count == 0) ShuffleBack();
        
        string toReturn = deck[0];

        discard.Add(toReturn);
        deck.RemoveAt(0);
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
