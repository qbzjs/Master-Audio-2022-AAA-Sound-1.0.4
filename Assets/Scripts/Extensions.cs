
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Extensions
{
    
    public static void Load(this Scene s)
    {
        SceneManager.LoadScene(s.buildIndex);
    }
    
    
    public static void Shuffle<T>(this List<T> toShuffle)
    {
        for (int i = 0; i < toShuffle.Count; i++)
        {
            int randomIndex = Random.Range(0, toShuffle.Count);

            // Swap the current value with the value at the random index
            (toShuffle[randomIndex], toShuffle[i]) = (toShuffle[i], toShuffle[randomIndex]);
        }
    }
    
    public static T PickRandom<T>(this List<T> toPickFrom)
    {
        return toPickFrom[Random.Range(0, toPickFrom.Count)];
    }
    
//
    public static Corners GetCorners(this RectTransform rectTransform, bool localCoords = false)
    {
        Vector3[] corners = new Vector3[4];
        if (localCoords)
        {
            rectTransform.GetLocalCorners(corners);
        }
        else
        {
            rectTransform.GetWorldCorners(corners);
        }
        Corners toReturn = new Corners();
        toReturn.bottomRight = corners[1];
        toReturn.topRight = corners[2];
        toReturn.topLeft = corners[3];
        toReturn.bottomLeft = corners[0];
        
        return toReturn;
    }

    public struct Corners
    {
        public Vector3 bottomRight, topRight, topLeft, bottomLeft;
    }
    
}