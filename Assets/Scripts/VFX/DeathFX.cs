using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DeathFX : MonoBehaviour
{
    [SerializeField]
    private GameEvent fadeOut;

    [SerializeField]
    private VignetteGet vigScript;

    [SerializeField]
    private HandMover handMover;

    Vignette vignette;

    private Camera mainCam;
    [SerializeField]
    private IntReference health;

    private HealthManager healthManager;

    [SerializeField]
    private float vignetteMin;
    [SerializeField]
    private float vignetteMax;
    [SerializeField]
    private float handSmoothStart;
    [SerializeField]
    private float handSmoothEnd;

    [SerializeField]
    private PostProcessingData postProcVol;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        healthManager = GetComponent<HealthManager>();
        vignette = vigScript.GetVignette();
        vignette.intensity.Override(vignetteMin);
        //vignette.Override();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DoHand()
    {
        float currentHealthPercent = (float)health.Value / (float)healthManager.maxHealth;
        float speedPercent =((currentHealthPercent * (handSmoothStart - handSmoothEnd)) + handSmoothEnd);
        //float healthPercentage = healthManager.maxHealth
        handMover.HandSpeed(speedPercent);
    }

    private void DoVignette()
    {
        float currentHealthPercent = (float)health.Value / (float)healthManager.maxHealth;
        float smoothPercent = vignetteMax - ((currentHealthPercent * (vignetteMax - vignetteMin)) + vignetteMin);

        vignette.intensity.Override(smoothPercent);
    }

    public void e_healthChanged()
    {
        DoVignette();
        DoHand();

        if(health.Value <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        fadeOut.Raise();

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("GameOver");
    }
}
