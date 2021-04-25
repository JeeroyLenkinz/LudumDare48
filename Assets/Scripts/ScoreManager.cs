﻿using System.Collections;
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

    void Start()
    {
        scoreSO.Value = 0;
    }

    public void e_CircleScored() {
        scoreSO.Value += circleScoreIncrement;
    }

    public void e_SquareScored() {
        scoreSO.Value += squareScoreIncrement;
    }
}
