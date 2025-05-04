using Sirenix.OdinInspector;
using UnityEngine;


namespace EVERY
{

    public class WorldPosTracker : MonoBehaviour
    {
        [Title("Main")]
        [SerializeField] bool active;
        [SerializeField] Transform obj;
        [SerializeField] Transform target;
        [SerializeField] Vector3 offset;

        private Camera cam;
        private void Start()
        {
            cam = CameraController.instance.Camera;
        }


        private void Update()
        {
            Movement();
        }

        private void Movement()
        {
            if (!active)
                return;

            Vector3 pos = cam.WorldToScreenPoint(target.position);
            pos += offset;

            obj.position = pos;
        }
    }
}

