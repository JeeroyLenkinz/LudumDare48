using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBase : MonoBehaviour, IUsable
{
    [SerializeField]
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
        joint = GetComponent<FixedJoint2D>();
        handRB = GetComponent<Rigidbody2D>();

        originalScale = transform.localScale;
    }

    public void OnUse()
    {
        transform.localScale = squeezeScale;
        // Change Art
        joint.connectedBody = handRB;
        // Change hitbox
    }

    public void OnRelease()
    {
        transform.localScale = originalScale;
    }
}
