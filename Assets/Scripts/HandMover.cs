using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class HandMover : MonoBehaviour
{

    [SerializeField]
    private Vector2Reference mousePos;

    [SerializeField, Range(0.01f, 0.1f)]
    private float smoothing;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, mousePos.Value, smoothing);
    }
}
