using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherHandle : MonoBehaviour, IUsable
{
    private Vector3 myRotation;
    private Vector3 startRot;
    private bool isAnimating = false;
    private bool isPrimed = false;

    [SerializeField]
    private GameObject hand;
    [SerializeField]
    private GameObject piston;
    [SerializeField]
    private GameObject pistonInPoint;
    [SerializeField]
    private GameObject pistonOutPoint;

    Animate_Basic animBasic;
    [SerializeField]
    Animate_Basic animBasicPistonA;
    [SerializeField]
    Animate_Basic animBasicPistonB;

    [SerializeField]
    GameObject crusherCrushLogic;

    BoxCollider2D inCollider;
    PolygonCollider2D outCollider;


    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation.eulerAngles;
        animBasic = GetComponent<Animate_Basic>();
        //animBasicPistonA = piston.GetComponent<Animate_Basic>();
        //animBasicPistonB = piston.GetComponent<Animate_Basic>();
        inCollider = piston.GetComponent<BoxCollider2D>();
        outCollider = piston.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {



    }

    public void OnUse()
    {

        Debug.Log("Grab Slot Arm");
        if (!isAnimating)
        {
            // Start pulling back
            if (!isPrimed)
            {
                StartCoroutine(pullBack());
            }
            else
            {
                StartCoroutine(pushForward());
            }

        }
    }

    public void OnRelease()
    {
        Debug.Log("Release Slot Arm");

    }

    private IEnumerator pullBack()
    {
        isAnimating = true;
        Tween tweener = animBasic.Animate(AnimationTweenType.RotateZ, Vector2.zero, Vector2.zero);
        animBasicPistonA.Animate(AnimationTweenType.Move, pistonOutPoint.transform.position, pistonInPoint.transform.position);
        yield return new WaitForSeconds(tweener.Duration()/2);
        outCollider.enabled = false;
        inCollider.enabled = true;
        yield return new WaitForSeconds(tweener.Duration() / 2);
        isAnimating = false;
        isPrimed = true;
    }

    private IEnumerator pushForward()
    {
        isAnimating = true;
        Tween tweener = animBasic.Animate(AnimationTweenType.RotateZ2, Vector2.zero, Vector2.zero);
        animBasicPistonB.Animate(AnimationTweenType.EndMove, pistonInPoint.transform.position, pistonOutPoint.transform.position);
        yield return new WaitForSeconds(tweener.Duration()*0.1f);
        crusherCrushLogic.GetComponent<CrusherCrush>().CrushAlien();
        // PARTICLES HERE
        outCollider.enabled = false;
        inCollider.enabled = true;

        yield return new WaitForSeconds(tweener.Duration() * 0.9f);
        isAnimating = false;
        isPrimed = false;
    }

}
