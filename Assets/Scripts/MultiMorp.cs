using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiMorp : MonoBehaviour
{

    public GameObject[] morpsConnected;
    public GameObject[] connectors;
    private List<GameObject> connectorList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject morp in morpsConnected)
        {
            morp.GetComponent<AlienBase>().SetAttached();
        }

        foreach(GameObject connector in connectors)
        {
            connectorList.Add(connector);
        }
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
        Destroy(connector);
        if(connectorList.Count == 0)
        {
            Destroy(this);
        }
        


    }

}
