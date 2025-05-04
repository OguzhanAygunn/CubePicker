using EVERY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{


    [RequireComponent(typeof(Rigidbody))]
    public class CharacterJoystickMovement : MonoBehaviour
    {
        [Title("Main")]
        [SerializeField] private Joystick js;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;


        [HideInInspector] public Rigidbody rb;
        private Vector3 moveDir;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            moveDir = transform.forward;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            Move();
        }

        [SerializeField] float baseSpeed;
        private void Move()
        {

            float targetSpeed = Mathf.Clamp(Mathf.Abs(js.Vertical) + Mathf.Abs(js.Horizontal), 0, 1);
            baseSpeed = Mathf.Lerp(baseSpeed, targetSpeed, Time.fixedDeltaTime);
            if (true)
            {
                Vector3 camForward = Camera.main.transform.forward;
                camForward.y = 0;
                Vector3 camRight = Camera.main.transform.right;
                camRight.y = 0;
                //print("horizontal : " + js.Horizontal + ", vertical : " + js.Vertical);
                var currentMoveDir = (js.Direction.y * camForward) + (js.Direction.x * camRight);
                moveDir = currentMoveDir.normalized;
            }

            rb.position += moveDir * Time.fixedDeltaTime * moveSpeed * baseSpeed;
            if (moveDir.magnitude > Mathf.Epsilon)
            {
                rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.fixedDeltaTime * rotateSpeed);
            }

        }
    }
}
