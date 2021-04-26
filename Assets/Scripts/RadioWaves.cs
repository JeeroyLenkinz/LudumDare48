using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioWaves : MonoBehaviour
{

    Animate_Basic animBasic;
    public GameObject radioWaveParent;

    private void Start()
    {
        animBasic = GetComponent<Animate_Basic>();
    }

    public void e_GrowRadio()
    {
        foreach(Transform child in radioWaveParent.transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255f);
        }
        animBasic.Animate(AnimationTweenType.Scale, Vector2.zero, Vector2.zero);
    }

    public void e_ShrinkRadio()
    {
        animBasic.Animate(AnimationTweenType.FadeRadio, Vector2.zero, Vector2.zero);
        //Debug.Log("RadioFade");
    }

}
