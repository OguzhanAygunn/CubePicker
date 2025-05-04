using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EVERY
{

    public class RigidbodyLimitter : MonoBehaviour
    {
        [Title("Velocity")]
        [SerializeField] bool activeLimitVelocity;
        [SerializeField] List<VelocityLimitter> velocityLimitters;


        private void Update()
        {
            if (activeLimitVelocity)
            {
                velocityLimitters.ForEach(v => v.Update());
            }
        }


    }


    [System.Serializable]
    public class VelocityLimitter
    {
        [Title("Main")]
        public bool active=true;
        public Rigidbody rigid;

        public bool limitX;
        public bool limitY;
        public bool limitZ;

        [Space(5)]

        [ShowIf(nameof(limitX))] public float minX;
        [ShowIf(nameof(limitX))] public float maxX;
        
        [Space(5)]

        [ShowIf(nameof(limitY))] public float minY;
        [ShowIf(nameof(limitY))] public float maxY;

        [Space(5)]

        [ShowIf(nameof(limitZ))] public float minZ;
        [ShowIf(nameof(limitZ))] public float maxZ;


        public void Update()
        {
            Limit();
        }

        private void Limit()
        {
            if (!active)
                return;

            Vector3 velocity = rigid.velocity;

            if (limitX)
                velocity.x = Mathf.Clamp(velocity.x, minX, maxX);

            if (limitY)
                velocity.y = Mathf.Clamp(velocity.y, minY, maxY);

            if (limitZ)
                velocity.z = Mathf.Clamp(velocity.z, minZ, maxZ);

            rigid.velocity = velocity;

        }

    }
}