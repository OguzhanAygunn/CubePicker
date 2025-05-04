using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EVERY;
using Cysharp.Threading.Tasks;
using System;

public class BuildController : MonoBehaviour
{
    public bool AllBlockAreFree { get { return allBlocksAreFree; } set { allBlocksAreFree = value; } }
    public bool AddExplosion { get { return addExplosion; } }
    public BlockSpawnAnimType SpawnAnimType {  get { return spawnAnimType; } }
    [Title("Main")]
    [SerializeField] BlockSpawnAnimType spawnAnimType;
    [SerializeField] bool randomColor;
    [SerializeField] bool allBlocksAreFree;
    [SerializeField] List<Block> blocks;

    [Space(7)]

    [Title("Explosion")]
    [SerializeField] bool addExplosion;
    [SerializeField] public float minPower;
    [SerializeField] public float maxPower;

    [Space(7)]

    [Title("Block Spawn Anim")]
    [SerializeField] float mainDelay = 1;
    [SerializeField] float perDelay = 0.005f;

    [Button(size:ButtonSizes.Large)]
    public void UpdateBlockList()
    {
        blocks = GetComponentsInChildren<Block>().ToList();

        blocks = blocks.OrderBy(block => block.transform.position.y).ToList();

    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        PlayBlocksSpawnAnims();
        TryRandomColor();
        InitBlocks();
    }

    public void InitBlocks()
    {
        blocks.ForEach(block => block.Init());
        blocks.ForEach(block => BlockManager.instance.AddBlockInAllBlockList(block));
    }

    [Button(size: ButtonSizes.Large)]
    public void AssignMaterial(Material mat)
    {
        List<MeshRenderer> renderers = GetComponentsInChildren<MeshRenderer>().ToList();

        renderers.ForEach(renderer => renderer.material = mat);
    }

    public void TryRandomColor()
    {
        if (!randomColor)
            return;

        blocks.ForEach(block => {
            block.ChangeRandomColor();
        });
    }

    public void PlayBlocksSpawnAnims()
    {
        float delay = mainDelay;
        delay += perDelay;


        blocks.ForEach(block => {

            block.PlaySpawnAnim(delay);
            delay += perDelay;
        });
    }

    public void AllBlocksFree(Block _block)
    {
        if (allBlocksAreFree)
            return;

        allBlocksAreFree = true;
        blocks.ForEach(block => { 
            
            if(block.State == BlockState.Static && block != _block)
            {
                block.SetBlockState(newState: BlockState.Free).Forget();
                
            }
        });
    }
}
