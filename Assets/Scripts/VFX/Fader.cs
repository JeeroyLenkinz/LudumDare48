using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    Image image;
    bool isAnimating = false;

    [SerializeField]
    private Color blackColor;

    [SerializeField]
    private Color seeThroughColor;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    private IEnumerator fadeAnimateIn()
    {
        isAnimating = true;
        Color tempColor = blackColor;
        Debug.Log("Start");

        while (tempColor.a > 0.01f)
        {
            Debug.Log(tempColor.a.ToString());


            tempColor = Color.Lerp(tempColor, seeThroughColor, Time.deltaTime * 2);
            image.color = new Color(0f, 0f, 0f, tempColor.a);
            if (tempColor.a < 0.05f)
            {
                image.color = new Color(0f, 0f, 0f, 0f);
            }
            yield return null;
        }



        yield return null;
        isAnimating = false;

    }

    private IEnumerator fadeAnimateOut()
    {
        isAnimating = true;
        Color tempColor = seeThroughColor;

        while (tempColor.a < 0.99f)
        {
            //Debug.Log(timer.ToString());


            tempColor = Color.Lerp(tempColor, blackColor, Time.deltaTime * 2);
            image.color = new Color(0f, 0f, 0f, tempColor.a);
            if (tempColor.a > 0.95f)
            {
                image.color = new Color(0f, 0f, 0f, 255f);
            }
            yield return null;
        }



        yield return null;
        isAnimating = false;

    }

    public void e_FadeIn()
    {
        if (!isAnimating)
        {
            StartCoroutine(fadeAnimateIn());
        }

    }

    public void e_FadeOut()
    {
        if (!isAnimating)
        {
            StartCoroutine(fadeAnimateOut());
        }

    }


}
