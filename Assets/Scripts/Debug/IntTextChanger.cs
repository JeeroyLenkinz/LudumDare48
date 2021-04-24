using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public class IntTextChanger : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI healthText;

    [SerializeField]
    private IntReference healthSO;

    public void e_ChangeMyTextInt()
    {
        healthText.text = healthSO.Value.ToString();
    }

}
