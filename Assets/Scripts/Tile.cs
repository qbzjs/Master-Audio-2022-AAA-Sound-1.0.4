using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Sprite[] tileOptions;
    public GameObject tileObject;
    

    public void Initialize() {
        int i = Random.Range(0, tileOptions.Length);
        tileObject.AddComponent<SpriteRenderer>();
        tileObject = Instantiate(tileObject,
                    transform.position,
                    Quaternion.identity);
        tileObject.GetComponent<SpriteRenderer>().sprite = tileOptions[i]; 
    }
}

