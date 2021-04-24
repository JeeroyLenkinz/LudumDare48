using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private IntReference healthSO;
    public int maxHealth;
    public int breathMissHealthLoss;
    public int breathHitHealthGain;
    // Start is called before the first frame update
    void Start()
    {
        healthSO.Value = maxHealth;
        Debug.Log(healthSO.Value);
    }

    // Raised by checkBreathHit in Breather.cs
    public void e_breathMiss() {
        int tempHealth = healthSO.Value - breathMissHealthLoss;
        tempHealth = Mathf.Clamp(tempHealth, 0, maxHealth);
        healthSO.Value = tempHealth;
    }

    // Raised by checkBreathHit in Breather.cs
    public void e_breathHit() {
        int tempHealth = healthSO.Value + breathHitHealthGain;
        tempHealth = Mathf.Clamp(tempHealth, 0, maxHealth);
        healthSO.Value = tempHealth;
    }
}
