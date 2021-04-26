using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherCrush : MonoBehaviour
{
    private CircleCollider2D circCollider;

    private GameObject collidingAlien;

    // Start is called before the first frame update
    void Start()
    {
        circCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called by CrusherHandle
    public void CrushAlien()
    {
        if (collidingAlien == null)
        {
            return;
        }
        AlienBase alienLogic = collidingAlien.GetComponent<AlienBase>();
        if (alienLogic.Crushable())
        {
            alienLogic.CrushMeDaddy();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != null)
        {
            GameObject otherObj = collision.transform.gameObject;
            if (otherObj.CompareTag("alien"))
            {
                collidingAlien = otherObj;
            }
        }

    }
}
