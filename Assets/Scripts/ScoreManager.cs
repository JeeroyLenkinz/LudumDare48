using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private IntReference scoreSO;
    [SerializeField]
    private int circleScoreIncrement;
    [SerializeField]
    private int squareScoreIncrement;
    [SerializeField]
    private AudioClip circleSFX;
    [SerializeField]
    private AudioClip squareSFX;

    private AudioSource audioSource;

    void Start()
    {
        scoreSO.Value = 0;
        audioSource = GetComponent<AudioSource>();
    }

    public void e_CircleScored() {
        scoreSO.Value += circleScoreIncrement;
        audioSource.clip = circleSFX;
        audioSource.Play();
    }

    public void e_SquareScored() {
        scoreSO.Value += squareScoreIncrement;
        audioSource.clip = squareSFX;
        audioSource.Play();
    }
}
