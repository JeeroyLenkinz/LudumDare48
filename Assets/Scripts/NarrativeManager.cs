using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;

public class NarrativeManager : MonoBehaviour
{
    [SerializeField]
    GameEvent fadeOut;
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
    private GameEvent displayTextEvent;
    [SerializeField]
    private GameEvent hideTextEvent;
    [SerializeField]
    private GameEvent startInhaleEvent;

    private enum difficulty {Deepest, Deep, Normal, Quick};
    private enum alienType {Circle, CrushedSquare, CirclePair, CircleTrio, LongSquare};
    private enum sfx {Siren, Oxygen, HyperSpace, Intercom, PirateBattle};

    private int currentIndex;
    private float timeBetweenSteps;
    private float textDisplayDuration;
    private bool displayingText;
    private bool playInterComSound = true;
    private bool narrativeComplete = false;
    private bool gameEndTriggered = false;
    private bool noDisplayRaise = false;
    private bool noHideRaise = false;
    private AudioSource intercomAudioSource;
    private AudioSource sirenAudioSource;
    private AudioSource oxygenAudioSource;
    private AudioSource hyperSpaceAudioSource;
    private AudioSource pirateBattleAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        intercomAudioSource = GetComponent<AudioSource>();
        sirenAudioSource = gameObject.transform.Find("SirenSFX").GetComponent<AudioSource>();
        oxygenAudioSource = gameObject.transform.Find("OxygenSFX").GetComponent<AudioSource>();
        hyperSpaceAudioSource = gameObject.transform.Find("HyperSpaceSFX").GetComponent<AudioSource>();
        pirateBattleAudioSource = gameObject.transform.Find("PirateBattleSFX").GetComponent<AudioSource>();
        timeBetweenSteps = initialTimeDelay;
        currentIndex = 0;
        displayingText = false;
        narrativeComplete = false;
        gameEndTriggered = false;
        noDisplayRaise = false;
        foreach(GameObject narrativeObject in narrativeObjects) {
            narrativeObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEndTriggered) {
            if (!displayingText) {
                timeBetweenSteps -= Time.deltaTime;
                if (timeBetweenSteps <= 0 && narrativeComplete) {
                    StartCoroutine(gameComplete());
                }
                else if (timeBetweenSteps <= 0 && currentIndex < narrativeObjects.Length) {
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
                noHideRaise = true;
                noDisplayRaise = true;
                break;
            //Hello Cadet! Welcome aboard the SS Junk! I’m Captain Von Droop!”

            case 1:
                textDisplayDuration = 13;
                timeBetweenSteps = 0;
                noHideRaise = true;
                noDisplayRaise = true;
                break;
            //We travel deep into space to process aliens like Blumps and Grunks. They’re valuale and pretty cute! The deeper we go, the more we’ll find!

            case 2:
                textDisplayDuration = 9;
                timeBetweenSteps = 0;
                noHideRaise = true;
                noDisplayRaise = true;
                break;
            //Your job is to receive Blumps and Grumps from the chute, process them and sort them into these bins.
            case 3:
                textDisplayDuration = 6;
                timeBetweenSteps = 0;
                noHideRaise = true;
                noDisplayRaise = true;
                break;
            //Air circulation is very important on this ship. You remember how to breathe right? 
            case 4:
                textDisplayDuration = 9;
                timeBetweenSteps = 0;
                noHideRaise = true;
                noDisplayRaise = true;
                break;
            //Watch the breath gauge! Use LEFT CTRL and RIGHT CTRL to inhale and exhale while in the Yellow zones
            //Breathe NORMAL
            case 5:
                textDisplayDuration = 6;
                timeBetweenSteps = 0;
                noHideRaise = true;
                noDisplayRaise = true;
                startInhaleEvent.Raise();
                break;
            //Remember if you fail to breathe properly you will pass out - be careful!

            case 6:
                textDisplayDuration = 10;
                timeBetweenSteps = 20;

                CameraShakeEvent.Raise(0);
                EmergencySirenEvent.Raise(true);

                playSFX(sfx.Siren);

                spawnAlien(alienType.Circle, 2);
                spawnAlien(alienType.CrushedSquare, 2);
                setAlienLimitEvent.Raise(10);
                setSpawnMultiplierEvent.Raise(1f);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Fresh Aliens incoming! Put the Purple Blumbles in the Purple Bin and the Orange Grunks in the orange bin.
            //Spawn 3 Blumbles & 3 Grunks
            //Variable spawn rate of BlumbleB's and GrunksB's
            case 7:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                EmergencySirenEvent.Raise(false);
                CameraShakeEvent.Raise(3);
                stopSFX(sfx.Siren);

                addAlienTypeToSpawner(alienType.CirclePair);
                addAlienTypeToSpawner(alienType.CircleTrio);
                spawnAlien(alienType.CirclePair, 1);
                spawnAlien(alienType.CircleTrio, 1);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Whoa those Blumbles need to be cut! Use the laser!
            //Summon BlumbleA's
            case 8:
                playSFX(sfx.Oxygen);
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                EmergencySirenEvent.Raise(true);
                CameraShakeEvent.Raise(1);
                changeBreathDifficulty(difficulty.Deep);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Uhh sorry cadet! Our new intern broke our oxygen valve, breathe deeper while I fix this!
            //Breathe DEEP
            case 9:
                stopSFX(sfx.Oxygen);
                textDisplayDuration = 6;
                timeBetweenSteps = 0;
                EmergencySirenEvent.Raise(false);
                changeBreathDifficulty(difficulty.Normal);
                noHideRaise = true;
                noDisplayRaise = true;
                break;
            //FIXED! You can breathe normally now - Ugh Interns are the worst sometimes.
            //Breath NORMAL
            case 10:
                textDisplayDuration = 6;
                timeBetweenSteps = 10;
                EmergencySirenEvent.Raise(true);
                playSFX(sfx.Siren);
                CameraShakeEvent.Raise(4);
                playSFX(sfx.HyperSpace);
                setAlienLimitEvent.Raise(20);
                setSpawnMultiplierEvent.Raise(1.5f);
                stopSFX(sfx.Siren);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Alright let’s go DEEPER INTO SPACE! Watch out for more aliens!
           //Increase spawn rate after transition
            case 11:
                textDisplayDuration = 6;
                timeBetweenSteps = 15;
                EmergencySirenEvent.Raise(false);
                addAlienTypeToSpawner(alienType.LongSquare);
                spawnAlien(alienType.LongSquare, 3);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Ah! Those grunks are too big! Use the crusher!
            //Spawn GrunkA's
            case 12:
                textDisplayDuration = 10;
                timeBetweenSteps = 20;
                changeBreathDifficulty(difficulty.Quick);
                playSFX(sfx.Siren);
                playSFX(sfx.PirateBattle);
                CameraShakeEvent.Raise(1);
                FlickerOff.Raise();
                EmergencySirenEvent.Raise(true);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Oh no Pirates are attacking - everyone panic and breathe quickly! Intern help me EEK! 
            //Breath QUICKLY
            case 13:
                textDisplayDuration = 9;
                timeBetweenSteps = 10;
                changeBreathDifficulty(difficulty.Normal);
                stopSFX(sfx.Siren);
                FlickerOn.Raise();
                EmergencySirenEvent.Raise(false);
                CameraShakeEvent.Raise(4);
                playSFX(sfx.HyperSpace);
                setAlienLimitEvent.Raise(30);
                setSpawnMultiplierEvent.Raise(2f);
                stopSFX(sfx.PirateBattle);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Let's get out of here! Onwards to Deepest Space! Breath normally but Expect a lot of aliens...
            //Breath NORMAL
            case 14:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                changeBreathDifficulty(difficulty.Deep);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Is that…a lavender scented candle? Nice! Everyone - Breath that in deeply!
            //Breath DEEPLY
            case 15:
                textDisplayDuration = 7;
                timeBetweenSteps = 20;
                changeBreathDifficulty(difficulty.Deepest);
                CameraShakeEvent.Raise(3);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Whoa slow down - that’s not lavender - that’s eucalyptus - my favorite!! Breath as deep as you can!
            //Breath DEEPEST
            case 16:
                textDisplayDuration = 7;
                timeBetweenSteps = 15;
                changeBreathDifficulty(difficulty.Normal);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Ahh that was nice... Alright - Breath normally
            //Breath NORMAL
            case 17:
                textDisplayDuration = 7;
                timeBetweenSteps = 2;
                narrativeComplete = true;
                setSpawnMultiplierEvent.Raise(0f);
                noHideRaise = false;
                noDisplayRaise = false;
                break;
            //Great work cadet! We ventured to deepest space and cleansed the galaxy! Great job!
        }
        if (playInterComSound) {
            playSFX(sfx.Intercom);
        }
        yield return null;
    }

    private void changeBreathDifficulty(difficulty diff) {
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
                typeInt = 4;
                break;
        }
        Vector2 spawnVector = new Vector2(typeInt, amount);
        spawnAlienEvent.Raise(spawnVector);
    }

    private void addAlienTypeToSpawner(alienType type) {
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
                typeInt = 4;
                break;
        }
        addAlienTypeEvent.Raise(typeInt);
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
            case sfx.PirateBattle:
                pirateBattleAudioSource.Play();
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
            case sfx.PirateBattle:
                pirateBattleAudioSource.Stop();
                break;
        }
    }

    private void displayStepText() {
        if (!noDisplayRaise) {
            displayTextEvent.Raise();
        }
        narrativeObjects[currentIndex].SetActive(true);
        displayingText = true;
    }

    private void hideStepText() {
        if (!noHideRaise) {
            hideTextEvent.Raise();
        }
        narrativeObjects[currentIndex].SetActive(false);
        displayingText = false;
        currentIndex++;
    }

    private IEnumerator gameComplete() {
        gameEndTriggered = true;
        fadeOut.Raise();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Victory");
    }
}