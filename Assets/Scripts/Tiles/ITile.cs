using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITile
{
    public GameObject TileObject { get; set; }
    
    public Vector3 LocalPosition();

    public Vector3 Position();

    public bool Destructible();

    public void WhenPlaced();

    public void AddEffect(Effect toAdd);

    //nextToGraveYard
    //graves - count how many times been destroyed since next to graveyard

    public int xPos { get; set; }
    public int yPos { get; set; }
    public string Type();
    public Score CalculateScore();
}

public struct Effect
{
    public int order;
    public int stacks;
    public int maxStacks;
    public Func<Score, Score> modify;
}

public struct Score
{
    public int score;
    public string explanation;

    public Score(int _score, string _explanation)
    {
        score = _score;
        explanation = _explanation;
    }
    
    public Score(int _score)
    {
        score = _score;
        explanation = _score.ToString();
    }
}
