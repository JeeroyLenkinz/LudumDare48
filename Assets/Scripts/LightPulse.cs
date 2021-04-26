using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class LightPulse : MonoBehaviour
{
    public Light2D pointLight;
    float startIntensityFloat;
    float intensityFloat;

    private void Start()
    {
        pointLight.intensity = 0.5f;
        Tween tweener = DOTween.To(() => intensityFloat, x => intensityFloat = x, 0.8f, 1.5f).SetEase(Ease.InOutSine, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        pointLight.intensity = intensityFloat;
    }

}
