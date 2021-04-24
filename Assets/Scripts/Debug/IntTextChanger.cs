using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public class IntTextChanger : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI scoreText;



    public void e_ChangeMyTextInt(int score)
    {
        scoreText.text = score.ToString();
    }

}
