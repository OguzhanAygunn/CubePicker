using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{


    public class Radar : MonoBehaviour
    {


        [Title("Main")]
        [SerializeField] bool active;
        [SerializeField] RadarType radarType;
        [SerializeField] float radius;
        [SerializeField] UpdateType updateType;
        [ShowIf("@this.updateType == EVERY.UpdateType.Manuel")][SerializeField] float manuelTimeRate;

        [Space(6)]

        [Title("Objects")]
        [SerializeField] Transform radarObj;
        [ShowIf("@this.radarType != RadarType.ObjectType")][SerializeField] LayerMask layer;
        [ShowIf("@this.radarType != RadarType.Layer")][SerializeField] ObjectType objectType;
        [SerializeField] protected List<GameObject> radarObjects;


        private void Awake()
        {
            
        }

        private void Start()
        {
            if(updateType is UpdateType.Manuel)
            {
                ManuelUpdate().Forget();
            }
        }

        private void Update()
        {
            if (!active)
                return;
            if (updateType is UpdateType.Update)
                RadarUpdate();
        }

        private void FixedUpdate()
        {
            if (!active)
                return;
            if (updateType is UpdateType.Fixed)
                RadarUpdate();
        }

        private void LateUpdate()
        {
            if (!active)
                return;

            if (updateType is UpdateType.Late)
                RadarUpdate();
        }


        private async UniTaskVoid ManuelUpdate()
        {
            while(updateType is UpdateType.Manuel)
            {
                RadarUpdate();

                await UniTask.Delay(TimeSpan.FromSeconds(manuelTimeRate));
            }
        }


        private void RadarUpdate()
        {
            Collider[] colliders = null;
            radarObjects.Clear();

            if (radarType is RadarType.ObjectType)
            {
                colliders = Physics.OverlapSphere(transform.position, radius);
                colliders.ForEach(coll =>
                {
                    GameObject obj = coll.gameObject;
                    radarObjects.Add(obj);
                });
                radarObjects = radarObjects.FindAll(obj => obj.GetComponent<ObjectInfoHandler>() && obj.GetComponent<ObjectInfoHandler>().ObjectType == objectType);
            }
            else if (radarType is RadarType.Layer)
            {
                colliders = Physics.OverlapSphere(transform.position, radius, layer);
                colliders.ForEach(obj => radarObjects.Add(obj.gameObject));

            }
            else if (radarType is RadarType.Both)
            {
                colliders = Physics.OverlapSphere(transform.position, radius, layer);
                colliders.ForEach(coll =>
                {
                    GameObject obj = coll.gameObject;
                    radarObjects.Add(obj);
                });

                radarObjects = radarObjects.FindAll(obj => obj.GetComponent<ObjectInfoHandler>() && obj.GetComponent<ObjectInfoHandler>().ObjectType == objectType);
            }
        }

        public void SetRadius(float newRadius)
        {
            radius = newRadius;
        }
    }

}
