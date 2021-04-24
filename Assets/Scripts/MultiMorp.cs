using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMorp : MonoBehaviour
{

    public GameObject[] morpsConnected;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SeperateMorps(GameObject morpOne, GameObject morpTwo, GameObject connector)
    {
        // Trigger smoke cloud

        // Detach Joint
        morpOne.GetComponent<AlienBase>().DetachJoint(morpTwo);

        // Detach from parents (i.e. this)
        morpOne.transform.parent = null;
        morpTwo.transform.parent = null;

        // Destroy this and the connector
        Destroy(this.gameObject);


    }

}
