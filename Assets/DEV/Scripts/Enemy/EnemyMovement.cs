using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public EnemyMovePhase MovePhase { get { return movePhase; } }

    [Title("Main")]
    [SerializeField] bool active = true;
    [SerializeField] bool isMove;
    [SerializeField] EnemyMovePhase movePhase = EnemyMovePhase.InWater;

    [Space(7)]
    
    [Title("Move")]
    [SerializeField] Transform target;
    [SerializeField] float waterSpeed;
    [SerializeField] float groundSpeed;
    [SerializeField] float lookSpeed = 5;

    [Space(7)]

    [Title("Components")]
    [SerializeField] CapsuleCollider coll;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rigid;
    [SerializeField] DefaultValuesHandler defaults;
    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }



    public void Init()
    {
        Transform ground = GroundController.instance.transform;
        target = ground;
        Vector3 lookPos = new Vector3(ground.position.x, transform.position.y, ground.position.z);
        SetActive(active: true);
        transform.LookAt(lookPos);
        SetMovePhase(EnemyMovePhase.InWater);
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }

    public void Move()
    {
        if (!active)
            return;

        rigid.velocity = Vector3.zero;
        float speed;

        if(movePhase is EnemyMovePhase.InGround)
            speed = groundSpeed;
        else if(movePhase is EnemyMovePhase.InWater)
            speed = waterSpeed;
        else
            speed = 0;

        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        Vector3 pos = rigid.position;
        pos = Vector3.MoveTowards(pos, targetPos, speed * Time.fixedDeltaTime);

        if (movePhase is EnemyMovePhase.InWater)
        {
            rigid.MovePosition(pos);
        }
        else if(movePhase is EnemyMovePhase.InGround)
        {
            rigid.MovePosition(pos);
            Look();
        }
    }


    private void Look()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 0.8f)
            return;

        Vector3 lookPos = target.position - transform.position;
        lookPos.y = transform.position.y + 2f;

        Quaternion look = Quaternion.LookRotation(lookPos);

        transform.rotation = Quaternion.Slerp(transform.rotation, look, lookSpeed * Time.fixedDeltaTime);
    }

    public void TryToGroundPhase()
    {
        if (movePhase is not EnemyMovePhase.InWater)
            return;

        coll.enabled = false;
        SetMovePhase(EnemyMovePhase.InAir);

        Vector3 pos = transform.position;
        Transform groundPoint = GroundController.instance.GetNearestPoint(pos);
        Vector3 groundPos = groundPoint.position;


        float jumpPower = EnemyManager.instance.jumpPower;
        float jumpDuration = EnemyManager.instance.jumpDuration;

        transform.DOJump(groundPos, jumpPower, 1, jumpDuration).SetEase(Ease.Linear).OnComplete( () =>
        {
            coll.enabled = true;
            SetMovePhase(EnemyMovePhase.InGround);
            target = PlayerController.instance.transform;
        });
    }

    public void SetMovePhase(EnemyMovePhase phase)
    {
        movePhase = phase;
        string animID;
        switch (movePhase)
        {
            case EnemyMovePhase.InAir:
                Vector3 effectPos = transform.position + Vector3.up;
                FXManager.PlayFX("Block Water", effectPos, 3f).Forget();
                animID = "Jump";
                break;
            case EnemyMovePhase.InGround:
                animID = "Run";
                break;
            case EnemyMovePhase.InWater:
                animID = "Swimming";
                break;
            case EnemyMovePhase.Kill:
                animID = "Kill";
                break;
            default:
                animID = "Swimming";
                break;
        }
        animator.SetTrigger(animID);
    }

    public async UniTaskVoid SetRigidbody(bool active,float delay = 0)
    {
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        RigidbodyConstraints constraints = active ? defaults.constraints : RigidbodyConstraints.FreezeAll;
    }

    public async UniTaskVoid SetActiveCollider(bool active,float delay = 0)
    {

        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        coll.enabled = active;
    }

    public async UniTaskVoid SetActiveAnimator(bool active,float delay = 0)
    {

        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        animator.enabled = active;
    }

    public async UniTaskVoid SetAnimatorSpeed(float speed = 1,float delay = 0)
    {
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        animator.speed = speed;
    }


    public void Kill()
    {
        SetActive(false);
        Vector3 pos = KillPos();
        SetRigidbody(false).Forget();
        transform.DOJump(pos, 5, 1, 1f).SetEase(Ease.Linear).OnComplete( () =>
        {
            Vector3 effectSpawnPos = transform.position + Vector3.up;
            FXManager.PlayFX("Water Enemy", effectSpawnPos , 3f).Forget();
            pos.y -= 7f;
            transform.DOMove(pos, 2f);
        });
    }

    public Vector3 KillPos()
    {
        Vector3 pos = Vector3.zero;

        float killPower = EnemyManager.instance.killPower;
        LayerMask waterLayer = EnemyManager.instance.waterLayer;


        float power = UnityEngine.Random.Range(killPower, killPower + 2f);
        Transform ground = GroundController.instance.transform;


        Vector3 rayPos = ground.position;
        rayPos.y = transform.position.y; ;
        rayPos += transform.TransformDirection(Vector3.back * power);
        //transform.forward * -power;

        RaycastHit hit;
        if(Physics.Raycast(rayPos,Vector3.down, out hit, 1000, waterLayer))
        {
            pos = hit.point;
            pos.y -= 1.5f;
        }

        return pos;
    }

}
