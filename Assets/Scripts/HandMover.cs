using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class HandMover : MonoBehaviour
{

    Rigidbody2D rb;

    Vector2 goalPos;

    [SerializeField]
    private Vector2Reference mousePos;

    [SerializeField, Range(0.01f, 0.1f)]
    private float smoothing;

    [SerializeField]
    private float maxSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, mousePos.Value, smoothing);
    }

    private void FixedUpdate()
    {
        goalPos = Vector2.Lerp(transform.position, mousePos.Value, smoothing);
        rb.MovePosition(goalPos);

    }

    // Called by DeathFX
    public void HandSpeed(float newSpeed)
    {
        smoothing = newSpeed;
    }
}
