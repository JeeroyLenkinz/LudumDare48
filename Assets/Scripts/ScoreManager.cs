using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private IntReference scoreSO;
    [SerializeField]
    private int scoreIncrement;

    void Start()
    {
        scoreSO.Value = 0;
    }

    public void e_incrementScore() {
        scoreSO.Value += scoreIncrement;
    }
}
