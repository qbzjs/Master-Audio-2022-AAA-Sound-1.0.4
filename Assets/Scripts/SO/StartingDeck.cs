using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(menuName = "Starting Deck")]
public class StartingDeck : ScriptableObject
{
     [BoxGroup("Starting deck"), SerializeField] public SerializableDictionaryBase<string, int> deck;
}
