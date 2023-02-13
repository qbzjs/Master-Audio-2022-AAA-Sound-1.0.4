using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    private static int sortingOrder;
    private const float DISAPPEAR_TIMER_MAX = 1.5f;
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Vector3 moveVector;

    public void Setup(int Amount){
        string text = "";
        if (Amount > 0){
            text += "+";
        }
        text += Amount.ToString();
        textMesh.SetText(text);
        disappearTimer = DISAPPEAR_TIMER_MAX;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;
        moveVector = new Vector3(-0.6f, 1.2f);
    }

    private void Awake (){
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    private void Update() {
        transform.position += moveVector * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * 0.75f) {
            // First half of the popup lifetime
            float increaseScaleAmount = 18f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 5f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            Destroy(gameObject);
        }
    }
}
