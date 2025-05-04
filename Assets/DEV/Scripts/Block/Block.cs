using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    #region  Main Values
    public BlockState State { get { return state; } }
    public EffectController EffectController { get { return effectController; } }

    [Title("Main")]
    [SerializeField] BuildController build;
    [SerializeField] protected BlockState state;
    [SerializeField] BlockType type;

    [Space(7)]

    [Title("Components")]
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Rigidbody rigid;
    [SerializeField] Collider collider;
    [SerializeField] EffectController effectController;
    [SerializeField] DefaultValuesHandler defaults;
    [SerializeField] DefaultValuesHandler meshDefaults;
    [SerializeField] TrailRenderer trail;

    [Space(7)]

    [Title("Others")]
    [SerializeField] Transform meshTrs;
    [SerializeField] LayerMask collLayer;
    
    [Space(7)]

    [Title("Other Block")]
    [SerializeField] LayerMask blockLayer;
    [SerializeField] List<Block> otherBlocks;
    [SerializeField] float rayDistance;


    private Transform playerPos;
    private Sequence toPlayerSequence;
    private bool spawnAnimIsFinished;
    #endregion

    #region Awake & Start & Init
    private void Awake()
    {
        toPlayerSequence = DOTween.Sequence();
        UpdateOtherBlocks();
        SetBlockState(BlockState.Static).Forget();
        SetActiveTrail(active: false, clear: true);
    }

    private void Start()
    {
        build = GetComponentInParent<BuildController>();
        playerPos = PlayerController.instance.BlockParent;

    }

    public void Init()
    {
        InitBlockType();
    }

    private void InitBlockType()
    {
        if(type is BlockType.Normal)
        {

        }
        else if(type is BlockType.Dangerous)
        {
            Color dangerColor = BlockManager.DangerousBlockColor;
            meshDefaults.color = dangerColor;
            meshRenderer.material.color = dangerColor;
            //DangerousBlockSpawnAnim().Forget();
        }
    }

    #endregion

    #region Update Methods

    private void RigidbodyUpdate()
    {

    }

    #endregion

    #region Spawn
    public void PlaySpawnAnim(float delay = 0)
    {
        switch (build.SpawnAnimType)
        {
            case BlockSpawnAnimType.Fall:
                PlaySpawnWithFallAnim(delay: delay);
                break;
            case BlockSpawnAnimType.Scale:
                PlaySpawnWithScaleAnim(delay: delay).Forget();
                break;
            case BlockSpawnAnimType.Siklon:
                PlaySpawnWithSiklonAnim();
                break;
            default:
                break;
        }
    }

    public void PlaySpawnWithFallAnim(float delay = 0)
    {
        spawnAnimIsFinished = false;
        collider.enabled = false;
        float extraY = UnityEngine.Random.Range(20f, 30f);
        Vector3 startPos = defaults.position + Vector3.up * extraY;
        Vector3 endPos = defaults.position;

        transform.position = startPos;

        Sequence sequence = DOTween.Sequence();

        Tween t = transform.DOMove(endPos, 1.5f).SetDelay(delay).SetEase(Ease.Linear).OnComplete( () =>
        {
            effectController.Sizer.PlayEffect("Fall Popup");
            collider.enabled = true;
            spawnAnimIsFinished = true;
        });

        sequence.Append(t);
        sequence.Play();

    }

    public async UniTaskVoid PlaySpawnWithScaleAnim(float delay = 0)
    {
        spawnAnimIsFinished = false;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = meshDefaults.localScale;

        meshTrs.localScale = startScale;

        AnimationCurve curve = CurveManager.GetCurve("smooth");

        await UniTask.Delay(TimeSpan.FromSeconds(delay));


        meshTrs.DOScale(endScale, 1f).SetEase(Ease.OutElastic).SetDelay(delay).OnComplete(() => spawnAnimIsFinished = true);
    }

    public void PlaySpawnWithSiklonAnim()
    {

    }

    private async UniTaskVoid DangerousBlockSpawnAnim()
    {
        await UniTask.WaitUntil(() => spawnAnimIsFinished);

        Color targetColor = BlockManager.DangerousBlockColor;
        meshDefaults.color = targetColor;



    }

    #endregion

    #region Set State
    public async UniTaskVoid SetBlockState(BlockState newState,float delay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));


        BlockState oldState = state;
        state = newState;


        if(state is BlockState.Static)
        {
            Static();
        }
        else if(state is BlockState.Free)
        {
            if (oldState is BlockState.Anim)
                return;

            Free();

        }
        else if(state is BlockState.InPlayer)
        {
            InPlayer();
        }
    }

    public void Static()
    {
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        rigid.isKinematic = true;
        
    }

    public void Free()
    {
        rigid.constraints = RigidbodyConstraints.None;
        rigid.isKinematic = false;

        transform.SetParent(null);
        TryAddVelocity();
    }

    public void InPlayer()
    {
        PlayerController.instance.Blocker.AddBlockInAllBlockList(this);
        PlayerController.instance.Blocker.AddBlockInInBlockList(this);

        toPlayerSequence.Kill();
        rigid.constraints = RigidbodyConstraints.FreezeAll;
        collider.enabled = false;
        transform.SetParent(playerPos);
        TryAllBlockFree();

    }
    #endregion

    #region Effects

    public void SetActiveTrail(bool active,bool clear=false)
    {
        if (clear)
            trail.Clear();

        trail.enabled = active;
    }

    #endregion

    #region Others

    public void SetFreezeBody(bool active)
    {
        RigidbodyConstraints constraints = active ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        rigid.constraints = constraints;
    }

    public void CollectPhaseFinish()
    {


        SetFreezeBody(active: false);
        
        Vector3 pos = transform.position;
        float yOffset = UnityEngine.Random.Range(25f, 30f);
        float duration = UnityEngine.Random.Range(1.3f, 1.6f);
        float delay = UnityEngine.Random.Range(0f, 0.3f);
        pos.y += yOffset;
        AnimationCurve curve = CurveManager.GetCurve("smooth V2");
        transform.DOMove(pos, duration).SetDelay(delay).SetEase(curve).OnComplete(() => gameObject.SetActive(value: false));
    }
    public async UniTaskVoid ToPlayer()
    {

        PlayerController.instance.Blocker.AddBlockInAllBlockList(this);
        toPlayerSequence = DOTween.Sequence();

        gameObject.layer = 9;

        SetBlockState(BlockState.Anim).Forget();
        Transform firstParent = PlayerController.instance.transform;
        Transform secondParent = PlayerController.instance.BlockParent;

        Vector3 targetPos = firstParent.InverseTransformPoint(PlayerController.instance.Positioner.GetRandomPos());
        transform.SetParent(firstParent);

        Vector3 targetRot = Vector3.one * UnityEngine.Random.Range(-360, +360);
        Vector3 pos = transform.position;
        Vector3 dir = firstParent.position - transform.position;

        float targetY = pos.y + 1;

        AnimationCurve curve = CurveManager.GetCurve("smooth");

        float delay = UnityEngine.Random.Range(0.05f, 0.15f);

        effectController.Sizer.PlayEffect("PopUp");
        effectController.Shader.Play("Highlight");

        //Tween firstTween = transform.DOMoveY(targetY, 0.15f).SetEase(Ease.Linear);
        Tween posTween = transform.DOLocalJump(targetPos, 3.5f, 1, 0.8f).SetEase(Ease.Linear);
        Tween rotTween = transform.DOLocalRotate(targetRot, 0.8f).SetEase(curve);

        //toPlayerSequence.Append(firstTween);
        toPlayerSequence.Append(posTween);
        toPlayerSequence.Join(rotTween);
        toPlayerSequence.Play();

        TryAllBlockFree();
    }
    public void ChangeRandomColor()
    {
        Color color = ColorManager.GetRandomColor();
        meshRenderer.material.color = color;
        meshDefaults.color = color;
    }
    public float GetDistance()
    {
        return Vector3.Distance(playerPos.position, transform.position);
    }
    public float GetDistance(Transform targetPos)
    {
        return Vector3.Distance(transform.position, targetPos.position);
    }
    private void UpdateOtherBlocks()
    {
        otherBlocks.Clear();
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, rayDistance, blockLayer))
        {
            Block block = hit.collider.GetComponent<Block>();
            otherBlocks.Add(block);
        }
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, blockLayer))
        {
            Block block = hit.collider.GetComponent<Block>();
            otherBlocks.Add(block);
        }
        if (Physics.Raycast(transform.position, Vector3.forward, out hit, rayDistance, blockLayer))
        {
            Block block = hit.collider.GetComponent<Block>();
            otherBlocks.Add(block);
        }
        if (Physics.Raycast(transform.position, Vector3.back, out hit, rayDistance, blockLayer))
        {
            Block block = hit.collider.GetComponent<Block>();
            otherBlocks.Add(block);
        }
        if (Physics.Raycast(transform.position, Vector3.right, out hit, rayDistance, blockLayer))
        {
            Block block = hit.collider.GetComponent<Block>();
            otherBlocks.Add(block);
        }
        if (Physics.Raycast(transform.position, Vector3.left, out hit, rayDistance, blockLayer))
        {
            Block block = hit.collider.GetComponent<Block>();
            otherBlocks.Add(block);
        }


    }

    public async UniTaskVoid SleepCollider(float delay)
    {
        collider.enabled = false;
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        collider.enabled = true;
    }

    public void SetActiveCollider(bool active)
    {
        collider.enabled = active;
    }
    #endregion

    #region Try
    public void TryAddVelocity()
    {
        if (!build.AddExplosion)
            return;

        Vector3 direction = (transform.position - playerPos.position);
        direction = direction.normalized;
        float power = UnityEngine.Random.Range(build.minPower, build.maxPower);
        rigid.AddForce(direction*power, ForceMode.VelocityChange);

    }
    
    public void TryAllBlockFree()
    {
        if (build.AllBlockAreFree == false)
        {
            build.AllBlocksFree(this);
        }
    }

    public async UniTaskVoid TryPlayAction()
    {
        if(type is BlockType.Dangerous)
        {
            if (state is not BlockState.InPlayer)
                return;

            float delay = BlockManager.DangerousBlockAnimDelay;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
        }
    }
    
    public void TryShoot(EnemyController enemy)
    {
        //Ready?
        transform.SetParent(null);
        SetActiveCollider(active: false);
        gameObject.layer = 8;
        PlayerController.instance.Blocker.RemoveBlockInInBlockList(this);
        

        //Values...
        Vector3 targetPos = enemy.transform.position;

        Vector3 maxOffset = +1 * BlockManager.instance.ShootPosOffset;
        Vector3 minOffset = -1 * BlockManager.instance.ShootPosOffset;
        
        float x = UnityEngine.Random.Range(minOffset.x, maxOffset.x);
        float y = UnityEngine.Random.Range(minOffset.y, maxOffset.y);
        float z = UnityEngine.Random.Range(minOffset.z, maxOffset.z);

        y += 0.5f;

        Vector3 offset;
        offset.x = x;
        offset.y = y;
        offset.z = z;
        targetPos += offset;

        targetPos = Vector3.MoveTowards(targetPos, playerPos.position, 0.4f);
        float distance = Vector3.Distance(transform.position, targetPos);
        float duration = distance / BlockManager.instance.ShootSpeed;
        Ease ease = Ease.Linear;

        //Shoot!
        SetActiveTrail(active: true);
        SetBlockState(BlockState.Shoot).Forget();
        transform.DOMove(targetPos, duration).SetEase(ease).OnComplete( () =>
        {
            enemy.Health.TakeHit(12);
            FXManager.PlayFX("Hit Yellow", transform.position, 0.5f).Forget();
            gameObject.SetActive(false);
        });
    }

    #endregion

}