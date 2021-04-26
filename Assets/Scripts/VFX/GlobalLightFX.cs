using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class GlobalLightFX : MonoBehaviour
{
    public Light2D globalLight;
    float startIntensityFloat;
    float intensityFloat;
    public bool isAnimating = false;

    public BoolReference isDark;

    // Start is called before the first frame update
    void Start()
    {
        startIntensityFloat = globalLight.intensity;
        intensityFloat = globalLight.intensity;
        isDark.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimating)
        {
            globalLight.intensity = intensityFloat;
        }

    }

    public void e_FlickerOff()
    {
        StartCoroutine(FlickerOff());
    }

    private IEnumerator FlickerOff()
    {
        isAnimating = true;
        Tween tweener = DOTween.To(() => intensityFloat, x => intensityFloat = x, 0.1f, 4f).SetEase(Ease.InOutBounce, 1f);

        yield return tweener.WaitForCompletion();

        isAnimating = false;
    }

    public void e_FlickerOn()
    {
        StartCoroutine(FlickerOn());
    }

    private IEnumerator FlickerOn()
    {
        isAnimating = true;
        Tween tweener = DOTween.To(() => intensityFloat, x => intensityFloat = x, startIntensityFloat, 4f).SetEase(Ease.InOutBounce, 1f);

        yield return tweener.WaitForCompletion();

        isAnimating = false;
    }
}
