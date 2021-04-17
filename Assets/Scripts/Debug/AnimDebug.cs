using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AnimDebug : MonoBehaviour
{

    public GameObject bushStart;

    private bool isAnimating;

    public void b_StartRace()
    {
        if (!isAnimating)
        {
            StartCoroutine(AnimateRace());
        }

    }

    private IEnumerator AnimateRace() 
    {
        Animate_Basic bushAnimator = GetComponent<Animate_Basic>();
        Tween tempTweener;
        isAnimating = true;

        tempTweener = bushAnimator.Animate(AnimationTweenType.Move, bushStart.transform.position, Vector2.zero);
        yield return new WaitForSeconds(tempTweener.Duration());
        tempTweener = bushAnimator.Animate(AnimationTweenType.Scale, Vector2.zero, Vector2.zero);
        //tempTweener = bushAnimator.Animate(AnimationTweenType.RotateZ, Vector2.zero, Vector2.zero);

        yield return new WaitForSeconds(tempTweener.Duration());

        isAnimating = false;
        // Raise event or something
    }

}
