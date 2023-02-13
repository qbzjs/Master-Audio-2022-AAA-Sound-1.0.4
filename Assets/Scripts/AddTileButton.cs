using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddTileButton : MonoBehaviour
{
    private string tileName, tileDescription, tilePoints;
    [SerializeField, BoxGroup("UI references")] private TextMeshProUGUI tileNameUI, tileDescriptionUI, tilePointsUI;
    [SerializeField, BoxGroup("UI references")] private Image tileArt, border; 
    
    private Color borderColor;
    
    #region properties
    public string TileName
    {
        set
        {
            tileName = value;
            tileNameUI.text = value;
            tileArt.sprite = ArtManager.LoadTileArt(value);
        }
    }
    
    public string TileDescription
    {
        set
        {
            tileDescription = value;
            tileDescriptionUI.text = value;
        }
    }
    public string TilePoints
    {
        set
        {
            tilePoints = value;
            tilePointsUI.text = value;
        }
    }

    public Color BorderColor
    {
        set
        {
            borderColor = value;
            border.color = value;
        }
    }
    #endregion
    
    public void SetValues(string myTileName, Color myBorderColor)
    {
        string myTileDescription= "", myTilePoints = "";

        System.Type ClassType = System.Type.GetType(myTileName);
        /*
        Debug.Log("class name is: " + myTileName);
        Debug.Log("class type: " + ClassType);
        */

        ITile tile = TileFactory.CreateTile(ClassType, transform, Vector3.zero);
        Destroy(tile.TileObject);
        
        TileName = myTileName;
        TileDescription = tile.GetDescription();;
        TilePoints = myTilePoints;
        BorderColor = myBorderColor;
    }

    public void AddMyTile()
    {
        DeckManager.Instance.AddToDeck(tileName);
    }

}
