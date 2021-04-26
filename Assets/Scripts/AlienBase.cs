using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;
using System;
using System.Collections.Specialized;

public class AlienBase : MonoBehaviour, IUsable
{
    private GameObject hand;
    [SerializeField]
    private GameObject GorpLong;
    [SerializeField]
    private GameObject GorpShort;
    [SerializeField]
    private GameObject GorpSqueeze;
    [SerializeField]
    private GameObject GorpCrushSqueeze;
    [SerializeField]
    private GameObject MorpNormal;
    [SerializeField]
    private GameObject MorpSqueeze;

    [SerializeField]
    private Sprite MorpUncut;
    [SerializeField]
    private Sprite MorpCut;

    [SerializeField]
    private SpriteRenderer MorpSpriteRend;

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

    private CapsuleCollider2D capsuleColl;
    private PolygonCollider2D polyColl;
    private BoxCollider2D boxColl;
    private AudioSource audioSource;

    [SerializeField]
    private ParticleSystem smokePuff;

    [SerializeField]
    private GameObject longLight;
    [SerializeField]
    private GameObject crushLight;
    [SerializeField]
    private GameObject longLightSquish;
    [SerializeField]
    private GameObject crushLightSquish;

    [SerializeField]
    private GameObject morpLight;
    [SerializeField]
    private GameObject morpLightSquish;


    void Start()
    {
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

        capsuleColl = GetComponent<CapsuleCollider2D>();
        polyColl = GetComponent<PolygonCollider2D>();
        boxColl = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();

        if (shapeType == "squareHole") {
            OnGorpSpawn(isCrushed);
        }

        if(myParent == null && shapeType == "circleHole")
        {
            MorpSpriteRend.sprite = MorpCut;
        }
        else if (shapeType == "circleHole")
        {
            MorpSpriteRend.sprite = MorpUncut;
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
                            StartCoroutine(dropAlien(false));
                        }
                        else if (shapeType == "squareHole" && isCrushed) {
                            squareScored.Raise();
                            StartCoroutine(dropAlien(false));
                        }
                    }
                }
                else if (dropTag == "OutOfBounds") {
                    isOnTable = false;
                    triggerParentStatusCheck();
                    if (!isHeld && myParent == null && !isDropping) {
                        StartCoroutine(dropAlien(true));
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
            boxColl.enabled = true;
            GorpLong.SetActive(false);
            GorpShort.SetActive(true);
        }
    }

    public void OnUse()
    {
        //transform.localScale = squeezeScale;
        ChangeArt(true);
        ChangeLightOn();
        rb.rotation = 0f;
        jointHand.enabled = true;
        isHeld = true;
        audioSource.Play();
        triggerParentStatusCheck();

        // Change hitbox
    }

    public void OnRelease()
    {
        ChangeArt(false);
        ChangeLightOff();
        releaseVel = rb.velocity;
        transform.localScale = originalScale;
        jointHand.enabled = false;
        if (shapeType == "squareHole" && isCrushed)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 0.89f);
        }
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
        MorpSpriteRend.sprite = MorpCut;
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

    private IEnumerator dropAlien(bool isOffScreen) {
        isDropping = true;
        if (!isOffScreen) {
            rb.drag = 8;
            Tweener tweener = animBasic.Animate(AnimationTweenType.Scale, Vector2.zero, Vector2.zero);
            yield return new WaitForSeconds(tweener.Duration()/3);
        }
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
        smokePuff.Play();
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GorpLong.SetActive(false);
        GorpShort.SetActive(true);
        if(longLight.activeSelf == true)
        {
            longLight.SetActive(false);
            crushLight.SetActive(true); ;
        }
        rb.velocity = Vector2.zero;
    }

    public void OnJointBreak2D(Joint2D joint)
    {
        transform.parent.GetComponent<MultiMorp>().ForceBreak();
    }

    private void ChangeLightOn()
    {
        if(shapeType == "squareHole")
        {
            if(crushLight.activeSelf == true)
            {
                crushLight.SetActive(false);
                crushLightSquish.SetActive(true);
            }
            else if (longLight.activeSelf == true)
            {
                longLight.SetActive(false);
                longLightSquish.SetActive(true);
            }

        }
        else if (shapeType == "circleHole")
        {
            if(morpLight.activeSelf == true)
            {
                morpLight.SetActive(false);
                morpLightSquish.SetActive(true);
            }
            else if (morpLightSquish.activeSelf == true)
            {
                morpLight.SetActive(true);
                morpLightSquish.SetActive(false);
            }

        }
    }

    private void ChangeLightOff()
    {
        if (shapeType == "squareHole")
        {
            if (crushLightSquish.activeSelf == true)
            {
                crushLight.SetActive(true);
                crushLightSquish.SetActive(false);
            }
            else if (longLightSquish.activeSelf == true)
            {
                longLight.SetActive(true);
                longLightSquish.SetActive(false);
            }

        }
        else if (shapeType == "circleHole")
        {
            if (morpLightSquish.activeSelf == true)
            {
                morpLight.SetActive(true);
                morpLightSquish.SetActive(false);
            }

        }
    }

    private void ChangeArt(bool isGrabbed)
    {
        if(shapeType == "circleHole")
        {
            if (isGrabbed)
            {
                MorpNormal.SetActive(false);
                MorpSqueeze.SetActive(true);
            } else if (!isGrabbed)
            {
                MorpNormal.SetActive(true);
                MorpSqueeze.SetActive(false);
            }

        } else if(shapeType == "squareHole" && !isCrushed)
        {
            if (isGrabbed)
            {
                capsuleColl.enabled = false;
                polyColl.enabled = true;
                GorpLong.SetActive(false);
                GorpSqueeze.SetActive(true);

            }
            else if (!isGrabbed)
            {
                capsuleColl.enabled = true;
                polyColl.enabled = false;
                GorpLong.SetActive(true);
                GorpSqueeze.SetActive(false);
            }
        }
        else if (shapeType == "squareHole" && isCrushed)
        {
            if (isGrabbed)
            {
                boxColl.offset = new Vector2(0.48f, 0.96f);
                GorpShort.SetActive(false);
                GorpCrushSqueeze.SetActive(true);

            }
            else if (!isGrabbed)
            {
                boxColl.offset = new Vector2(0.48f, -0.06f);
                GorpShort.SetActive(true);
                GorpCrushSqueeze.SetActive(false);
            }
        }
    }

    public void e_GlowOn()
    {
        if(shapeType == "squareHole" && !isCrushed)
        {
            longLight.SetActive(true);
        } else if(shapeType == "squareHole" && isCrushed)
        {
            crushLight.SetActive(true);
        }
        else if (shapeType == "squareHole" && !isCrushed && isHeld)
        {
            longLightSquish.SetActive(true);
        }
        else if (shapeType == "squareHole" && isCrushed && isHeld)
        {
            crushLightSquish.SetActive(true);
        } else if(shapeType == "circleHole" && !isHeld)
        {
            morpLight.SetActive(true);
        }
        else if (shapeType == "circleHole" && isHeld)
        {
            morpLightSquish.SetActive(true);
        }
    }

    public void e_GlowOff()
    {
        if(shapeType == "squareHole")
        {
            longLight.SetActive(false);
            crushLight.SetActive(false);
            longLightSquish.SetActive(false);
            crushLightSquish.SetActive(false);
        } else if (shapeType == "circleHole")
        {
            morpLight.SetActive(false);
            morpLightSquish.SetActive(false);
        }



    }
}
