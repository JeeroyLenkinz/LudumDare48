using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorpConnector : MonoBehaviour
{

    public GameObject morpOne;
    public GameObject morpTwo;

    private LineRenderer lRenderer;
    EdgeCollider2D edgeCollider2D;

    private List<Vector2> lrPoints = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        lRenderer = GetComponent<LineRenderer>();
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        lrPoints.Add(morpOne.transform.position);
        lrPoints.Add(morpTwo.transform.position);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lrPoints[0] = morpOne.transform.position;
        lrPoints[1] = morpTwo.transform.position;


        //position
        lRenderer.SetPosition(0, morpOne.transform.position);
        lRenderer.SetPosition(1, morpTwo.transform.position);

        Vector2[] points = new Vector2[2]
        {
            morpOne.transform.localPosition,
            morpTwo.transform.localPosition
        };

        //update the edge colliders points
        //edgeCollider2D.points = points;

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
        lRenderer.enabled = false;
        //Destroy(this.gameObject);
    }

    public GameObject[] GetConnectedMorps()
    {
        GameObject[] morps = { morpOne, morpTwo };
        return morps;
    }
}
