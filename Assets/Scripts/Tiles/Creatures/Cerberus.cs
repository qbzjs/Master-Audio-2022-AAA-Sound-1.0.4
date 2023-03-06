﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;

public class Cerberus : Monster
{
    [SerializeField] protected int packSize = 3;

    private int bigScore = 10, smallScore = 0;

    public override Tag[] GetTags()
    {
        return new [] {Tag.Monster};
    }

    public override string GetDescription()
    {
        return "+10 if in a pack of exactly 3, 0 otherwise.";
    }
    public Cerberus(Transform parentTransform, Vector3 pos) : base(parentTransform, pos)
    {

    }

    protected override Score CalculateBaseScore()
    {
        List<Creature> pack = new();
        pack.Add(this);
        int packCount = CountGroupCreatures(this.GetType(), pack);

        if (packCount == packSize)
        {
            scoreWorth = bigScore;
        } else
        {
            scoreWorth = smallScore;
        }

        return new Score(scoreWorth);
    }
}
