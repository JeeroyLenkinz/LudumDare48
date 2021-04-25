using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBounce : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    
    public float duration;
    public float bounceAmount;

    public bool Hor;

    private void Start()
    {
        if (Hor)
        {
            startPos = transform.position;
            endPos = startPos + Vector2.left * bounceAmount;
        }
        else
        {
            startPos = transform.position;
            endPos = startPos + Vector2.up * bounceAmount;
        }


        transform.DOMove(endPos, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}
