using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBlocker : MonoBehaviour
{
    public static PlayerBlocker instance;
    public static PlayerBlocker Instance {  get { return instance; } }

    [Title("Block Lists")]
    [SerializeField] List<Block> allBlocks;
    [SerializeField] List<Block> inBlocks;

    [Space(7)]

    [Title("Controller")]
    [SerializeField] PlayerBlockRadar blockRadar;

    [Space(7)]

    [Title("Level")]
    [SerializeField] int startLevel;
    [SerializeField] List<BlockerLevelInfo> levelInfos;
    [SerializeField] BlockerLevelInfo currentLevel;
    private void Awake()
    {
        instance = (!instance) ? this : instance;

        SetLevel(levelIndex: startLevel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InBlocksTriggerEffect().Forget();
        }
        
    }

    #region List

    public void AddBlockInAllBlockList(Block block)
    {
        if (instance.allBlocks.Contains(block))
            return;

        instance.allBlocks.Add(block);
        instance.UpdateLevel();

    }

    public void RemoveBlockInAllBlockList(Block block)
    {
        if (!instance.allBlocks.Contains(block))
            return;

        instance.allBlocks.Remove(block);
    }

    public void AddBlockInInBlockList(Block block)
    {
        if (instance.inBlocks.Contains(block))
            return;

        instance.inBlocks.Add(block);
        instance.UpdateLevel();

    }

    public void RemoveBlockInInBlockList(Block block)
    {
        if (!instance.inBlocks.Contains(block))
            return;

        instance.inBlocks.Remove(block);
    }

    #endregion

    #region GET & SET
    public void SetLevel(int levelIndex)
    {
        BlockerLevelInfo levelInfo = GetLevelInfo(levelIndex);

        currentLevel = levelInfo;

        blockRadar.SetRadius(levelInfo);
    }

    public BlockerLevelInfo GetLevelInfo(int levelIndex)
    {
        BlockerLevelInfo levelInfo = levelInfos.Find(level => level.level == levelIndex);

        return levelInfo;
    }
    
    public BlockerLevelInfo GetLevel(int blockCount)
    {
        return levelInfos.Find(info => info.blockCount == blockCount);
    }

    public Block GetNearestBlock(Transform target)
    {
        int count = inBlocks.OrderBy(block => block.GetDistance(target)).ToList().Count;

        if (count == 0)
            return null;

        Block block = inBlocks.OrderBy(block => block.GetDistance(target)).ToList()[0];

        return block;
    }

    [Button(size: ButtonSizes.Large)]
    public void NearestBlock(Transform target)
    {
        Block block = GetNearestBlock(target: target);

        block.transform.localScale *= 2f;
    }
    #endregion

    #region Update (Other)
    public void UpdateLevel()
    {
        int count = inBlocks.Count;

        BlockerLevelInfo levelInfo = GetLevel(count);

        if (levelInfo == null)
            return;

        SetLevel(levelInfo.level);
    }
    #endregion

    #region Trigger Effects
    public async UniTaskVoid InBlocksTriggerEffect()
    {
        inBlocks.ForEach(block => block.EffectController.Shader.Play("Highlight"));

        List<Block> blocks = new List<Block>();
        inBlocks.ForEach(block => blocks.Add(block));

        blocks = blocks.OrderBy(block => block.GetDistance()).ToList();

        //0.0025f
        float delay = 0.050f / blocks.Count;
        delay = Mathf.Clamp(delay, 0, 0.01f);

        blocks.Reverse();
        foreach(Block block in blocks) {
            block.EffectController.Sizer.PlayEffect("Mini Popup");
            //await UniTask.Delay(TimeSpan.FromSeconds(delay));

        }

        await UniTask.Delay(0);
    }
    #endregion


}


[System.Serializable]
public class BlockerLevelInfo
{
    [Title("Main")]
    public int level;
    public float radius;
    public int blockCount;
}

