using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;

    [SerializeField] List<Collectible> sceneCollectibles;


    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }

    [Button(size: ButtonSizes.Large)]
    public void SceneCollectibleListUpdate()
    {
        sceneCollectibles = FindObjectsOfType<Collectible>().ToList();
    }


    public void CollectPhaseEnd()
    {
        AllCollectiblesDestroy();
    }

    public void AllCollectiblesDestroy()
    {
        List<Collectible> collectibles = sceneCollectibles.FindAll(collectible => collectible.State is EVERY.CollectibleState.Ready);
        collectibles.ForEach(collectible => collectible.DeActive());
    }
}
