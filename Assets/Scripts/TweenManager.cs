using System;
using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using NaughtyAttributes;
using Scripts;
using TMPro;
using UnityEngine;
 using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TweenManager : Singleton<TweenManager>
{
    [Range(0, 2)]
    [SerializeField] private float cameraShakeAmount, cameraShakeTime, blockUpAmount, blockAnimationTime, emphasizeAmount, emphasizeTime, slideOnTime;

    [SerializeField] private AnimationCurve blockCurve;
    [SerializeField] private Vector3 slideOnDiff;
    
    [SerializeField, BoxGroup("callout")] private float calloutTime, calloutMoveAmount;
    [SerializeField, BoxGroup("callout")] private Vector2 calloutStartSize, calloutEndSize;
    [SerializeField, BoxGroup("callout")] private LeanTweenType calloutScaleEase, calloutMoveEase;
    [SerializeField, BoxGroup("callout")] private GameObject calloutPrefab, calloutParent;
    
    public GameObject cardBack, cardParticles;
    public Transform cardStart, cardStop, cardMiddle1, cardMiddle2;
    public float cardRandomness;

    [SerializeField, BoxGroup("waiting")] private float waitTime = 0.2f, waitTimeMultiplier;
    
    [SerializeField, BoxGroup("tileEffects")] private GameObject destroyEffectPrefab, createEffectPrefab, transformEffectPrefab;
    [SerializeField, BoxGroup("tileEffects")] private float bigSize, smallSize, effectTweenTime;
    [SerializeField, BoxGroup("tileEffects")] private LeanTweenType effectEase;



    private GameObject mainCanvas;

    private Queue<Action> tweenQueue = new();
    private int consecutiveActions = 0;
    private float adjustedWaitTime;

    private void Awake()
    {
        adjustedWaitTime = waitTime;
        mainCanvas = FindObjectOfType<Canvas>().gameObject;
        StartCoroutine(CheckNextRecursive());
    }

    public void AddCallbackToQueue(Action CB)
    {
        tweenQueue.Enqueue(CB);
    }

    /*
    [Button()]
    public void TestPlaceBlock()
    {
        PlaceBlock(FindObjectOfType<Block>().gameObject);
    }
    */

    private IEnumerator CheckNextRecursive()
    {
        yield return new WaitForSeconds(adjustedWaitTime);
        Action toInvoke;
        if (tweenQueue.TryDequeue(out toInvoke))
        {
            toInvoke.Invoke();
            consecutiveActions++;
            adjustedWaitTime *= waitTimeMultiplier;
        }
        else
        {
            adjustedWaitTime = waitTime;
            consecutiveActions = 0;
        }

        StartCoroutine(CheckNextRecursive());
    }

    public void DrawCard(Action callback = null)
    {
        GameObject card = Instantiate(cardBack, cardStart);
        Vector3[] positions = new []{cardStart.position, cardMiddle1.position, cardMiddle2.position, cardStop.position};
        for (int i = 1; i < positions.Length; i++)
        {
            positions[i] = positions[i] +
                           new Vector3(Random.Range(0, cardRandomness), Random.Range(0, cardRandomness), 0);
        }
        LTSpline spline = new LTSpline(new[]{positions[0], positions[0], positions[1], positions[2], positions[3], positions[3]});
        //spline.
        card.transform.localPosition = Vector3.zero;
        //card.transform.localScale = Vector3.zero;
        LeanTween.moveSpline(card, spline, 1f).setEaseInOutCirc();
        LeanTween.scale(card, Vector3.one * 1.5f, 1f).setEaseInOutCirc()
            .setOnComplete(() =>
            {
                if (callback != null)
                {
                    callback.Invoke();
                }

                MasterAudio.PlaySound("Poof");
                Instantiate(cardParticles, card.transform.position, Quaternion.identity);
                Destroy(card, 0.01f);
            });
    }
    
    public void SlideOnLocal(GameObject ob)
    {
        if (LeanTween.isTweening(ob)) return;
        Vector3 endPos = ob.transform.localPosition;
        ob.transform.localPosition = endPos + slideOnDiff;
        LeanTween.moveLocal(ob, endPos, slideOnTime)
            .setEaseOutBounce();
        CanvasGroup canvas = ob.GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            canvas = ob.AddComponent<CanvasGroup>();
        }

        canvas.alpha = 0.5f;
        LeanTween.alphaCanvas(canvas, 1, slideOnTime);
    }
    
    public void PlaceBlock(GameObject ob, Action CB)
    {
        foreach (var sp in ob.GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sortingOrder = 20;
        }

        Move(ob);
        Shake();
        CB();
        foreach (var sp in ob.GetComponentsInChildren<SpriteRenderer>())
        {
            sp.sortingOrder = -100;
        }
    }

    public void Callout(string displayText, Vector2Int location)
    {
        tweenQueue.Enqueue(() => CalloutHelper(displayText, GridManager.Instance.GridToWorldPos(location)));
    }
    
    public void Callout(string displayText, Vector3 location)
    {
        tweenQueue.Enqueue(() => CalloutHelper(displayText, location));
    }

    public void CalloutHelper(string displayText, Vector3 location)
    {
        GameObject newOb = Instantiate(calloutPrefab, location, Quaternion.identity, calloutParent.transform);
        TextMeshProUGUI newTextMesh = newOb.GetComponentInChildren<TextMeshProUGUI>();
        newTextMesh.text = displayText;
        newOb.transform.localScale = calloutStartSize;
        LeanTween.scale(newOb, calloutEndSize, calloutTime)
            .setEase(calloutScaleEase);
        LeanTween.moveY(newOb, location.y + calloutMoveAmount, calloutTime)
            .setEase(calloutMoveEase)
            .setOnComplete(() =>
            {
                LeanTween.alphaCanvas(newOb.GetComponent<CanvasGroup>(), 0, 1)
                    .setEaseOutCubic()
                    .setOnComplete(() => Destroy(newOb, 0.1f));
            });
    }

    [Button()]
    public void Shake()
    {
        LeanTween.moveY(Camera.main.gameObject, Camera.main.transform.position.x - cameraShakeAmount, cameraShakeTime)
            .setEase(LeanTweenType.easeShake).setDelay(0.1f);
    }

    public void Emphasize(GameObject ob, float multiplier = 1)
    {
        LeanTween.scale(ob, Vector3.one * emphasizeAmount * multiplier, emphasizeTime)
            .setEasePunch();
    }


    public void Move(GameObject ob)
    {
        LeanTween.moveLocalY(ob, ob.transform.localPosition.y + blockUpAmount, blockAnimationTime)
        .setEase(blockCurve);   
        
    }

    public Transform moveStop, moveMiddle;
    
    public void PlaceBlockCards(Block block)
    {
        Transform moveStart = block.gameObject.transform;

        for (int j = 0; j < block.Tiles.Count; j++) {
            GameObject card = Instantiate(cardBack, moveMiddle);
            card.SetActive(false);
            LeanTween.move(card, moveStart.position, 0.000001f);
            card.SetActive(true);

            Vector3 mid = (moveStart.position + moveStop.position)/2 ;
            Vector3[] positions = new []{moveStart.position, mid, moveMiddle.position, moveStop.position};
            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i] +
                            new Vector3(Random.Range(0, 2), Random.Range(0, 2), 0);
            } 
            LTSpline spline = new LTSpline(new[]{positions[0], positions[0], positions[1], positions[2], positions[3], positions[3]});
            card.transform.localPosition = Vector3.zero;
            LeanTween.moveSpline(card, spline, 1.25f).setEaseInSine();
            LeanTween.scale(card, Vector3.one, 1.25f).setEaseInSine()
                .setOnComplete(() =>
                {
                    Destroy(card, 0.01f);
                }); 
        }

    }
    public void MoveCard(Transform startPos, Transform endPos, int size)
    {
        for (int i = 0; i < size; i++){
            GameObject card = Instantiate(cardBack, startPos);
            card.transform.localPosition = Vector3.zero;
            float f = 1 - (i * 0.3f);
            LeanTween.move(card, endPos, 0.6f).setEaseInBack().setDelay(f).setOnComplete(()=>
            {
                Destroy(card, f);
            });
        } 
    }

    public void DestroyEffect(Vector2Int location, Action CB)
    {
        tweenQueue.Enqueue(() => DestroyEffectHelper(GridManager.Instance.GridToWorldPos(location), CB));
    }

    public void DestroyEffectHelper(Vector3 location, Action CB)
    {
        GameObject newOb =  Instantiate(destroyEffectPrefab, location, Quaternion.identity);
        newOb.transform.localScale = GridManager.Instance.GridUnit * Vector3.one * bigSize;
        LeanTween.scale(newOb, Vector3.one * smallSize, effectTweenTime)
            .setEase(effectEase)
            .setOnComplete(()=>
            {
                CB.Invoke();
                Destroy(newOb, 0.3f);
            });
    }
    
    public void CreateEffect(Vector2Int location, Action CB)
    {
        tweenQueue.Enqueue(() => CreateEffectHelper(GridManager.Instance.GridToWorldPos(location), CB));
    }

    public void CreateEffectHelper(Vector3 location, Action CB)
    {
        GameObject newOb =  Instantiate(createEffectPrefab, location, Quaternion.identity);
        newOb.transform.localScale = GridManager.Instance.GridUnit * Vector3.one * bigSize;
        LeanTween.rotateAround(newOb, Vector3.forward, 360, effectTweenTime)
            .setEase(effectEase);
        LeanTween.scale(newOb, Vector3.one * smallSize, effectTweenTime)
            .setEase(effectEase)
            .setOnComplete(()=>
            {
                CB.Invoke();
                Destroy(newOb, 0.3f);
            });
    }
    
    public void TransformEffect(Vector2Int location, Action CB)
    {
        tweenQueue.Enqueue(() => TransformEffectHelper(GridManager.Instance.GridToWorldPos(location), CB));
    }

    public void TransformEffectHelper(Vector3 location, Action CB)
    {
        GameObject newOb =  Instantiate(transformEffectPrefab, location, Quaternion.identity);
        newOb.transform.localScale = GridManager.Instance.GridUnit * Vector3.one * smallSize;
        LeanTween.scale(newOb, Vector3.one * bigSize, effectTweenTime)
            .setEaseOutElastic()
            .setOnComplete(()=>
            {
                CB.Invoke();
                Destroy(newOb, 0.3f);
            });
    }

    public void Reveal(GameObject obj)
    {
        Instantiate(cardParticles, obj.transform.position, Quaternion.identity);
    }

}
