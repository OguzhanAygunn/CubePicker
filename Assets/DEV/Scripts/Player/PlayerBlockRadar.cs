using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVERY;
using Cysharp.Threading.Tasks;
using System;

public class PlayerBlockRadar : MonoBehaviour
{
    [Title("Main")]
    [SerializeField] bool active;
    [SerializeField] SphereCollider collider;
    [SerializeField] SphereCollider colliderTrigger;
    
    [Space(6)]
    
    [SerializeField] float radius = 0.5f;
    [SerializeField] float radiusSpeed = 1f;
    private void Update()
    {
        RadiusController();
    }

    private void RadiusController()
    {
        collider.radius = Mathf.MoveTowards(collider.radius, radius, radiusSpeed * Time.deltaTime);
        colliderTrigger.radius = Mathf.MoveTowards(colliderTrigger.radius, radius, radiusSpeed * Time.deltaTime);
    }

    public void SetRadius(BlockerLevelInfo info)
    {
        radius = info.radius;
    }

    public async UniTaskVoid SetActive(bool active,float delay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        this.active = active;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Block") || other.gameObject.layer == LayerMask.NameToLayer("CollBlock"))
        {
            Block block = other.GetComponent<Block>();
            
            if (block.State is BlockState.InPlayer)
                return;

            if (!active)
            {
                if (block.State is BlockState.Anim)
                {
                    block.SetBlockState(BlockState.InPlayer).Forget();
                    return;
                }

                return;

            }

            block.SetBlockState(BlockState.InPlayer).Forget();
        }
    }
}
