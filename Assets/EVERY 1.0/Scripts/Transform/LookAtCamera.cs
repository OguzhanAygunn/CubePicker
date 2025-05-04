using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{
    public class LookAtCamera : MonoBehaviour
    {

        private Camera cam;
        [SerializeField] private Vector3 offset;

        private void Start()
        {
            cam = CameraController.instance.Camera;
        }

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
                cam.transform.rotation * Vector3.up);

            if (offset != Vector3.zero)
            {
                transform.eulerAngles += offset;
            }
        }
    }
}

