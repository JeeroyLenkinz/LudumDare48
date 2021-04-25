using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherHandle : MonoBehaviour, IUsable
{
    private Vector3 myRotation;
    private Vector3 startRot;
    private bool isHeld = false;

    [SerializeField]
    private GameObject hand;

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void FixedUpdate()
    {



    }

    public void OnUse()
    {

        Debug.Log("Grab Slot Arm");
        isHeld = true;
    }

    public void OnRelease()
    {
        Debug.Log("Release Slot Arm");
        isHeld = false;
    }


}
