﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBase : MonoBehaviour, IUsable
{
    private GameObject hand;

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

    private Sprite normalSprite;
    private Sprite grabSprite;

    private Vector2 releaseVel;

    private List<GameObject> morpsConnected = new List<GameObject>();

    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("hand");

        rb = GetComponent<Rigidbody2D>();

        jointHand.enabled = false;
        jointHand.connectedBody = hand.GetComponent<Rigidbody2D>();
        //handRB = hand.GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;
    }


    public void OnUse()
    {
        transform.localScale = squeezeScale;
        // Change Art
        jointHand.enabled = true;

        // Change hitbox
    }

    public void OnRelease()
    {
        releaseVel = rb.velocity;
        transform.localScale = originalScale;
        jointHand.enabled = false;

        rb.velocity = releaseVel;

    }

}
