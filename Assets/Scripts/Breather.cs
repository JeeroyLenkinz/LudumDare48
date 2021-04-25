using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Breather : MonoBehaviour
{
    private bool inhaling;
    private bool exhaling;
    private bool hasStarted;
    private bool inactive;
    private float breathMarkerPosition;
    private float displayRangeScale;
    private float startOffset;
    private RectTransform breathMarkerRT;
    private AudioSource audioSource;

    [SerializeField]
    private GameEvent breathMissEvent;
    [SerializeField]
    private GameEvent breathHitEvent;
    [SerializeField]
    private RectTransform breathZoneRT;
    [SerializeField]
    private RectTransform leftBreathBandRT;
    [SerializeField]
    private RectTransform rightBreathBandRT;
    [SerializeField]
    private float maxBreathSeconds;
    [SerializeField]
    private AudioClip inhaleSFX;
    [SerializeField]
    private AudioClip exhaleSFX;

    // Start is called before the first frame update
    void Start()
    {
        if (breathZoneRT == null || leftBreathBandRT == null || rightBreathBandRT == null) {
            Debug.LogError("No breath zone rect transform found!");
        }
        breathMarkerRT = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        inhaling = false;
        exhaling = false;
        hasStarted = false;
        breathMarkerPosition = breathZoneRT.position.x - breathZoneRT.rect.width;
        startOffset = breathMarkerRT.rect.width/2;
        displayRangeScale = breathZoneRT.rect.width - breathMarkerRT.rect.width;
        breathMarkerRT.position = new Vector2(breathMarkerPosition + startOffset, breathMarkerRT.position.y);
    }

    // Raised by InputManager.cs
    public void e_toggleInhale() {
        if (!inactive) {
            if (exhaling) {
                checkBreathHit();
                audioSource.clip = inhaleSFX;
                audioSource.Play();
            }
            hasStarted = true;
            inhaling = true;
            exhaling = false;
        }
    }

    // Raised by InputManager.cs
    public void e_toggleExhale() {
        if (!inactive) {
            if (inhaling) {
                checkBreathHit();
                audioSource.clip = exhaleSFX;
                audioSource.Play();
            }
            hasStarted = true;
            exhaling = true;
            inhaling = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // breathMarkerPosition will move between 0 and 1
        // That will then be scaled upwards to the desired range on screen
        if (inhaling) {
            breathMarkerPosition += Time.deltaTime/maxBreathSeconds;
        }
        else if (exhaling) {
            breathMarkerPosition -= Time.deltaTime/maxBreathSeconds;
        }
        else if (hasStarted) {
            Debug.LogError("You are neither exhaling nor inhaling!!!");
        }

        breathMarkerPosition = Mathf.Clamp(breathMarkerPosition, 0, 1);
        float displayPosition = breathMarkerPosition * displayRangeScale;
        breathMarkerRT.position = new Vector2(displayPosition + startOffset, breathMarkerRT.position.y);

        if (inactive) {
            float markPosX = breathMarkerRT.position.x;
            if (inhaling && markPosX >= breathZoneRT.position.x) {
                inactive = false;
            }
            else if (exhaling && markPosX <= breathZoneRT.position.x) {
                inactive = false;
            }
        }

        if (hasStarted && (breathMarkerPosition >=1 || breathMarkerPosition <= 0)) {
            fullMiss();
        }
    }

    private void checkBreathHit() {
        float markerLeftBound = breathMarkerRT.position.x - (breathMarkerRT.rect.width/2);
        float markerRightBound = breathMarkerRT.position.x + (breathMarkerRT.rect.width/2);

        float leftTargetLeftBound = leftBreathBandRT.position.x - (leftBreathBandRT.rect.width/2);
        float leftTargetRightBound = leftBreathBandRT.position.x + (leftBreathBandRT.rect.width/2);

        float rightTargetLeftBound = rightBreathBandRT.position.x - (rightBreathBandRT.rect.width/2);
        float rightTargetRightBound = rightBreathBandRT.position.x + (rightBreathBandRT.rect.width/2);

        // Check the right target while inhaling (moving right)
        if (inhaling) {
            if (markerLeftBound <= rightTargetRightBound && markerRightBound >= rightTargetLeftBound) {
                Debug.Log("Score! You got in the right target!");
                breathHitEvent.Raise();
            }
            else {
                Debug.Log("Uh oh you missed the right target!");
                breathMissEvent.Raise();
            }
        }
        // Check the left target while exhaling (moving left)
        else if (exhaling) {
            if (markerLeftBound <= leftTargetRightBound && markerRightBound >= leftTargetLeftBound) {
              Debug.Log("Score! You got in the left target!");
              breathHitEvent.Raise();
            }
            else {
                Debug.Log("Uh oh you missed the left target!");
                breathMissEvent.Raise();
            }
        }
        else if (hasStarted) {
            Debug.LogError("You are neither exhaling nor inhaling!!!");
        }
    }

    private void fullMiss() {
        if (inhaling) {
            e_toggleExhale();
        }
        else if (exhaling) {
            e_toggleInhale();
        }
        else {
            Debug.LogError("You are neither exhaling nor inhaling!!!");
            return;
        }
        // Give the player a grace period after a full miss so they dont accidentally hit right after
        inactive = true;
    }
}