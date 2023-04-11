using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI)), ExecuteAlways]
public class VersionNumber : MonoBehaviour
{
    public TextMeshProUGUI myText;
    void Start()
    {
        myText.text = ("V. " + Application.version);
    }
}
