using EVERY;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{
    public class ObjectInfoHandler : MonoBehaviour
    {
        [SerializeField] public ObjectType objectType;
        [SerializeField] string id;
        public ObjectType ObjectType { get { return objectType; } }
        public string Id { get { return id; } }
    }
}