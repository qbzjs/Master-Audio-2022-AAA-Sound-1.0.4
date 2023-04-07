using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkButton : MonoBehaviour
{
    [Header("Enter URL for button to go to")]
    public string linkURL;

    public void GoToLink()
    {
        Application.OpenURL(linkURL);
    }
}
