using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;
using System;

public class AlienBase : MonoBehaviour, IUsable
{
    private GameObject hand;
    [SerializeField]
    private GameObject GorpLong;
    [SerializeField]
    private GameObject GorpShort;

    private Rigidbody2D rb;
    private Rigidbody2D handRB;
    [SerializeField]
    private FixedJoint2D jointHand;
    [SerializeField]
    private FixedJoint2D[] jointMorp;

    private Vector2 originalScale;
    private Vector2 originalHitbox;

    [SerializeField]
    private Vector2 squeezeScale;
    [SerializeField]
    private Vector2 squeezeHitbox;

    [SerializeField]
    private GameEvent alienDropped;

    private Sprite baseSprite;
    private Sprite grabSprite;

    private Vector2 releaseVel;

    private List<GameObject> morpsConnected = new List<GameObject>();

    private bool isOnTable = true;
    private bool isHeld = false;
    private bool isDropping = false;
    private GameObject myParent;
    private Animate_Basic animBasic;

    [SerializeField]
    private string shapeType;
    [SerializeField]
    private bool isCrushed;

    [SerializeField]
    private GameEvent circleScored;
    [SerializeField]
    private GameEvent squareScored;


    void Start()
    {
        if (shapeType == "squareHole") {
            OnGorpSpawn(isCrushed);
        }
        hand = GameObject.FindGameObjectWithTag("hand");

        rb = GetComponent<Rigidbody2D>();
        animBasic = GetComponent<Animate_Basic>();

        jointHand.enabled = false;
        jointHand.connectedBody = hand.GetComponent<Rigidbody2D>();
        //handRB = hand.GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;

        if (transform.parent != null) {
            myParent = transform.parent.gameObject;
        }
    }

    void FixedUpdate() {
        if (rb.velocity.magnitude != 0) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Drop"));
            if(hit.collider != null)
            {
                string dropTag = hit.transform.gameObject.tag;
                if (dropTag == shapeType) {
                    isOnTable = false;
                    if (!isHeld && myParent == null && !isDropping) {
                        if (shapeType == "circleHole") {
                            circleScored.Raise();
                            StartCoroutine(dropAlien());
                        }
                        else if (shapeType == "squareHole" && isCrushed) {
                            squareScored.Raise();
                            StartCoroutine(dropAlien());
                        }
                    }
                }
                else if (dropTag == "OutOfBounds") {
                    isOnTable = false;
                    triggerParentStatusCheck();
                    if (!isHeld && myParent == null && !isDropping) {
                        StartCoroutine(dropAlien());
                    }
                }
            }
            else {
                isOnTable = true;
                triggerParentStatusCheck();
            }
        }
    }

    public void OnGorpSpawn(bool isShort)
    {
        //ShortBoit
        if(isShort)
        {
            isCrushed = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = true;
            GorpLong.SetActive(false);
            GorpShort.SetActive(true);
        }
    }

    public void OnUse()
    {
        //transform.localScale = squeezeScale;
        // Change Art
        Debug.Log(transform.name + " was used.");
        rb.rotation = 0f;
        jointHand.enabled = true;
        isHeld = true;
        triggerParentStatusCheck();

        // Change hitbox
    }

    public void OnRelease()
    {
        releaseVel = rb.velocity;
        transform.localScale = originalScale;
        jointHand.enabled = false;
        isHeld = false;
        CrushAlign();
        triggerParentStatusCheck();

        rb.velocity = releaseVel;

    }

    internal void ForceBreak()
    {
        foreach (FixedJoint2D joint in jointMorp)
        {
            joint.enabled = false;
        }
    }

    // Called by MultiMorp
    public void DetachJoint(GameObject attachedMorp)
    {
        foreach(FixedJoint2D joint in jointMorp)
        {
            // If the joint in the list is attached to the parameter Morp
            // Then it is the correct one to sever
            if(joint.connectedBody.gameObject == attachedMorp)
            {
                joint.enabled = false;
                return;
            }
        }
    }

    private void CrushAlign()
    {
        if(shapeType == "squareHole" && !isCrushed)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, 1f, 1 << LayerMask.NameToLayer("Alignment"));
            if(hit.collider != null)
            {
                transform.rotation = Quaternion.identity;
                rb.velocity = Vector2.zero;
            }

        }

    }


    // Severed by multiMorp
    public void SetParentNull()
    {
        myParent = null;
    }

    public bool getIsHeld() {
        return isHeld;
    }

    public bool getIsOnTable() {
        return isOnTable;
    }

    private void triggerParentStatusCheck() {
        if (myParent != null) {
            myParent.GetComponent<MultiMorp>().checkChildStatuses();
        }
    }

    private IEnumerator dropAlien() {
        isDropping = true;
        rb.drag = 8;
        Tweener tweener = animBasic.Animate(AnimationTweenType.Scale, Vector2.zero, Vector2.zero);
        yield return new WaitForSeconds(tweener.Duration()/3);
        alienDropped.Raise();
        Destroy(gameObject);
    }

    // Returns true if long Gorp
    public bool Crushable()
    {
        if(shapeType == "squareHole" && !isCrushed)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void CrushMeDaddy()
    {
        isCrushed = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GorpLong.SetActive(false);
        GorpShort.SetActive(true);
    }

    public void OnJointBreak2D(Joint2D joint)
    {
        transform.parent.GetComponent<MultiMorp>().ForceBreak();
    }
}
