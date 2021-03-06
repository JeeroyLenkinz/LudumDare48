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
    private bool fullyMissed;
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
    private RectTransform breathCapRT;
    [SerializeField]
    private RectTransform leftBreathBandRT;
    [SerializeField]
    private RectTransform rightBreathBandRT;
    [SerializeField]
    private float maxBreathSeconds;
    [SerializeField]
    private float slowestMultiplier;
    [SerializeField]
    private float slowMultiplier;
    [SerializeField]
    private float mediumMultiplier;
    [SerializeField]
    private float fastMultiplier;
    [SerializeField, Range(0.01f, 0.1f)]
    private float bandChangeSmoothing;
    [SerializeField]
    private AudioClip inhaleSFX;
    [SerializeField]
    private AudioClip exhaleSFX;
    [SerializeField]
    private AudioClip gaspSFX;

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
        fullyMissed = false;
        inactive = false;
        breathMarkerPosition = 0;
        startOffset = breathZoneRT.position.x - (breathZoneRT.rect.width/2) + (breathMarkerRT.rect.width/2) + breathCapRT.rect.width;
        displayRangeScale = breathZoneRT.rect.width - breathMarkerRT.rect.width - (breathCapRT.rect.width*2);
        breathMarkerRT.position = new Vector2(breathMarkerPosition + startOffset, breathMarkerRT.position.y);
        e_changeBreathBands(2);
    }

    // Raised by InputManager.cs
    public void e_toggleInhale() {
        if (!inactive) {
            if (exhaling) {
                checkBreathHit();
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
            }
            hasStarted = true;
            exhaling = true;
            inhaling = false;
        }
    }

    public void e_safetyStartInhale() {
        if (!hasStarted) {
            e_toggleInhale();
        }
    }

    public void e_changeBreathBands(int difficulty) {
        float distance = (breathZoneRT.rect.width - leftBreathBandRT.rect.width - (2*breathCapRT.rect.width))/2;
        switch (difficulty) {
            case 0: // Easiest/Deepest (aka long, slow breaths)
                distance *= slowestMultiplier;
                break;
            case 1: // Easy/Deep
                distance *= slowMultiplier;
                break;
            case 2: // Medium
                distance *= mediumMultiplier;
                break;
            case 3: // Hard (aka short, fast breaths)
                distance *= fastMultiplier;
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
        if (fullyMissed) {
            audioSource.clip = gaspSFX;
            audioSource.Play();
            breathMissEvent.Raise();
            fullyMissed = false;
            return;
        }
        float markerLeftBound = breathMarkerRT.position.x - (breathMarkerRT.rect.width/2);
        float markerRightBound = breathMarkerRT.position.x + (breathMarkerRT.rect.width/2);

        float leftTargetLeftBound = leftBreathBandRT.position.x - (leftBreathBandRT.rect.width/2);
        float leftTargetRightBound = leftBreathBandRT.position.x + (leftBreathBandRT.rect.width/2);

        float rightTargetLeftBound = rightBreathBandRT.position.x - (rightBreathBandRT.rect.width/2);
        float rightTargetRightBound = rightBreathBandRT.position.x + (rightBreathBandRT.rect.width/2);

        // Check the right target while inhaling (moving right)
        if (inhaling) {
            if (markerLeftBound <= rightTargetRightBound && markerRightBound >= rightTargetLeftBound) {
                audioSource.clip = exhaleSFX;
                breathHitEvent.Raise();
            }
            else {
                audioSource.clip = gaspSFX;
                
                breathMissEvent.Raise();
            }
        }
        // Check the left target while exhaling (moving left)
        else if (exhaling) {
            if (markerLeftBound <= leftTargetRightBound && markerRightBound >= leftTargetLeftBound) {
                audioSource.clip = inhaleSFX;
                breathHitEvent.Raise();
            }
            else {
                audioSource.clip = gaspSFX;
                breathMissEvent.Raise();
            }
        }
        else if (hasStarted) {
            Debug.LogError("You are neither exhaling nor inhaling!!!");
        }
        audioSource.Play();
    }

    private void fullMiss() {
        if (inhaling) {
            fullyMissed = true;
            e_toggleExhale();
        }
        else if (exhaling) {
            fullyMissed = true;
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