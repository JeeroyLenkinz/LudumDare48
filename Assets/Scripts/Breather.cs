using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breather : MonoBehaviour
{
    private bool inhaling;
    private bool exhaling;
    private float breathMarkerPosition;
    private float displayRangeScale;
    private float startOffset = 10;
    private RectTransform breathMarkerRectTransform;


    public RectTransform breathZoneRectTransform;     
    public float maxBreathSeconds;
    public float failBreathRangeWidth;
    public float targetBreathRangeWidth;


    // Start is called before the first frame update
    void Start()
    {
        if (breathZoneRectTransform == null) {
            Debug.LogError("No breath zone rect transform found!");
        }
        breathMarkerRectTransform = GetComponent<RectTransform>();
        inhaling = false;
        exhaling = false;
        breathMarkerPosition = 0;
        displayRangeScale = breathZoneRectTransform.rect.width - breathMarkerRectTransform.rect.width;
        breathMarkerRectTransform.position = new Vector2(breathMarkerPosition + startOffset, breathMarkerRectTransform.position.y);
    }

    public void e_toggleInhale() {
        inhaling = true;
        exhaling = false;
    }

    public void e_toggleExhale() {
        exhaling = true;
        inhaling = false;
    }

    // Update is called once per frame
    void Update()
    {
        // breathMarkerPosition will move between 0 and maxBreathRangeWidth
        // On either end there is a fail range
        // Inside of that there is a target range that you want to hit
        if (inhaling) {
            breathMarkerPosition += Time.deltaTime/maxBreathSeconds;
        }
        else if (exhaling) {
            breathMarkerPosition -= Time.deltaTime/maxBreathSeconds;
        }
        breathMarkerPosition = Mathf.Clamp(breathMarkerPosition, 0, 1);
        float displayPosition = breathMarkerPosition * displayRangeScale;
        breathMarkerRectTransform.position = new Vector2(displayPosition + startOffset, breathMarkerRectTransform.position.y);
    }

    void checkBreathHit() {

    }
}