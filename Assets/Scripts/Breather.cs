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
    private bool difficultyChanging;
    private float breathMarkerPosition;
    private float displayRangeScale;
    private float startOffset;
    private Vector2 leftBandDestination;
    private Vector2 rightBandDestination;
    private RectTransform breathMarkerRT;
    private AudioSource audioSource;
    private float bandChangeTolerance = 0.05f;

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
    private float easyMultiplier;
    [SerializeField]
    private float mediumMultiplier;
    [SerializeField]
    private float hardMultiplier;
    [SerializeField, Range(0.01f, 0.1f)]
    private float bandChangeSmoothing;
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
        difficultyChanging = false;
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

    public void e_changeBreathBands(int difficulty) {
        float distance = (breathZoneRT.rect.width - leftBreathBandRT.rect.width)/2;
        switch (difficulty) {
            case 0: // Easy (aka long, slow breaths)
                distance *= easyMultiplier;
                break;
            case 1: // Medium
                distance *= mediumMultiplier;
                break;
            case 2: // Hard (aka short, fast breaths)
                distance *= hardMultiplier;
                break;
            default:
                return;
        }
        if (!difficultyChanging) {
            leftBandDestination = new Vector2((breathZoneRT.position.x - distance), leftBreathBandRT.position.y);
            rightBandDestination = new Vector2((breathZoneRT.position.x + distance), rightBreathBandRT.position.y);
            difficultyChanging = true;
        }
    }

    void Update()
    {
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

        if (difficultyChanging) {
            leftBreathBandRT.position = Vector2.Lerp(leftBreathBandRT.position, leftBandDestination, bandChangeSmoothing);
            rightBreathBandRT.position = Vector2.Lerp(rightBreathBandRT.position, rightBandDestination, bandChangeSmoothing);
            bool leftDone = (leftBreathBandRT.position.x >= (leftBandDestination.x - bandChangeTolerance)) && (leftBreathBandRT.position.x <= (leftBandDestination.x + bandChangeTolerance));
            bool rightDone = (rightBreathBandRT.position.x >= (rightBandDestination.x - bandChangeTolerance)) && (rightBreathBandRT.position.x <= (rightBandDestination.x + bandChangeTolerance));
            if (leftDone || rightDone) {
                leftBreathBandRT.position = leftBandDestination;
                rightBreathBandRT.position = rightBandDestination;
                difficultyChanging = false;
            }
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