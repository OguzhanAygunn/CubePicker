using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager instance;
        public static TimeManager Instance { get { return instance; } }
        [SerializeField] List<TimeScaleEffect> effects;
        public int targetFPS = 60;

        private void Awake()
        {
            Application.targetFrameRate = targetFPS;
        }


        public TimeScaleEffect GetEffect(string id = "test")
        {
            return effects.Find(effect => effect.id == id);
        }

        [Button(size: ButtonSizes.Large)]
        public void PlayEffect(string id)
        {
            TimeScaleEffect effect = GetEffect(id: id);

            effect.Play();
        }

    }



    [System.Serializable]
    public class TimeScaleEffect
    {
        [Title("Main")]
        public bool active;
        public string id;

        [Space(6)]

        [Title("Start Anim")]
        public float startTargetTime;
        public float startDelay;
        public float startDuration;
        public AnimCurveType startAnimType;
        [ShowIf("@this.startAnimType == AnimCurveType.Ease")] public Ease startEase;
        [ShowIf("@this.startAnimType == AnimCurveType.Curve")] public AnimationCurve startCurve;
        [ShowIf("@this.startAnimType == AnimCurveType.CurveID")] public string startCurveID;

        [Space(6)]

        [Title("End Anim")]
        public float endTargetTime;
        public float endDelay;
        public float endDuration;
        public AnimCurveType endAnimType;
        [ShowIf("@this.endAnimType == AnimCurveType.Ease")] public Ease endEase;
        [ShowIf("@this.endAnimType == AnimCurveType.Curve")] public AnimationCurve endCurve;
        [ShowIf("@this.endAnimType == AnimCurveType.CurveID")] public string endCurveID;

        [Space(6)]

        [Title("Events")]
        public bool activeEventPanel;
        [ShowIf("@this.activeEventPanel")] public List<EventInfo> events;

        public void Play()
        {
            PlayStart().Forget();
        }

        public async UniTaskVoid PlayStart()
        {
            if(startAnimType is AnimCurveType.Ease)
            {
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, startTargetTime, startDuration).SetEase(startEase).SetDelay(startDelay);
            }
            else if(startAnimType is AnimCurveType.Curve)
            {
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, startTargetTime, startDuration).SetEase(startCurve).SetDelay(startDelay);
            }
            else if(startAnimType is AnimCurveType.CurveID)
            {
                AnimationCurve curve = CurveManager.GetCurve(startCurveID);

                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, startTargetTime, startDuration).SetEase(curve).SetDelay(startDelay);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(startDuration));

            PlayEnd();
        }

        public async void PlayEnd()
        {
            if (startAnimType is AnimCurveType.Ease)
            {
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endTargetTime, endDuration).SetEase(endEase).SetDelay(endDelay);
            }
            else if (startAnimType is AnimCurveType.Curve)
            {
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endTargetTime, endDuration).SetEase(endCurve).SetDelay(endDelay);
            }
            else if (startAnimType is AnimCurveType.CurveID)
            {
                AnimationCurve curve = CurveManager.GetCurve(endCurveID);

                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, endTargetTime, endDuration).SetEase(curve).SetDelay(endDelay);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(endDuration));

            events.ForEach(_event => _event.PlayEvent().Forget());
        }

    }
}

