using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace EVERY
{
    public class CameraController : MonoBehaviour
    {
        #region Main Values
        public static CameraController instance;
        public static CameraController Instance { get { return instance; } }


        [Title("Main")]
        [SerializeField] bool active;
        [SerializeField] string startID;

        [Space(6)]

        [Title("Components")]
        [SerializeField] CinemachineBrain brain;
        [SerializeField] Camera camera;
        [SerializeField] DefaultValuesHandler defaults;
        public Camera Camera { get { return camera; } }
        public DefaultValuesHandler Defaults { get { return defaults; } }
        [Space(6)]

        [LabelText("SHOW CAMS INFO")] public bool showCamInfos = true;
        [Space(4)]
        [LabelText("SHOW ANIMS")] public bool showAnims;

        [Space(4)]

        [Title("Current Camera")]
        [ShowIf(nameof(showCamInfos))][SerializeField] CinemachineVirtualCamera virtualCamera;
        [ShowIf(nameof(showCamInfos))][SerializeField] CinemachineFramingTransposer transposer;

        [Space(6)]

        [Title("Cameras")]
        [ShowIf(nameof(showCamInfos))][SerializeField] CamInfo currentCamera;
        [ShowIf(nameof(showCamInfos))][SerializeField] List<CamInfo> cameras;
        public CamInfo CurrentCamera { get { return currentCamera; } }

        [Space(6)]


        [Title("Offset Anims")]
        [ShowIf(nameof(showAnims))][SerializeField] List<CamOffsetAnim> offsetAnims;
        [ShowIf(nameof(showAnims))][SerializeField] List<CamDistanceAnim> distanceAnims;
        [ShowIf(nameof(showAnims))][SerializeField] List<CamFOVAnim> fovAnims;
        [ShowIf(nameof(showAnims))][SerializeField] List<ShakeAnim> shakeAnims;

        #endregion

        #region Awake & Start & Init
        private void Awake()
        {
            instance = (!instance) ? this : instance;
            cameras.ForEach(cam => cam.Init());
            SetCam(id: startID).Forget();
            fovAnims.ForEach(anim => anim.Init());

        }

        public void Init()
        {

        }

        private void Start()
        {
            

        }
        #endregion

        #region CAM FUNCS
        public CamInfo GetCamInfo(string id)
        {
            return cameras.Find(cam => cam.id == id);
        }

        public async UniTaskVoid SetCam(string id,float delay=0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            DeActiveAllCamera();

            CamInfo camInfo = GetCamInfo(id);
            currentCamera = camInfo;
            
            if (currentCamera == null)
                return;

            virtualCamera = camInfo.virtualCamera;
            transposer = camInfo.transposer;
            
            virtualCamera.gameObject.SetActive(true);
        }

        public void DeActiveAllCamera()
        {
            cameras.ForEach(cam => cam.virtualCamera.gameObject.SetActive(false));
        }
        #endregion

        #region UPDATE
        private void Update()
        {
            if (!active)
                return;

            if (currentCamera == null)
                return;

            currentCamera.Update();
        }
        #endregion

        #region PLAY ANIM
        [ShowIf(nameof(showAnims))]
        [Button(size: ButtonSizes.Large)]
        public async UniTaskVoid PlayCamOffsetAnim(string id, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            CamOffsetAnim anim = GetOffsetAnim(id);

            if (anim == null)
                return;

            anim.Play(currentCamera).Forget();
            
        }

        [ShowIf(nameof(showAnims))]
        [Button(size: ButtonSizes.Large)]
        public async UniTaskVoid PlayFOVAnim(string id , float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            CamFOVAnim info = GetFOVAnim(id: id);

            if (info == null) return;


            info.Play(currentCamera).Forget();

        }
        [ShowIf(nameof(showAnims))]
        [Button(size: ButtonSizes.Large)]
        public async UniTaskVoid PlayDistanceAnim(string id, float delay=0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            CamDistanceAnim info = GetDistanceAnim(id: id);

            if (info == null)
                return;

            info.Play(camInfo: currentCamera).Forget();
            
        }

        [ShowIf(nameof(showAnims))]
        [Button(size: ButtonSizes.Large)]
        public async UniTaskVoid PlayShakeAnim(string id, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            ShakeAnim anim = GetShakeAnim(id: id);

            if (anim == null)
                return;

            anim.Play().Forget();

        }
        #endregion

        #region GET ANIM
        public CamFOVAnim GetFOVAnim(string id)
        {
            return fovAnims.Find(anim => anim.id == id);
        }
        public CamOffsetAnim GetOffsetAnim(string id)
        {
            return offsetAnims.Find(anim => anim.id == id);
        }
        public CamDistanceAnim GetDistanceAnim(string id)
        {
            return distanceAnims.Find(anim => anim.id == id);
        }
        public ShakeAnim GetShakeAnim(string id) {
            return shakeAnims.Find(anim => anim.id == id);
        }
        #endregion
    }

    #region ANIM AND CAM CLASSES

    #region CAM INFO
    [System.Serializable]
    public class CamInfo
    {
        [Title("Main")]
        public string id;
        public CinemachineVirtualCamera virtualCamera;
        public CinemachineFramingTransposer transposer;
        public CinemachineComposer composer;
        public CinemachineBasicMultiChannelPerlin perlin;
        public float fov;
        public float size;


        [LabelText("Show Offset Panel")] public bool showOffsetPanel;

        [ShowIf(nameof(showOffsetPanel))] public Vector3 defaultposOffset;
        [ShowIf(nameof(showOffsetPanel))] public Vector3 defaultrotOffset;
        
        [Space(7)]
        
        [ShowIf(nameof(showOffsetPanel))] public Vector3 posOffset;
        [ShowIf(nameof(showOffsetPanel))] public Vector3 rotOffset;
        [ShowIf(nameof(showOffsetPanel))] public float posOffsetSpeed;
        [ShowIf(nameof(showOffsetPanel))] public float rotOffsetSpeed;
        [Space(7)]

        [ShowIf(nameof(showOffsetPanel))] public Vector3 extraPosOffset;
        [ShowIf(nameof(showOffsetPanel))] public Vector3 extraRotOffset;


        public void Init()
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            fov = virtualCamera.m_Lens.FieldOfView;
            if (transposer)
            {
                size = transposer.m_CameraDistance;
                defaultposOffset = transposer.m_TrackedObjectOffset;
                defaultrotOffset = composer.m_TrackedObjectOffset;
            }
        }

        public void PosOffset()
        {
            Vector3 offset = transposer.m_TrackedObjectOffset;
            Vector3 targetOffset = defaultposOffset + posOffset + extraPosOffset;
            offset = Vector3.Lerp(offset, targetOffset, posOffsetSpeed * Time.deltaTime);
            transposer.m_TrackedObjectOffset = offset;
        }

        public void Update()
        {
            PosOffset();
        }

    }
    #endregion

    #region DISTANCE ANIM
    [System.Serializable]
    public class CamDistanceAnim
    {

        [Title("Main")]
        public bool active;
        public string id;
        public bool bruteForce = true;
        public List<CamDistanceAnimInfo> anims;


        public async UniTaskVoid Play(CamInfo camInfo)
        {
            if (!bruteForce && active)
                return;

            active = true;
            CamDistanceAnimInfo info = null;
            foreach (CamDistanceAnimInfo anim in anims)
            {
                anim.Play(camInfo: camInfo).Forget();
                info = anim;
                await UniTask.WaitUntil(() => anim.active == false);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(info.duration));

            active = false;
        }

    }

    [System.Serializable]
    public class CamDistanceAnimInfo
    {
        [Title("Anim")]
        public bool active;
        public bool useDefaultDistance;
        [ShowIf("@this.useDefaultDistance == false")] public float targetDistance;
        public float duration;
        public float delay;
        public AnimCurveType curveType;
        [ShowIf("@this.curveType == AnimCurveType.Ease")] public Ease ease;
        [ShowIf("@this.curveType == AnimCurveType.Curve")] public AnimationCurve curve;
        [ShowIf("@this.curveType == AnimCurveType.CurveID")] public string curveID;
        private float target;
        private Camera cam;
        private DefaultValuesHandler defaults;
        private CamInfo camInfo;

        [Space(6)]

        [Title("EVENT")]
        public bool useTheEvents;
        [ShowIf(nameof(useTheEvents))] public List<EventInfo> events;

        public void Init()
        {
            cam = CameraController.instance.Camera;
        }

        public async UniTaskVoid Play(CamInfo camInfo)
        {

            if (useTheEvents)
                events.ForEach(e => e.PlayEvent().Forget());

            active = true;

            CinemachineFramingTransposer transposer = camInfo.transposer;
            defaults = camInfo.virtualCamera.GetComponent<DefaultValuesHandler>();

            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            target = targetDistance;

            if (useDefaultDistance)
                target = defaults.defaultDistance;


            if (curveType is AnimCurveType.Ease)
            {
                DOTween.To(() => transposer.m_CameraDistance, x => transposer.m_CameraDistance = x, target, duration).SetEase(ease);
                
            }
            else if (curveType is AnimCurveType.Curve)
            {
                DOTween.To(() => transposer.m_CameraDistance, x => transposer.m_CameraDistance = x, target, duration).SetEase(curve);
            }
            else if (curveType is AnimCurveType.CurveID)
            {
                AnimationCurve curve = CurveManager.GetCurve(curveID);
                DOTween.To(() => transposer.m_CameraDistance, x => transposer.m_CameraDistance = x, target, duration).SetEase(curve);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));


            active = false;
        }

    }
    #endregion

    #region FOV ANIM
    [System.Serializable]
    public class CamFOVAnim
    {
        [Title("Main")]
        public bool active;
        public bool bruteForce = true;
        public string id;
        public List<CamFOVAnimInfo> anims;

        public void Init()
        {
            anims.ForEach(anim => anim.Init());
        }

        public async UniTaskVoid Play(CamInfo camInfo)
        {
            if (!bruteForce && active)
                return;

            active = true;
            CamFOVAnimInfo info = null;
            foreach(CamFOVAnimInfo anim in anims)
            {
                anim.Play(camInfo: camInfo).Forget();
                info = anim;
                await UniTask.WaitUntil(() => anim.active == false);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(info.duration));
        
            active = false;
        }

    }

    [System.Serializable]
    public class CamFOVAnimInfo
    {
        #region Values
        [Title("Anim")]
        public bool active;
        public bool useDefaultFOV;
        [ShowIf("@this.useDefaultFOV == false")] public float targetFOV;
        public float duration;
        public float delay;
        public AnimCurveType curveType;
        [ShowIf("@this.curveType == AnimCurveType.Ease")] public Ease ease;
        [ShowIf("@this.curveType == AnimCurveType.Curve")] public AnimationCurve curve;
        [ShowIf("@this.curveType == AnimCurveType.CurveID")] public string curveID;

        private float target;
        private Camera cam;
        private DefaultValuesHandler defaults;
        
        [Space(6)]

        [Title("EVENT")]
        public bool useTheEvents;
        [ShowIf(nameof(useTheEvents))] public List<EventInfo> events;
        #endregion
        public void Init()
        {
            cam      = CameraController.instance.Camera;
            defaults = CameraController.instance.Defaults;
        }

        public async UniTaskVoid Play(CamInfo camInfo)
        {

            if (useTheEvents)
                events.ForEach(e => e.PlayEvent().Forget());

            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            target = targetFOV;

            if (useDefaultFOV)
                target = defaults.defaultFOV;


            if(curveType is AnimCurveType.Ease)
            {
                cam.DOFieldOfView(target, duration).SetEase(ease);
            }
            else if(curveType is AnimCurveType.Curve)
            {
                cam.DOFieldOfView(target, duration).SetEase(curve);
            }
            else if(curveType is AnimCurveType.CurveID)
            {
                AnimationCurve curve = CurveManager.GetCurve(curveID);
                cam.DOFieldOfView(target, duration).SetEase(curve);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            active = false;

        }

    }
    #endregion

    #region OFFSET ANIM
    [System.Serializable]
    public class CamOffsetAnim
    {
        [Title("Main")]
        public bool active;
        public string id;

        [SerializeField] List<CamOffsetAnimInfo> anims;


        public async UniTaskVoid Play(CamInfo camInfo)
        {
            active = true;
            CamOffsetAnimInfo offsetAnim = null;
            foreach(CamOffsetAnimInfo anim in anims)
            {
                offsetAnim = anim;
                anim.Play(camInfo: camInfo).Forget();
                await UniTask.WaitUntil(() => anim.active == false);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(offsetAnim.duration));

            active = false;
        }

    }

    [System.Serializable]
    public class CamOffsetAnimInfo
    {
        #region Values
        [Title("Anim")]
        public bool active;
        public CamOffsetAnimType type;
        [ShowIf("@this.useTheDefaultOffset == false")] public OffsetType offsetType;
        public bool useTheDefaultOffset;
        [ShowIf("@this.useTheDefaultOffset == false")] public Vector3 targetOffset;
        public float duration;
        public float delay;
        public AnimCurveType curveType;
        [ShowIf("@this.curveType == AnimCurveType.Ease")] public Ease ease;
        [ShowIf("@this.curveType == AnimCurveType.Curve")] public AnimationCurve curve;
        [ShowIf("@this.curveType == AnimCurveType.CurveID")] public string curveID;

        [Space(6)]

        [Title("EVENT")]
        public bool useTheEvents;
        [ShowIf(nameof(useTheEvents))] public List<EventInfo> events;

        private Vector3 target;
        #endregion
        public async UniTaskVoid Play(CamInfo camInfo)
        {

            #region Start

            if (useTheEvents)
                events.ForEach(e => e.PlayEvent().Forget());

            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            target = targetOffset;
            if(offsetType is OffsetType.Add)
            {
                Vector3 defaultOffset = Vector3.zero;
                if(type is CamOffsetAnimType.Position)
                {
                    defaultOffset = camInfo.defaultposOffset;
                }
                else if(type is CamOffsetAnimType.Rotate)
                {
                    defaultOffset = camInfo.defaultrotOffset;
                }
                target += defaultOffset;
            }

            if(useTheDefaultOffset)
            {
                if(type is CamOffsetAnimType.Position)
                    target = camInfo.defaultposOffset;
                else
                    target = camInfo.defaultrotOffset;
            }
            #endregion

            #region Plays
            if (type is CamOffsetAnimType.Position)
            {
                CinemachineFramingTransposer transposer = camInfo.transposer;

                if(curveType is AnimCurveType.Ease)
                {
                    DOTween.To(() => camInfo.extraPosOffset, x => camInfo.extraPosOffset = x, target, duration).SetEase(ease);
                }
                else if(curveType is AnimCurveType.Curve)
                {
                    DOTween.To(() => camInfo.extraPosOffset, x => camInfo.extraPosOffset = x, target, duration).SetEase(curve);
                }
                else if(curveType is AnimCurveType.CurveID)
                {
                    AnimationCurve curve = CurveManager.GetCurve(curveID);
                    DOTween.To(() => camInfo.extraPosOffset, x => camInfo.extraPosOffset = x, target, duration).SetEase(curve);
                }


            }
            else if(type is CamOffsetAnimType.Rotate)
            {
                CinemachineComposer composer = camInfo.composer;

                if(curveType is AnimCurveType.Ease)
                {
                    DOTween.To(() => camInfo.extraPosOffset, x => camInfo.extraPosOffset = x, target, duration).SetEase(ease);
                }
                else if(curveType is AnimCurveType.Curve)
                {
                    DOTween.To(() => camInfo.extraPosOffset, x => camInfo.extraPosOffset = x, target, duration).SetEase(curve);
                }
                else if(curveType is AnimCurveType.CurveID)
                {
                    AnimationCurve curve = CurveManager.GetCurve(curveID);

                    DOTween.To(() => camInfo.extraPosOffset, x => camInfo.extraPosOffset = x, target, duration).SetEase(curve);
                }

            }
            #endregion

            #region End

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            active = false;


            #endregion
        }

    }
    #endregion


    [System.Serializable]
    public class ShakeAnim
    {
        public bool active;
        public string id;
        [SerializeField] List<ShakeAnimInfo> anims;

        public async UniTaskVoid Play()
        {
            foreach(ShakeAnimInfo info in anims)
            {
                active = true;
                info.Play().Forget();

                await UniTask.WaitUntil(() => info.active == false);
                active = false;
            }
        }

    }

    [System.Serializable]
    public class ShakeAnimInfo
    {
        public bool active;
        public float amplitudeGain;
        public float frequencyGain;
        public float duration;
        public float delay;
        public AnimCurveType curveType;
        [ShowIf("@this.curveType == AnimCurveType.Ease")] public Ease ease;
        [ShowIf("@this.curveType == AnimCurveType.Curve")] public AnimationCurve curve;
        [ShowIf("@this.curveType == AnimCurveType.CurveID")] public string curveID;

        public async UniTaskVoid Play()
        {
            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            CamInfo camInfo = CameraController.instance.CurrentCamera;
            CinemachineBasicMultiChannelPerlin perlin = camInfo.perlin;

            if(curveType is AnimCurveType.Ease)
            {
                DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, amplitudeGain, duration).SetEase(ease);
                DOTween.To(() => perlin.m_FrequencyGain, x => perlin.m_FrequencyGain = x, frequencyGain, duration).SetEase(ease);
            }
            else if(curveType is AnimCurveType.Curve)
            {
                DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, amplitudeGain, duration).SetEase(curve);
                DOTween.To(() => perlin.m_FrequencyGain, x => perlin.m_FrequencyGain = x, frequencyGain, duration).SetEase(curve);
            }
            else if(curveType is AnimCurveType.CurveID)
            {
                AnimationCurve curve = CurveManager.GetCurve(curveID);
                DOTween.To(() => perlin.m_AmplitudeGain, x => perlin.m_AmplitudeGain = x, amplitudeGain, duration).SetEase(curve);
                DOTween.To(() => perlin.m_FrequencyGain, x => perlin.m_FrequencyGain = x, frequencyGain, duration).SetEase(curve);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            active = false;
        }
        
    }
    
    #endregion
}
