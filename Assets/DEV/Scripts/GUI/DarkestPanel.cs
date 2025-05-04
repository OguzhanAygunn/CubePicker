using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkestPanel : MonoBehaviour
{
    public static DarkestPanel instance;


    public float AnimDuration { get { return animDuration; } }

    [Title("Main")]
    [SerializeField] CanvasGroup group;
    [SerializeField] Transform effectParent;

    [Space(7)]

    [Title("Anim")]
    [SerializeField] float animDuration;
    [SerializeField] float animDelay;
    [Space(7)]
    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;
    private void Awake()
    {
        instance = (!instance) ? this : instance;


        effectParent.localScale = startScale;
    }


    [Button(size: ButtonSizes.Large)]
    public async UniTaskVoid PlayAnim(float delay)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        AnimationCurve animCurve = CurveManager.GetCurve("smooth V2");
        effectParent.DOScale(endScale, animDuration).SetEase(animCurve).OnComplete(() =>
        {
            effectParent.DOScale(startScale, animDuration).SetEase(animCurve).SetDelay(animDelay);
        });

    }
}
