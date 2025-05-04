using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public static GroundController instance;

    [SerializeField] DefaultValuesHandler defaults;
    [SerializeField] List<Transform> groundPoints;

    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }


    public async UniTaskVoid ToFightMode(float delay = 0)
    {
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        Vector3 targetScale = defaults.localScale;
        targetScale.x *= 1.25f;
        targetScale.z *= 1.25f;


        AnimationCurve curve = CurveManager.GetCurve("smooth V2");
        transform.DOScale(targetScale, 1.2f).SetEase(curve);
    }

    public Transform GetNearestPoint(Vector3 pos)
    {
        Transform point = groundPoints.OrderBy(p => Vector3.Distance(pos, p.position)).ToList()[0];
        return point;
    }
}



