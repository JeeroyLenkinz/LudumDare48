using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBase : MonoBehaviour, IUsable
{
    private GameObject hand;

    private Rigidbody2D handRB;
    private FixedJoint2D joint;

    private Vector2 originalScale;
    private Vector2 originalHitbox;

    [SerializeField]
    private Vector2 squeezeScale;
    [SerializeField]
    private Vector2 squeezeHitbox;

    private Sprite normalSprite;
    private Sprite grabSprite;

    void Start()
    {
        hand = GameObject.FindGameObjectWithTag("hand");

        joint = GetComponent<FixedJoint2D>();
        joint.enabled = false;
        joint.connectedBody = hand.GetComponent<Rigidbody2D>();
        //handRB = hand.GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;
    }

    public void OnUse()
    {
        transform.localScale = squeezeScale;
        // Change Art
        joint.enabled = true;

        // Change hitbox
    }

    public void OnRelease()
    {
        transform.localScale = originalScale;
        joint.enabled = false;

    }
}
