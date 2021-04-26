using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class HandGrabber : MonoBehaviour
{

    [SerializeField]
    private BoolReference mouseClick;


    private SpriteRenderer sr;
    private CapsuleCollider2D collider;

    private List<GameObject> collidingObjs = new List<GameObject>();
    private GameObject grabbedObj;

    [SerializeField]
    private GameObject grabCenterPoint;

    [SerializeField]
    private Sprite openHand;
    [SerializeField]
    private Sprite closedHand;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Called by an event listener
    public void e_toggleGrab()
    {
        if (mouseClick.Value == true)
        {
            Grab();

            // attach joint

        } else if (mouseClick.Value == false)
        {
            Release();
        }
    }

    private void Grab()
    {
        sr.sprite = closedHand;

        grabbedObj = null;

        //RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Interactable"));
        grabbedObj = CalculateClosestObj();
        if(grabbedObj != null)
        {
            //Debug.Log("Closest Object: " + grabbedObj.name.ToString());
        }

        
        if(grabbedObj != null)
        {
            if (grabbedObj.CompareTag("alien"))
            {
                grabbedObj.GetComponent<AlienBase>().OnUse();
                //grabbedObj.transform.rotation = Quaternion.identity;
            } else if (grabbedObj.CompareTag("handle"))
            {
                grabbedObj.GetComponent<CrusherHandle>().OnUse();
            }
        }
        //collider.isTrigger = false;
    }

    private void Release()
    {
        sr.sprite = openHand;
        if(grabbedObj == null)
        {
            return;
        }
        if (grabbedObj.CompareTag("alien"))
        {
            grabbedObj.GetComponent<AlienBase>().OnRelease();
        }
        else if (grabbedObj.CompareTag("handle"))
        {
            grabbedObj.GetComponent<CrusherHandle>().OnRelease();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collidingObjs.Add(collision.transform.gameObject);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        collidingObjs.Remove(collision.transform.gameObject);
    }

    private GameObject CalculateClosestObj()
    {
        GameObject closestObj = null;
        float closestDistMag = 0;

        if (collidingObjs.Count == 0)
        {
            return null;
        }

        closestObj = collidingObjs[0];
        closestDistMag = (closestObj.transform.position - grabCenterPoint.transform.position).magnitude;

        if(collidingObjs.Count == 1)
        {
            return closestObj;
        }

        foreach (GameObject grabbableObj in collidingObjs)
        {
            float thisDistMag = (grabCenterPoint.transform.position - grabbableObj.transform.position).magnitude;
            if (thisDistMag < closestDistMag)
            {
                closestObj = grabbableObj;
                closestDistMag = thisDistMag;
            }
        }

        return closestObj;


    }
}

