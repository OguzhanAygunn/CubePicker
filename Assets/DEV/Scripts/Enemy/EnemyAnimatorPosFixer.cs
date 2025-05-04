using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorPosFixer : MonoBehaviour
{
    Vector3 defaultPos;

    private void Awake()
    {
        defaultPos = transform.localPosition;
    }

    private void LateUpdate()
    {
        transform.localPosition = defaultPos;
    }
}
