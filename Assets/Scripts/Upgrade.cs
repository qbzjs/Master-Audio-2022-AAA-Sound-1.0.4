using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private string type = "";
    // Start is called before the first frame update
    void Start()
    {
        RefreshUpgrade();
    }

    public void SetUpgrade()
    {
        GameManager.Instance.upgrading = type;
    }

    public void RefreshUpgrade()
    {
        int rand = Random.Range(0, 7); //TODO un-magic-number this=

        switch (rand)
        {
            case 0:
                type = "GA";
                GetComponent<Image>().sprite = ArtManager.Instance.gargoyleArt;
                break;
            case 1:
                type = "MA";
                GetComponent<Image>().sprite = ArtManager.Instance.mansionArt;
                break;
            case 2:
                type = "TE";
                GetComponent<Image>().sprite = ArtManager.Instance.tenementArt;
                break;
            case 3:
                type = "RI";
                GetComponent<Image>().sprite = ArtManager.Instance.riverArt;
                break;
            case 4:
                type = "CH";
                GetComponent<Image>().sprite = ArtManager.Instance.churchArt;
                break;
            case 5:
                type = "WI";
                GetComponent<Image>().sprite = ArtManager.Instance.churchWingArt;
                break;
            case 6:
                type = "GR";
                GetComponent<Image>().sprite = ArtManager.Instance.graveyardArt;
                break;
            default:
                type = "GA";
                GetComponent<Image>().sprite = ArtManager.Instance.gargoyleArt;
                break;
        }
    }
}
