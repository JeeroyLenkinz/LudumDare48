using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class NarrativeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] narrativeObjects;
    [SerializeField]
    private float initialTimeDelay;
    [SerializeField]
    private IntGameEvent changeDifficultyEvent;
    [SerializeField]
    private IntGameEvent setAlienLimitEvent;
    [SerializeField]
    private FloatGameEvent setSpawnMultiplierEvent;
    [SerializeField]
    private IntGameEvent addAlienTypeEvent;
    [SerializeField]
    private Vector2GameEvent spawnAlienEvent;
    [SerializeField]
    private BoolGameEvent EmergencySirenEvent;
    [SerializeField]
    private IntGameEvent CameraShakeEvent;
    [SerializeField]
    private GameEvent FlickerOn;
    [SerializeField]
    private GameEvent FlickerOff;
    [SerializeField]
    private AudioClip intercomSFX;

    private enum difficulty {Deepest, Deep, Normal, Quick};
    private enum alienType {Circle, CrushedSquare, CirclePair, CircleTrio, LongSquare};
    private enum sfx {Siren, Oxygen, HyperSpace, Intercom};

    private int currentIndex;
    private float timeBetweenSteps;
    private float textDisplayDuration;
    private bool displayingText;
    private bool playInterComSound = true;
    private AudioSource intercomAudioSource;
    private AudioSource sirenAudioSource;
    private AudioSource oxygenAudioSource;
    private AudioSource hyperSpaceAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        intercomAudioSource = GetComponent<AudioSource>();
        sirenAudioSource = gameObject.transform.Find("SirenSFX").GetComponent<AudioSource>();
        oxygenAudioSource = gameObject.transform.Find("OxygenSFX").GetComponent<AudioSource>();
        hyperSpaceAudioSource = gameObject.transform.Find("HyperSpaceSFX").GetComponent<AudioSource>();
        timeBetweenSteps = initialTimeDelay;
        currentIndex = 1;
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
        playInterComSound = true;
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
                playInterComSound = false;
                break;
            //Remember if you fail to breathe properly you will pass out - be careful!
            //Spawn 3 Blumbles & 3 Grunks

            case 6:
                textDisplayDuration = 10;
                timeBetweenSteps = 20;

                CameraShakeEvent.Raise(0);
                EmergencySirenEvent.Raise(true);

                playSFX(sfx.Siren);

                spawnAlien(alienType.Circle, 3);
                spawnAlien(alienType.CrushedSquare, 3);
                setAlienLimitEvent.Raise(10);
                setSpawnMultiplierEvent.Raise(1f);
                break;
            //Fresh Aliens incoming! Put the Purple Blumbles in the Purple Bin and the Orange Grunks in the orange bin.
            //Variable spawn rate of BlumbleB's and GrunksB's
            case 7:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                EmergencySirenEvent.Raise(false);
                CameraShakeEvent.Raise(3);
                stopSFX(sfx.Siren);
                break;
            //Whoa those Blumbles need to be cut! Use the laser!
            //Summon BlumbleA's
            case 8:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                changeDifficulty(difficulty.Deep);
                break;
            //Uhh sorry cadet! Our new intern broke our oxygen valve, breathe deeper while I fix this!
            //Breathe DEEP
            case 9:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                changeDifficulty(difficulty.Normal);
                break;
            //FIXED! You can breathe normally now - Ugh Interns are the worst sometimes.
            //Breath NORMAL
            case 10:
                textDisplayDuration = 6;
                timeBetweenSteps = 10;
                CameraShakeEvent.Raise(1);
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
                changeDifficulty(difficulty.Quick);
                FlickerOff.Raise();
                EmergencySirenEvent.Raise(true);
                break;
            //Oh no Pirates are attacking - everyone panic and breathe quickly! Intern help me EEK! 
            //Breath QUICKLY
            case 13:
                textDisplayDuration = 9;
                timeBetweenSteps = 10;
                changeDifficulty(difficulty.Normal);
                FlickerOn.Raise();
                EmergencySirenEvent.Raise(false);
                CameraShakeEvent.Raise(2);
                break;
            //Crisis solved - Not today matey! Breath normally. Onwards to Deepest Space! Expect a lot of aliens...
            //Breath NORMAL
            case 14:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                changeDifficulty(difficulty.Deep);
                break;
            //Is that…a lavender scented candle? Nice! Everyone - Breath that in deeply!
            //Breath DEEPLY
            case 15:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                changeDifficulty(difficulty.Deepest);
                break;
            //Whoa slow down - that’s not lavender - that’s eucalyptus - my favorite!! Breath as deep as you can!
            //Breath DEEPEST
            case 16:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                changeDifficulty(difficulty.Normal);
                break;
            //Ahh that was nice... Alright - Breath normally
            //Breath NORMAL
            case 17:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                break;
            //Great work cadet! We ventured to deepest space and cleansed the galaxy! Great job!
        }
        if (playInterComSound) {
            playSFX(sfx.Intercom);
        }
        yield return null;
    }

    private void changeDifficulty(difficulty diff) {
        // 0 = Slowest/Deepest, 1 = Slow/Deep, 2 = Medium, 3 = Fast/Shallow
        int diffInt;
        switch (diff) {
            case difficulty.Deepest:
                diffInt = 0;
                break;
            case difficulty.Deep:
                diffInt = 1;
                break;
            case difficulty.Normal:
                diffInt = 2;
                break;
            case difficulty.Quick:
                diffInt = 3;
                break;
            default:
                diffInt = 2;
                break;
        }
        changeDifficultyEvent.Raise(diffInt);
    }

    private void spawnAlien(alienType type, int amount) {
        int typeInt = 0;
        switch (type) {
            case alienType.Circle:
                typeInt = 0;
                break;
            case alienType.CrushedSquare:
                typeInt = 1;
                break;
            case alienType.CirclePair:
                typeInt = 2;
                break;
            case alienType.CircleTrio:
                typeInt = 3;
                break;
            case alienType.LongSquare:
                typeInt = 04;
                break;
        }
        Vector2 spawnVector = new Vector2(typeInt, amount);
        spawnAlienEvent.Raise(spawnVector);
    }

    private void playSFX(sfx sfxType) {
        switch (sfxType) {
            case sfx.Intercom:
                intercomAudioSource.Play();
                break;
            case sfx.Oxygen:
                oxygenAudioSource.Play();
                break;
            case sfx.Siren:
                sirenAudioSource.Play();
                break;
            case sfx.HyperSpace:
                hyperSpaceAudioSource.Play();
                break;
        }
    }

        private void stopSFX(sfx sfxType) {
        switch (sfxType) {
            case sfx.Intercom:
                intercomAudioSource.Stop();
                break;
            case sfx.Oxygen:
                oxygenAudioSource.Stop();
                break;
            case sfx.Siren:
                sirenAudioSource.Stop();
                break;
            case sfx.HyperSpace:
                hyperSpaceAudioSource.Stop();
                break;
        }
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