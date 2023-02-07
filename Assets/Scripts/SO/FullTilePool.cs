using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

[CreateAssetMenu(menuName = "Tile Pool")]
public class FullTilePool : ScriptableObject
{
     [BoxGroup("Rarity Weights - must add to 1")] public float CommonWeight, UncommonWeight, RareWeight, LegendaryWeight;
     [BoxGroup("Rarity Colors")] public Color CommonColor, UncommonColor, RareColor, LegendaryColor;     
     [BoxGroup("Tile Lists"), SerializeField] public SerializableDictionaryBase<string, int> startingDeck;
     [BoxGroup("Tile Lists")] public List<string> Commons, Uncommons, Rares, Legendaries;


}
