using EVERY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [Title("Main")]
    [SerializeField] bool active;
    [SerializeField] DefaultValuesHandler defaults;
    [SerializeField] Transform target;

    [Space(6)]

    [SerializeField] Vector3 startScale;
    [SerializeField] Vector3 endScale;
    
    [Space(6)]

    [SerializeField] float distance;
    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    private void Update()
    {
        if (!active) return;

        Vector3 firstPos = target.position;
        Vector3 secondPos = transform.position;

        secondPos.x = firstPos.x;
        secondPos.z = firstPos.z;

        distance = Vector3.Distance(firstPos, secondPos);
        float slerpValue = distance / maxDistance;

        Vector3 targetScale = Vector3.Slerp(startScale, endScale, slerpValue);
        Vector3 scale = transform.localScale;

        transform.localScale = targetScale;
    }

}
