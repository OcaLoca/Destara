using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakePage : MonoBehaviour
{
    public static ShakePage Instance { get; private set; }

    public Transform camTransform;
    Vector3 originalPos;
    Coroutine runningShake;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable()
    {
        originalPos = camTransform.localPosition;
        //ShakeObject(this.shakeAmount, this.duration); use to test
    }
    public void ShakeObject(float shakeAmount)
    {
        ShakeAmountAndDurationManager(shakeAmount);
    }

    IEnumerator ShakeObjectTransform(float shakeAmount, float shakeDuration)
    {
        if (Application.isBatchMode) { yield return null; }
        camTransform.DOShakePosition(shakeDuration, shakeAmount, vibrato: 50, 90, snapping: true, fadeOut: true);

        yield return new WaitForSeconds(shakeDuration);
        camTransform.localPosition = originalPos;
    }

    public static float GetShakeStrengthFromDamage(float damage)
    {
        var shakeStrengthCurve = new AnimationCurve();
        shakeStrengthCurve.AddKey(0, 0);
        shakeStrengthCurve.AddKey(5, 30);
        shakeStrengthCurve.AddKey(20, 50);
        return shakeStrengthCurve.Evaluate(damage);
    }

    void ShakeAmountAndDurationManager(float damage)
    {
        float shakeAmount;
        float shakeDuration;

        switch (damage)
        {
            case > 2000:
                shakeAmount = 50;
                shakeDuration = 3;
                break;
            case > 1000:
                shakeAmount = 35;
                shakeDuration = 2.5f;
                break;
            case > 500:
                shakeAmount = 20;
                shakeDuration = 2;
                break;
            case > 250:
                shakeAmount = 10;
                shakeDuration = 1.5f;
                break;
            case > 100:
                shakeAmount = 5;
                shakeDuration = 1;
                break;
            case > 50:
                shakeAmount = 3;
                shakeDuration = 0.5f;
                break;
            default:
                shakeAmount = 2;
                shakeDuration = 0.3f;
                break;
        }
        runningShake = StartCoroutine(ShakeObjectTransform(GetShakeStrengthFromDamage(shakeAmount), shakeDuration));
    }
}
