using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherCrush : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void checkPresent()
    {

        GameObject grabbedObj;
        AlienBase alienLogic;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Interactable"));
        if (hit.collider != null)
        {
            grabbedObj = hit.collider.gameObject;
            if (grabbedObj.CompareTag("alien"))
            {
                alienLogic = grabbedObj.GetComponent<AlienBase>().OnUse();
            }
        }
    }
}
