using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent inhaleCtrl;
    [SerializeField]
    private GameEvent exhaleCtrl;

    [SerializeField]
    private BoolReference lClick;

    [SerializeField]
    private Vector2Reference mousePos;

    [SerializeField]
    private Camera cam;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inhaleCtrl.Raise();
        }

        if (Input.GetKeyDown(KeyCode.P))
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

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        mousePos.Value = cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
