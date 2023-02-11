using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTile: MonoBehaviour
{
    [SerializeField] public ITile Tile;
    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.scoretip.Hide();
        }else{
            Debug.Log("here");
            GameManager.Instance.scoretip.Show(Tile); 
        }
    }
    public void OnMouseDown()
    {
        GameManager.Instance.scoretip.Hide();
    //    GameManager.Instance.tooltip.Show(Tile);  
    }
    public void OnMouseUp()
    {
    //    GameManager.Instance.tooltip.Hide();

    }
    public void OnMouseExit()
    {
        GameManager.Instance.scoretip.Hide();
    }
}