using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Experimental : MonoBehaviour
{
    Vector3 endPos;
    Vector3 defaultPos;

    AnimationCurve curve;
    private DefaultValuesHandler valuesHandler;
    private void Awake()
    {
        valuesHandler = GetComponent<DefaultValuesHandler>();
        defaultPos = transform.position;
        endPos = transform.position;
        endPos.x += 6f;
    }

    private void Start()
    {
        curve = CurveManager.GetCurve("Smooth");

        //Move();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            valuesHandler.AllReset();
        }
    }

    Sequence sequence;
    [Button(size: ButtonSizes.Large)]
    private void Move()
    {

        sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(endPos, 1F).SetEase(curve));
        sequence.Append(transform.DOMove(defaultPos, 1f).SetEase(curve));
        sequence.OnComplete(() => Move());

    }

    [Button(ButtonSizes.Large)]
    public void DestroySequence()
    {
        sequence.Kill();
    }
}
