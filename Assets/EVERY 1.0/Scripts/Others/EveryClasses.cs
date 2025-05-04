using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace EVERY
{
    public class EveryClasses : MonoBehaviour
    {
    }


    [System.Serializable]
    public class EventInfo
    {
        public float delay;
        public UnityEvent _event;

        public async UniTaskVoid PlayEvent()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            _event.Invoke();
        }
    }
}

