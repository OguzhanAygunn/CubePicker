using Cysharp.Threading.Tasks;
using DG.Tweening;
using EVERY;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType CollectibleType { get { return type; } }
    public CollectibleState State { get { return state; } }
    public bool IsCollectible { get { return isCollectible; } }
    public bool Visibility { get { return visibility; } }

    [Title("Main")]
    [SerializeField] CollectibleType type;
    [SerializeField] CollectibleState state = CollectibleState.Hide; 
    [SerializeField] bool isCollectible = true;
    [SerializeField] bool visibility = true;
    [SerializeField] DefaultValuesHandler defaults;

    [Space(7)]

    [Title("Visibility")]
    [SerializeField] Transform meshTrs;
    [SerializeField] SpriteRenderer shadowRenderer;

    [SerializeField] DefaultValuesHandler meshDefaults;

    [SerializeField] float scaleDuration;
    [SerializeField] float shadowDuration;

    public void Awake()
    {
        
    }

    public void Start()
    {
        if (visibility == false)
        {
            SetVisibility(visibility: false, force: true, delay: 0).Forget();
        }
    }

    public void PlayAnim()
    {
        if (!visibility)
            return;

        switch (type)
        {
            case CollectibleType.Magnet:

                MagnetCollectible magnet = GetComponent<MagnetCollectible>();
                magnet.PlayAnim().Forget();

                break;
            default:
                break;
        }
    }

    private void UpdateState()
    {
        CollectibleState state = visibility ? CollectibleState.Ready : CollectibleState.Hide;
        this.state = state;
    }

    private async UniTaskVoid SetState(CollectibleState state,float delay = 0)
    {
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        this.state = state;
    }

    public void DeActive()
    {
        Vector3 targetScale = Vector3.zero;
        transform.DOScale(targetScale, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    public async UniTaskVoid SetVisibility(bool visibility, bool force = false, float delay = 0)
    {
        if (delay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

        this.visibility = visibility;

        
        PlayAnim();
        UpdateState();

        Vector3 targetScale = visibility ? meshDefaults.localScale : Vector3.zero;
        float fadeVal = visibility ? 0.8f : 0f;

        float trsDuration = force ? 0f : scaleDuration;
        float shadowDuration = force ? 0f : scaleDuration;

        Ease trsEase = force ? Ease.Linear : Ease.OutElastic;

        meshTrs.DOScale(targetScale, trsDuration).SetEase(trsEase);
        shadowRenderer.DOFade(fadeVal, shadowDuration).OnComplete( () =>
        {
            if (!visibility)
                gameObject.SetActive(value: false);
        });
    }

    public void ActiveVisibility()
    {
        SetVisibility(visibility: true).Forget();
    }

    public virtual void Collect()
    {
        if (state is not CollectibleState.Ready)
            return;

        if (!isCollectible)
            return;


        isCollectible = false;

        SetState(CollectibleState.Collected).Forget();

        switch (type)
        {
            case CollectibleType.Magnet:
                MagnetCollectible magnet = GetComponent<MagnetCollectible>();
                magnet.Collect();
                break;
            default:
                break;
        }

    }
}
