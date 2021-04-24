using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breather : MonoBehaviour
{
    private bool inhaling;
    private bool exhaling;

    [SerializeField]
    private float breathMarkerPosition;

    public float breathMarkerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        inhaling = false;
        exhaling = false;
    }

    public void e_toggleInhale() {
        inhaling = true;
        exhaling = false;
    }

    public void e_toggleExhale() {
        exhaling = true;
        inhaling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inhaling) {
            breathMarkerPosition += breathMarkerSpeed * Time.deltaTime;
        }
        else if (exhaling) {
            breathMarkerPosition -= breathMarkerSpeed * Time.deltaTime;
        }
    }
}
