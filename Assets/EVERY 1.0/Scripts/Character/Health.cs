using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EVERY
{

    public class Health : MonoBehaviour
    {
        [Title("Main")]
        [SerializeField] bool isAlive = true;
        [SerializeField] int hp;
        [SerializeField] int maxHP;
        [SerializeField] int defenceVal;
        public int HP { get { return hp; } }

        [Space(6)]

        [Title("Events")]
        [SerializeField] List<EventInfo> takeHitEvents;
        [SerializeField] List<EventInfo> killEvents;


        [Button(size:ButtonSizes.Large)]
        public void TakeHit(int damage)
        {
            if (!isAlive)
                return;

            damage -= defenceVal;
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            hp -= damage;
            
            if(hp <= 0)
            {
                Kill();
                return;
            }

            takeHitEvents.ForEach(e => e.PlayEvent().Forget());
        }

        public void Kill()
        {
            isAlive = false;
            killEvents.ForEach(e => e.PlayEvent().Forget());
            Vector3 effectSpawnPos = transform.position + Vector3.up;
            FXManager.PlayFX("Kill Enemy", effectSpawnPos, 2f).Forget();
        }


    }


}