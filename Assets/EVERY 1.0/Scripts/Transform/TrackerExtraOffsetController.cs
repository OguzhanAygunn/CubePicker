using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EVERY
{
    [RequireComponent(typeof(Tracker))]
    public class TrackerExtraOffsetController : MonoBehaviour
    {
        Tracker tracker;
        Vector3 defaultPos;

        public List<ExtraPosOffsetAnim> posAnims;

        public Vector3 posOffset;
        public Vector3 rotOffset;
        public Vector3 scaleOffset;

        private Sequence posSequence;
        private Sequence rotSequence;
        private Sequence scaleSequence;
        private void Start()
        {
            defaultPos = transform.position;
            tracker = GetComponent<Tracker>();

            if(posAnims != null)
            {
                if (posAnims.Count != 0)
                    posAnims.ForEach(anim => anim.Init(this));
            }

        }


        [Button(size: ButtonSizes.Large)]
        public void PlayPosAnim(string id)
        {
            ExtraPosOffsetAnim anim = GetPosAnim(id: id);

            if (anim == null)
                return;

            anim.Play();

        }

        public ExtraPosOffsetAnim GetPosAnim(string id)
        {
            return posAnims.Find(anim => anim.id == id);
        }
    }


    [System.Serializable]
    public class ExtraPosOffsetAnim
    {
        public bool active;
        public string id;
        public List<ExtraPosOffsetAnimInfo> anims;
        public Sequence sequence;
        private TrackerExtraOffsetController controller;
        public bool loop;
        public void Init(TrackerExtraOffsetController controller)
        {
            sequence = DOTween.Sequence();
            this.controller = controller;

            foreach(ExtraPosOffsetAnimInfo anim in anims)
            {
                anim.Init(controller);

                sequence.Append(anim.GetTween());
                
            }
            sequence.Pause();
        }

        private void UpdateSequence()
        {
            sequence = DOTween.Sequence();
            foreach (ExtraPosOffsetAnimInfo anim in anims)
            {
                sequence.Append(anim.GetTween());
            }
            sequence.Pause();
        }

        public void Play()
        {
            UpdateSequence();
            sequence.Play().OnComplete( () =>
            {
                if (loop)
                    Play();
            });
        }

        public void Kill()
        {
            sequence.Kill(complete: false);
            sequence.Pause();
        }
    }

    [System.Serializable]
    public class ExtraPosOffsetAnimInfo
    {
        public bool active;
        public Vector3 targetOffset;
        public float duration;
        public float delay;
        public AnimCurveType curveType;
        [ShowIf("@this.curveType == AnimCurveType.Ease")] public Ease ease;
        [ShowIf("@this.curveType == AnimCurveType.Curve")] public AnimationCurve curve;
        [ShowIf("@this.curveType == AnimCurveType.CurveID")] public string curveID;

        private TrackerExtraOffsetController controller;
        public void Init(TrackerExtraOffsetController controller)
        {
            this.controller = controller;
        }

        public Tween GetTween()
        {
            Tween tween = null;


            switch (curveType)
            {
                case AnimCurveType.Ease:
                    tween = DOTween.To(() => controller.posOffset, x => controller.posOffset = x, targetOffset, duration).SetEase(ease).SetDelay(delay);
                    break;
                case AnimCurveType.Curve:
                    tween = DOTween.To(() => controller.posOffset, x => controller.posOffset = x, targetOffset, duration).SetEase(curve).SetDelay(delay);
                    break;
                case AnimCurveType.CurveID:
                    AnimationCurve _curve = CurveManager.GetCurve("Smooth");
                    tween = DOTween.To(() => controller.posOffset, x => controller.posOffset = x, targetOffset, duration).SetEase(_curve).SetDelay(delay);
                    break;
                default:
                    tween = DOTween.To(() => controller.posOffset, x => controller.posOffset = x, targetOffset, duration).SetEase(ease).SetDelay(delay);
                    break;

            }




            return tween;
        }

    }

}
