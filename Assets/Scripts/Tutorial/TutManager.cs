using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutManager : MonoBehaviour
{
    public int tutIndex;
    public GameObject tutStepsGO;
    public GameObject[] tutStepsUI;

    private GameObject currentStep;
    private bool canControl;

    private void Start()
    {
        tutIndex = 0;

        foreach (GameObject tutStep in tutStepsUI)
        {
            tutStep.SetActive(false);
        }

        tutStepsUI[0].SetActive(true);
        currentStep = tutStepsUI[0];
    }

    // The button to advance calls this
    public void b_IterateTutorial()
    {
        if (canControl)
        {
            canControl = false;
            StartCoroutine(Animate_Advance());
        }

    }

    private void Advance_Tutorial()
    {
        //tutStepsUI[tutIndex].SetActive(false);
        currentStep.SetActive(false);
        tutIndex++;
        tutStepsUI[tutIndex].SetActive(true);
        currentStep = tutStepsUI[tutIndex];
        Debug.Log("Tutorial Index now at: " + tutIndex.ToString());
    }

    private void Advance_Tutorial(int index)
    {
        currentStep.SetActive(false);
        //tutStepsUI[tutIndex].SetActive(false);
        tutIndex = index;
        currentStep = tutStepsGO.transform.Find("Step " + index).gameObject;
        currentStep.SetActive(true);
        Debug.Log("Tutorial Index now at: " + tutIndex.ToString());
    }

    private IEnumerator Animate_Advance()
    {
        yield return null;

        Tweener tempTweener = null;

        switch (tutIndex)
        {
            case 0:
                Advance_Tutorial(90);
                canControl = true;
                break;

            default:
                Debug.LogError("Invalid Tutorial Step!");
                break;
        }
    }
}
