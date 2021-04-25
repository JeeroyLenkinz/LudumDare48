using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] narrativeObjects;
    [SerializeField]
    private float initialTimeDelay;


    private int currentIndex;
    private float timeBetweenSteps;
    private float textDisplayDuration;
    private bool displayingText;
    // Start is called before the first frame update
    void Start()
    {
        timeBetweenSteps = initialTimeDelay;
        currentIndex = 0;
        displayingText = false;
        foreach(GameObject narrativeObject in narrativeObjects) {
            narrativeObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!displayingText) {
            timeBetweenSteps -= Time.deltaTime;
            if (timeBetweenSteps <= 0 && currentIndex <= narrativeObjects.Length) {
                StartCoroutine(NarrativeSteps());
            }
        }
        else {
            textDisplayDuration -= Time.deltaTime;
            if (textDisplayDuration <= 0) {
                hideStepText();
            }
        }
    }

    private IEnumerator NarrativeSteps() {
        displayStepText();
        switch (currentIndex) {
            case 0:
                textDisplayDuration = 5;
                timeBetweenSteps = 10;
                break;
        }
        yield return null;
    }

    private void displayStepText() {
        narrativeObjects[currentIndex].SetActive(true);
        displayingText = true;
    }

    private void hideStepText() {
        narrativeObjects[currentIndex].SetActive(false);
        displayingText = false;
        currentIndex++;
    }
}
