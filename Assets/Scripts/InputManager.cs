using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent inhaleCtrl;
    [SerializeField]
    private GameEvent exhaleCtrl;

    [SerializeField]
    private BoolReference lClick;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            inhaleCtrl.Raise();
        }

        if (Input.GetKeyDown(KeyCode.RightControl))
        {
            exhaleCtrl.Raise();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            lClick.Value = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            lClick.Value = false;
        }
    }
}
