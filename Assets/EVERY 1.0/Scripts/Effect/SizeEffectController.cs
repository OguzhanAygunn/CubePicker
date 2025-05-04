using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EVERY
{
    public class SizeEffectController : MonoBehaviour
    {
        [SerializeField] List<SizeEffect> effects;


        private void Awake()
        {

            effects.ForEach(effect => effect.Init());
        }

        private void Start()
        {
        }


        public SizeEffect GetEffect(string id = "test")
        {
            return effects.Find(effect => effect.id == id);
        }

        [Button(size: ButtonSizes.Large)]
        public void PlayEffect(string id)
        {
            SizeEffect effect = GetEffect(id: id);

            if (effect == null)
                return;

            effect.Play();
        }

        public void KillTween()
        {
            //DOTween.Kill("Scale Effect");
        }

    }


    [System.Serializable]
    public class SizeEffect
    {
        [Title("Main")]
        public bool active;
        public string id;
        public Transform effectObj;

        [SerializeField] List<SizeEffectDetail> effects;


        public void Init()
        {
            effects.ForEach(effect => effect.AssignObj(effectObj));
            effects.ForEach(effect => effect.Init());
        }


        public async void Play()
        {
            bool kill = true;
            foreach(SizeEffectDetail effect in effects)
            {
                effect.Play(kill: kill);
                kill = false;
                await UniTask.WaitUntil(() => effect.active == false);
            }
        }

    }

    [System.Serializable]
    public class SizeEffectDetail
    {
        #region Main (Title)
        [Title("Main")]
        public bool active;
        public SizeAnimType animType;
        [HideInInspector] public Transform trs;
        #endregion

        #region To Target
        [ShowIf("@this.animType == SizeAnimType.ToTarget")]
        [Title("To Target")]
        [ShowIf("@this.animType == SizeAnimType.ToTarget")][LabelText("Use Default Scale")] public bool toTargetUseTheDefaultScale;
        [ShowIf("@this.animType == SizeAnimType.ToTarget && this.toTargetUseTheDefaultScale == false")] [LabelText("Target Scale")]public Vector3 toTargetScale;
        [ShowIf("@this.animType == SizeAnimType.ToTarget")][LabelText("Duration")] public float toTargetDuration;
        [ShowIf("@this.animType == SizeAnimType.ToTarget")][LabelText("Delay")] public float toTargetDelay;

        [Space(6)]
        #endregion

        #region Shake
        [ShowIf("@this.animType == SizeAnimType.Shake")]
        [Title("Shake")]
        [ShowIf("@this.animType == SizeAnimType.Shake")][LabelText("Duration")] public float shakeDuration = 1f;
        [ShowIf("@this.animType == SizeAnimType.Shake")] public float strength = 1f;
        [ShowIf("@this.animType == SizeAnimType.Shake")] public int vibrato = 10;
        [ShowIf("@this.animType == SizeAnimType.Shake")] public int randomness = 90;
        [ShowIf("@this.animType == SizeAnimType.Shake")] public bool fadeOut = true;
        [ShowIf("@this.animType == SizeAnimType.Shake")][LabelText("Delay")] public float shakeDelay;

        [Space(10)]
        #endregion

        #region Three Axis
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")]
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][Title("Three Axis")]
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("Use Default Scale")] public bool threeAxisUseTheDefaultScale;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisUseTheDefaultScale == false")][LabelText("Three Axis Scale")] public Vector3 threeAxisScale;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("Anim Type")] public AnimCurveType threeAxisAnimType;

        [Space(15)]

        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("X Axis Duration")] public float xAxisDuration;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("Y Axis Duration")] public float yAxisDuration;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("Z Axis Duration")] public float zAxisDuration;

        [Space(15)]

        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("X Axis Delay")] public float xAxisDelay;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("Y Axis Delay")] public float yAxisDelay;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis")][LabelText("Z Axis Delay")] public float zAxisDelay;

        [Space(15)]

        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.Ease")][LabelText("X Axis Ease")] public Ease xEase;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.Ease")][LabelText("Y Axis Ease")] public Ease yEase;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.Ease")][LabelText("Z Axis Ease")] public Ease zEase;
        
        [Space(10)]

        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.Curve")][LabelText("X Axis Curve")] public AnimationCurve xCurve;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.Curve")][LabelText("Y Axis Curve")] public AnimationCurve yCurve;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.Curve")][LabelText("Z Axis Curve")] public AnimationCurve zCurve;
        
        [Space(15)]

        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.CurveID")][LabelText("X Axis Curve ID")] public string xCurveID;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.CurveID")][LabelText("Y Axis Curve ID")] public string yCurveID;
        [ShowIf("@this.animType == SizeAnimType.ThreeAxis && this.threeAxisAnimType == AnimCurveType.CurveID")][LabelText("Z Axis Curve ID")] public string zCurveID;


        [Space(6)]

        #endregion

        #region Curves
        [ShowIf("@this.animType != SizeAnimType.ThreeAxis")]
        [Title("Curves")]
        public AnimCurveType animCurveType;
        [ShowIf("@this.animCurveType == AnimCurveType.Ease && this.animType != SizeAnimType.ThreeAxis")] public Ease ease;
        [ShowIf("@this.animCurveType == AnimCurveType.Curve && this.animType != SizeAnimType.ThreeAxis")] public AnimationCurve curve;
        [ShowIf("@this.animCurveType == AnimCurveType.CurveID && this.animType != SizeAnimType.ThreeAxis")] public string curveID;
        #endregion

        #region Events
        [Space(10)]
        public bool eventsPanelActive;
        [ShowIf("@this.eventsPanelActive == true")] public List<EventInfo> events;
        #endregion

        #region Values
        private Vector3 targetScale;
        private float duration;
        private DefaultValuesHandler defaultValues;
        #endregion

        #region Main Funcs
        public void Init()
        {
            defaultValues = trs.GetComponent<DefaultValuesHandler>();


            if (animType is SizeAnimType.ThreeAxis)
            {
                targetScale = threeAxisScale;
                
                if (threeAxisUseTheDefaultScale)
                    targetScale = defaultValues.localScale;
            }
                
            else if (animType is SizeAnimType.ToTarget)
            {
                targetScale = toTargetScale;
                duration = toTargetDuration;

                if (toTargetUseTheDefaultScale)
                    targetScale = defaultValues.localScale;
            }

            else if(animType is SizeAnimType.Shake)
            {
                 duration = shakeDuration;
            }


        }

        public float GetMaxDuration()
        {
            float duration;

            switch (animType)
            {
                case SizeAnimType.ToTarget:
                    duration = toTargetDuration;
                    break;
                case SizeAnimType.Shake:
                    duration = shakeDuration;
                    break;
                case SizeAnimType.ThreeAxis:
                    List<float> durations = new List<float>() { xAxisDuration, yAxisDuration, zAxisDuration };

                    duration = durations.OrderByDescending(x => x).ToList()[0];

                    break;
                default:
                    duration = 1;
                    break;

            }

            return duration;
        }

        public float GetMaxDelay()
        {
            List<float> delays = new List<float>() { xAxisDelay, yAxisDelay, zAxisDelay };
            float delay = delays.OrderByDescending((x) => x).ToList()[0];

            return delay;
        }

        public float GetDelay()
        {
            float delay = 0;

            return delay;
        }

        public void AssignObj(Transform _trs)
        {
            trs = _trs;
        }
        #endregion

        #region Play Funcs
        public void Play(bool kill = false)
        {
            if (active)
                return;
            Init();
            active = true;

            //if (kill)
                //DOTween.Kill(targetOrId: "Scale Effect", complete: false);

            switch (animType)
            {
                case SizeAnimType.ToTarget:
                    PlayToTarget().Forget();
                    break;
                case SizeAnimType.Shake:
                    PlayShake().Forget();
                    break;
                case SizeAnimType.ThreeAxis:
                    PlayThreeAxis().Forget();
                    break;
            }
        }

        public async UniTaskVoid PlayToTarget()
        {

            PlayEvents();

            await UniTask.Delay(TimeSpan.FromSeconds(toTargetDelay));

            if (toTargetUseTheDefaultScale)
                targetScale = defaultValues.localScale;
            else
                targetScale = toTargetScale;

            if(animCurveType is AnimCurveType.Ease)
            {
                trs.DOScale(targetScale, duration).SetEase(ease).SetId("Scale Effect").SetAutoKill(true);
            }
            else if(animCurveType is AnimCurveType.Curve)
            {
                trs.DOScale(targetScale, duration).SetEase(curve).SetId("Scale Effect").SetAutoKill(true);
            }
            else if(animCurveType is AnimCurveType.CurveID)
            {
                AnimationCurve _curve = CurveManager.GetCurve(curveID);
                trs.DOScale(targetScale, duration).SetEase(_curve).SetId("Scale Effect").SetAutoKill(true);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            active = false;
        }

        public async UniTaskVoid PlayShake()
        {

            PlayEvents();

            await UniTask.Delay(TimeSpan.FromSeconds(shakeDelay));


            if (animCurveType is AnimCurveType.Ease)
            {
                trs.DOShakeScale(duration, strength,vibrato,randomness,fadeOut).SetEase(ease).SetId("Scale Effect");
            }
            else if (animCurveType is AnimCurveType.Curve)
            {
                trs.DOShakeScale(duration, strength, vibrato, randomness, fadeOut).SetEase(curve).SetId("Scale Effect");
            }
            else if (animCurveType is AnimCurveType.CurveID)
            {
                AnimationCurve _curve = CurveManager.GetCurve(curveID);
                trs.DOShakeScale(duration, strength, vibrato, randomness, fadeOut).SetEase(ease).SetId("Scale Effect");
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            active = false;
        }

        public async UniTaskVoid PlayThreeAxis()
        {

            PlayEvents();

            float xScale = threeAxisUseTheDefaultScale ? defaultValues.localScale.x : threeAxisScale.x;
            float yScale = threeAxisUseTheDefaultScale ? defaultValues.localScale.y : threeAxisScale.y;
            float zScale = threeAxisUseTheDefaultScale ? defaultValues.localScale.z : threeAxisScale.z;

            if (animCurveType is AnimCurveType.Ease)
            {
                trs.DOScaleX(xScale, xAxisDuration).SetEase(xEase).SetDelay(xAxisDelay).SetId("Scale Effect");
                trs.DOScaleY(yScale, yAxisDuration).SetEase(yEase).SetDelay(yAxisDelay).SetId("Scale Effect");
                trs.DOScaleZ(zScale, zAxisDuration).SetEase(zEase).SetDelay(zAxisDelay).SetId("Scale Effect");
            }
            else if(animCurveType is AnimCurveType.Curve)
            {
                trs.DOScaleX(xScale, xAxisDuration).SetEase(xCurve).SetDelay(xAxisDelay).SetId("Scale Effect");
                trs.DOScaleY(yScale, yAxisDuration).SetEase(yCurve).SetDelay(yAxisDelay).SetId("Scale Effect");
                trs.DOScaleZ(zScale, zAxisDuration).SetEase(zCurve).SetDelay(zAxisDelay).SetId("Scale Effect");
            }
            else if(animCurveType is AnimCurveType.CurveID)
            {
                AnimationCurve _xCurve = CurveManager.GetCurve(xCurveID);
                AnimationCurve _yCurve = CurveManager.GetCurve(yCurveID);
                AnimationCurve _zCurve = CurveManager.GetCurve(zCurveID);

                trs.DOScaleX(xScale, xAxisDuration).SetEase(_xCurve).SetDelay(xAxisDelay).SetId("Scale Effect");
                trs.DOScaleY(yScale, yAxisDuration).SetEase(_yCurve).SetDelay(yAxisDelay).SetId("Scale Effect");
                trs.DOScaleZ(zScale, zAxisDuration).SetEase(_zCurve).SetDelay(zAxisDelay).SetId("Scale Effect");
            }

            float delay = GetMaxDuration() + GetMaxDelay();
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            active = false;
        }

        public void PlayEvents()
        {
            if (!eventsPanelActive)
                return;

            events.ForEach(e => { e.PlayEvent().Forget(); });
        }

        #endregion
    }
}

