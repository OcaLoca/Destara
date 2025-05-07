using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    Vector3 originalPos;

    Coroutine runningShake;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void ShakeObject(Transform objTransform, float shakeAmount, float shakeDuration)
    {
        originalPos = objTransform.localPosition;
        runningShake = StartCoroutine(ShakeObjectTransform(objTransform,GetShakeStrengthFromDamage(shakeAmount), shakeDuration));
    }

    IEnumerator ShakeObjectTransform(Transform targetTransform, float strenght, float shakeDuration)
    {
        if (Application.isBatchMode) { yield return null; }
        targetTransform.DOShakePosition(shakeDuration, strenght, vibrato: 50, 90, snapping: true, fadeOut: true);

        yield return new WaitForSeconds(shakeDuration);
        targetTransform.localPosition = originalPos;
    }


    public static float GetShakeStrengthFromDamage(float damage)
    {
        var shakeStrengthCurve = new AnimationCurve();
        shakeStrengthCurve.AddKey(0, 0);
        shakeStrengthCurve.AddKey(5, 30);
        shakeStrengthCurve.AddKey(20, 50);
        return shakeStrengthCurve.Evaluate(damage);
    }
}
