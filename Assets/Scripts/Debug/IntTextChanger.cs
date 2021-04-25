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

    [SerializeField]
    private string prefix;

    public void e_ChangeMyTextInt()
    {
        displayText.text = prefix + intSO.Value.ToString();
    }

}
