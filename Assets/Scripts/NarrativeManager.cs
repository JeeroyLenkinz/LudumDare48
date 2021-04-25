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
            if (timeBetweenSteps <= 0 && currentIndex < narrativeObjects.Length) {
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

    // HERE JAMES
    private IEnumerator NarrativeSteps() {
        displayStepText();
        switch (currentIndex)
        {
            case 0:
                textDisplayDuration = 7;
                timeBetweenSteps = 0;
                break;
            //Hello Cadet! Welcome aboard the SS Junk! I’m Captain Von Droop!”

            case 1:
                textDisplayDuration = 13;
                timeBetweenSteps = 0;
                break;
            //We travel deep into space to process aliens like Blumps and Grunks. They’ll fetch a pretty penny! The deeper we go, the more we’ll find!

            case 2:
                textDisplayDuration = 10;
                timeBetweenSteps = 0;
                break;
            //Your job is to receive Blumps and Grumps from the chute, process them and sort them into these bins.
            case 3:
                textDisplayDuration = 6;
                timeBetweenSteps = 0;
                break;
            //Air circulation is very important on this ship. You remember how to breathe right? 
            case 4:
                textDisplayDuration = 7;
                timeBetweenSteps = 0;
                break;
            //Watch the breath gauge! Use LEFT CTRL and RIGHT CTRL to inhale and exhale while in the Yellow zones
            //Breathe NORMAL
            case 5:
                textDisplayDuration = 5;
                timeBetweenSteps = 0;
                break;
            //Remember if you fail to breathe properly you will pass out - be careful!
            //Spawn 3 Blumbles & 3 Grunks

            case 6:
                textDisplayDuration = 10;
                timeBetweenSteps = 20;
                break;
            //Fresh Aliens incoming! Put the Purple Blumbles in the Purple Bin and the Orange Grunks in the orange bin.
            //Variable spawn rate of BlumbleB's and GrunksB's
            case 7:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                break;
            //Whoa those Blumbles need to be cut! Use the laser!
            case 8:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                break;
            //Uhh sorry cadet! Our new intern broke our oxygen valve, breathe deeper while I fix this!
            //Breathe DEEP
            case 9:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                break;
            //FIXED! You can breathe normally now - Ugh Interns are the worst sometimes.
            //Breath NORMAL
            case 10:
                textDisplayDuration = 6;
                timeBetweenSteps = 10;
                break;
            //Alright let’s go DEEPER INTO SPACE! Watch out for more aliens!
           //Increase spawn rate after transition
            case 11:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                break;
            //Ah! Those grunks are too big! Use the crusher!
            //Spawn GrunkA's
            case 12:
                textDisplayDuration = 10;
                timeBetweenSteps = 20;
                break;
            //Oh no Pirates are attacking - everyone panic and breathe quickly! Intern help me EEK! 
            //Breath QUICKLY
            case 13:
                textDisplayDuration = 9;
                timeBetweenSteps = 10;
                break;
            //Crisis solved - Not today matey! Breath normally. Onwards to Deepest Space! Expect a lot of aliens...
            //Breath NORMAL
            case 14:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                break;
            //Is that…a lavender scented candle? Nice! Everyone - Breath that in deeply!
            //Breath DEEPLY
            case 15:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                break;
            //Whoa slow down - that’s not lavender - that’s eucalyptus - my favorite!! Breath as deep as you can!
            //Breath DEEPEST
            case 16:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                break;
            //Ahh that was nice... Alright - Breath normally
            //Breath NORMAL
            case 17:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                break;
                //Great work cadet! We ventured to deepest space and cleansed the galaxy! Great job!






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