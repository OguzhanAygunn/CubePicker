using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVERY;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [Title("Main")]
    [SerializeField] bool active;
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] Rigidbody rb;
    [SerializeField] DefaultValuesHandler defaults;

    [Space(7)]

    [Title("Move")]
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveSpeedAcceleration;

    [Space(7)]

    [Title("Fixer")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float targetYaxis;

    [Space(7)]

    [Title("Rotation")]
    [SerializeField] Transform rotateObj;
    [SerializeField] float rotateSpeed;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (!active)
            return;

        if (GameManager.GameState is not GameState.Go)
            return;

        Move();
    }


    private void SetRigidActive(bool active)
    {
        RigidbodyConstraints constraints = active ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        rb.constraints = constraints;
    }

    Vector3 targetDirection;
    private void Move()
    {
        Vector3 vel = rb.velocity;
        vel.Set(0, vel.y, 0);
        rb.velocity = vel;
        
        Vector3 joyDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        float joyMagnitude = joyDirection.magnitude;

        targetDirection = Vector3.Lerp(targetDirection, joyDirection, Time.fixedDeltaTime * 5f);

        float maxSpeed = maxMoveSpeed * joyMagnitude;
        moveSpeed = Mathf.MoveTowards(moveSpeed, maxSpeed, moveSpeedAcceleration * Time.fixedDeltaTime);

        Vector3 pos = rb.position;
        Vector3 targetPos = pos + (targetDirection * moveSpeed * Time.fixedDeltaTime);

        //targetPos.y = targetYaxis;

        rb.MovePosition(targetPos);


        Vector3 rotatePower = new Vector3(targetDirection.z, 0, -targetDirection.x) * rotateSpeed * Time.fixedDeltaTime;
        rotateObj.Rotate(rotatePower,Space.World);
    }

}
