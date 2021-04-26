using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public enum ShakeType { Soft = 0, Medium, Hard, SoftLong, Custom};

public class ShakeCaller : MonoBehaviour
{

    private bool isAnimating = false;

    public void e_StartShake(int index)
    {


        if (!isAnimating)
        {
            isAnimating = true;
            StartCoroutine(myShake((ShakeType)index));
        }

    }

    private IEnumerator myShake(ShakeType shakeType)
    {
        switch (shakeType)
        {
            case ShakeType.Soft:
                CameraShakeInstance instance = CameraShaker.Instance.StartShake(1.5f, 1f, 1f);
                yield return new WaitForSeconds(2f);
                instance.StartFadeOut(1f);
                break;

            case ShakeType.Medium:
                CameraShakeInstance instance2 = CameraShaker.Instance.StartShake(2f, 2f, 1f);
                yield return new WaitForSeconds(2f);
                instance2.StartFadeOut(1f);
                break;

            case ShakeType.Hard:
                CameraShakeInstance instance3 = CameraShaker.Instance.StartShake(3f, 3f, 1f);
                yield return new WaitForSeconds(2f);
                instance3.StartFadeOut(1f);
                break;

            case ShakeType.SoftLong:
                CameraShakeInstance instance4 = CameraShaker.Instance.StartShake(1.5f, 1f, 1f);
                yield return new WaitForSeconds(4f);
                instance4.StartFadeOut(1f);
                break;

            case ShakeType.Custom:
                CameraShakeInstance instance5 = CameraShaker.Instance.StartShake(3f, 3f, 1f);
                yield return new WaitForSeconds(0.8f);
                instance5.StartFadeOut(0.3f);
                break;

            default:
                Debug.LogError("Invalid Shake type - ask Hudson");
                break;
        }

        isAnimating = false;
        yield return null;


    }
}
