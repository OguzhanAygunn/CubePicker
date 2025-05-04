using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCollectible : Collectible
{

    [Title("Magnet")]
    [SerializeField] Transform effectSpawnTrs;

    [Space(7)]

    [Title("Pos Anim")]
    [SerializeField] Transform animObj;
    [SerializeField] Vector3 animOffset;
    [SerializeField] float animDuration;
    private Vector3 animDefaultPos;
    private Sequence posSequence;


    [Space(7)]

    [Title("Rotate Anim")]
    [SerializeField] Transform rotObj;
    [SerializeField] Vector3 rotPower;


    private void Awake()
    {
        animDefaultPos = animObj.localPosition;
    }

    private void Start()
    {
        base.Start();
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        rotObj.Rotate(rotPower * Time.deltaTime);
    }

    public async UniTaskVoid PlayAnim(float delay =0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        Vector3 startPos = animDefaultPos + animOffset;
        Vector3 endPos = animDefaultPos;

        AnimationCurve curve = CurveManager.GetCurve("smooth V2");

        Tween posTween = animObj.DOLocalMove(endValue: startPos, duration: animDuration).SetEase(curve).OnComplete( () =>
        {
            animObj.DOLocalMove(endValue: endPos, duration: animDuration).SetEase(curve).OnComplete( () =>
            {
                PlayAnim().Forget();
            });
        });

        posSequence.Append(posTween);
        posSequence.Play();
    }

    public override void Collect()
    {
        print("collect");
        FXManager.PlayFX("Collect Magnet", effectSpawnTrs.position, 4f).Forget();
        PlayerController.instance.RadarV2.SetActiveMagnet(active: true);
        PlayerController.instance.Blocker.InBlocksTriggerEffect().Forget();
        base.SetVisibility(visibility: false, force: true).Forget();
        
    }

}
