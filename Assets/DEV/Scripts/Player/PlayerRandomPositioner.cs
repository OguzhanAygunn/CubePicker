using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRandomPositioner : MonoBehaviour
{

    [Title("Main")]
    [SerializeField] SphereCollider collider;
    [SerializeField] Transform positionerParent;
    [SerializeField] Transform positioner;


    [Button(size:ButtonSizes.Large)]
    public Vector3 GetRandomPos()
    {

        Vector3 defaultScale = positioner.localScale;
        defaultScale.z = collider.radius * 2;
        //positionerParent.localScale = defaultScale;

        Vector3 rotate = Vector3.zero;
        rotate.x = Random.Range(-360f,360f);
        rotate.y = Random.Range(-360f,360f);
        rotate.z = Random.Range(-360f,360f);

        positionerParent.eulerAngles = rotate;
        Vector3 pos = positioner.transform.position;

        return pos;
    }
}
