using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVERY;
using Cysharp.Threading.Tasks;
using System;

public class BlockManager : MonoBehaviour
{
    public static BlockManager instance;
    public static BlockManager Instance { get { return instance; } }
    public static Color DangerousBlockColor { get { return instance.dangerousBlockColor; } }
    public static float DangerousBlockAnimDelay { get { return instance.dangerousBlockAnimDelay; } }
    public float ShootSpeed { get { return shootSpeed; } }
    public Vector3 GroundOffset { get { return grounOffset; } }
    public Vector3 ShootPosOffset { get { return shootPosOffset; } }
    [Title("Main")]
    [SerializeField] List<Block> allBlocks;

    [Space(7)]

    [Title("Trigger")]
    [SerializeField] float radius;
    [SerializeField] LayerMask blockLayer;

    [Space(7)]

    [Title("Block Values")]
    [SerializeField] Color dangerousBlockColor;
    [SerializeField] float dangerousBlockAnimDelay;

    [Space(7)]

    [Title("Shoot")]
    [SerializeField] float shootSpeed;
    [SerializeField] Vector3 grounOffset;
    [SerializeField] Vector3 shootPosOffset;


    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }

    public async UniTaskVoid TriggerBlocks(Block block,float delay = 0,float blockDelay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));


        Collider[] colliders = Physics.OverlapSphere(block.transform.position, radius, blockLayer);
        List<Block> blocks = new List<Block>();
        colliders.ForEach(collider => blocks.Add(collider.GetComponent<Block>()));

        blocks = blocks.FindAll(block => block.State == BlockState.Static);
        blocks.ForEach(block => block.SetBlockState(newState: BlockState.Free, delay: blockDelay).Forget());
    }

    public void AllBlocksFree()
    {
        allBlocks.ForEach(block => block.SetBlockState(BlockState.Free).Forget());
    }

    public void AddBlockInAllBlockList(Block block)
    {
        if (allBlocks.Contains(block))
            return;

        allBlocks.Add(block);
    }

    public void RemoveBlockInAllBlockList(Block block)
    {
        if (!allBlocks.Contains(block))
            return;

        allBlocks.Remove(block);
    }

    public void CollectPhaseEnd()
    {
        FreeBlockDestroy();
    }

    public void FreeBlockDestroy()
    {
        List<Block> freeBlocks = allBlocks.FindAll(block => block.State is BlockState.Free || block.State is BlockState.Static);
        freeBlocks.ForEach(block => block.CollectPhaseFinish());

    }

}
