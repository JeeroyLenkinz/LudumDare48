using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorpConnector : MonoBehaviour
{

    public GameObject morpOne;
    public GameObject morpTwo;

    private LineRenderer lr;

    EdgeCollider2D edge;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lr.SetPosition(0, morpOne.transform.position);
        lr.SetPosition(1, morpTwo.transform.position);

        // Use Raycast not edge collider and check for collision
    }
}
