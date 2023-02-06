using UnityEngine;


public class ArtManager
{
    //The file path (within the Resources folder) to the folder
    //  where we store the tile art
    private static string TILE_ART_FOLDER_PATH = "Art/";

    //The prefix we agree on for tile art files
    private static string TILE_ART_PREFIX = "TileArt_";
    
    /// <summary>
    /// For Tiles to easily load sprites
    /// </summary>
    /// <param name="name">name of the art to load (minus prefix) e.g. Gargoyle</param>
    /// <returns>The sprite loaded</returns>
    public static Sprite LoadTileArt(string name)
    {
        string loadAt = TILE_ART_FOLDER_PATH + TILE_ART_PREFIX + name;
        Texture2D temp = Resources.Load<Texture2D>(loadAt);
        if (temp == null)
        {
            loadAt = TILE_ART_FOLDER_PATH + TILE_ART_PREFIX + "Placeholder";
            temp = Resources.Load<Texture2D>(loadAt);
        }
        return Sprite.Create(temp, new Rect(0.0f, 0.0f, temp.width, temp.height), new Vector2(0.5f, 0.5f), temp.width);
    }
}