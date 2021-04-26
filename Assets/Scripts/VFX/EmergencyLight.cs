using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyLight : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -80f);
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -80f);
    }

    private void Update()
    {
        RotateMeBB();
    }

    void RotateMeBB()
    {

        Vector3 targetRot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - 0.2f);

        transform.rotation = Quaternion.Euler(targetRot.x, targetRot.y, targetRot.z);

    }





}
