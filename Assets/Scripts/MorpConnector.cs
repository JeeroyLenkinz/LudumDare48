using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorpConnector : MonoBehaviour
{

    public GameObject morpOne;
    public GameObject morpTwo;

    private LineRenderer lr;

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
        float distance = (morpTwo.transform.position - morpOne.transform.position).magnitude;
        Vector2 dir = (morpTwo.transform.position - morpOne.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(morpOne.transform.position, dir, distance, 1 << LayerMask.NameToLayer("Laser"));
        //Debug.DrawRay(morpOne.transform.position, dir, Color.white, 1f, false);

        if(hit.collider != null)
        {
            NotifyMultiMorp();
        }
    }



    private void NotifyMultiMorp()
    {
        GetComponentInParent<MultiMorp>().SeperateMorps(morpOne, morpTwo, this.gameObject);
        lr.enabled = false;
        //Destroy(this.gameObject);
    }
}
