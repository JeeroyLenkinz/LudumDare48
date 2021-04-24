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

    private List<GameObject> collidingAliens = new List<GameObject>();
    private GameObject grabbedObj;

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
        sr.color = Color.black;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Interactable"));
        if(hit.collider != null)
        {
            grabbedObj = hit.collider.gameObject;
            if (grabbedObj.CompareTag("alien"))
            {
                grabbedObj.GetComponent<AlienBase>().OnUse();
            }
        }
        //collider.isTrigger = false;
    }

    private void Release()
    {
        sr.color = Color.white;
        if(grabbedObj == null)
        {
            return;
        }
        if (grabbedObj.CompareTag("alien"))
        {
            grabbedObj.GetComponent<AlienBase>().OnRelease();
        }
    }
}
