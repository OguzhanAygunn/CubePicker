using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectibleRadar : EVERY.Radar
{
    [Title("Collectible Radar")]
    [SerializeField] bool act;
    [SerializeField] List<Collectible> collectibles;

    private void FixedUpdate()
    {
        if (!act)
            return;

        if (GameManager.GameState is not EVERY.GameState.Go)
            return;

        collectibles.Clear();
        radarObjects.ForEach(obj => collectibles.Add(obj.GetComponent<Collectible>()));
        collectibles.ForEach(coll => coll.Collect());
    }

    public void SetActive(bool active)
    {
        act = active;
    }
}
