using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.SocialPlatforms.Impl;

public class IncrementScore_Test : MonoBehaviour
{

    [SerializeField]
    private IntGameEvent scoreEvent;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void b_IncrementScore()
    {
        score++;
        scoreEvent.Raise(score);
    }
}

