using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVERY;
using Sirenix.OdinInspector;

public class PlayerBlockRadarV2 : Radar 
{
    [SerializeField] bool isMagnetActive;
    [SerializeField] List<Block> blocks;

    private void Update()
    {

        ListUpdate();
        Magnet();
    }

    private void ListUpdate()
    {
        blocks.Clear();
        radarObjects.ForEach(obj => blocks.Add(obj.GetComponent<Block>()));
        blocks = blocks.FindAll(block => block.State != BlockState.InPlayer);
    }

    private void Magnet()
    {
        if (!isMagnetActive)
            return;

        blocks.ForEach(block => {
            if(block.State is not BlockState.InPlayer && block.State is not BlockState.Anim)
            {
                block.ToPlayer().Forget();
            }
        });
    }


    public void SetActiveMagnet(bool active)
    {
        isMagnetActive = active;
    }
}
