using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public class IntTextChanger : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI displayText;

    [SerializeField]
    private IntReference intSO;

    public void e_ChangeMyTextInt()
    {
        displayText.text = intSO.Value.ToString();
    }

}
