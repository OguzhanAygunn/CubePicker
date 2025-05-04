using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

//this version 1.0
namespace EVERY
{
    [RequireComponent(typeof(TrackerExtraOffsetController))]
    //MANUAL UPDATE COMÝNG SOOONNNNN..... (   maybe V2.0  (O_O)   )
    public class Tracker : MonoBehaviour
    {
        #region Values
        [Title("Main")]
        [SerializeField] TrackerExtraOffsetController extraOffsetController;
        [SerializeField] bool initOffsets;

        [Space(6)]

        [Title("Position Tracker")]
        [SerializeField] bool posTrackerActive;
        [ShowIf("@this.posTrackerActive == true")][SerializeField] Transform posObj;
        [ShowIf("@this.posTrackerActive == true")][SerializeField] Transform posTarget;
        [ShowIf("@this.posTrackerActive == true")][SerializeField] Vector3 posOffset;
        [ShowIf("@this.posTrackerActive == true")][SerializeField] MoveType posMoveType = MoveType.Fix;
        [ShowIf("@this.posTrackerActive == true")][ShowIf("@this.posMoveType != MoveType.Fix")][SerializeField] float posMoveSpeed = 5;
        [ShowIf("@this.posTrackerActive == true")][SerializeField] UpdateType posUpdateType = UpdateType.Late;
        [ShowIf("@this.posTrackerActive == true")][ShowIf("@this.posUpdateType == UpdateType.Manuel")][SerializeField] float posUpdateRate = 0.02f;
        [ShowIf("@this.posTrackerActive == true")][SerializeField] Vector3 posExtraOffset;

        [Space(6)]

        [Title("Rotation Tracker")]
        [SerializeField] bool rotateTrackerActive;
        [ShowIf("@this.rotateTrackerActive")][SerializeField] Transform rotObj;
        [ShowIf("@this.rotateTrackerActive")][SerializeField] Transform rotTarget;
        [ShowIf("@this.rotateTrackerActive")][SerializeField] Vector3 rotateOffset;
        [ShowIf("@this.rotateTrackerActive")][SerializeField] MoveType rotateType = MoveType.Fix;
        [ShowIf("@this.rotateTrackerActive")][SerializeField] UpdateType rotateUpdateType = UpdateType.Late;
        [ShowIf("@this.rotateTrackerActive")][ShowIf("@this.rotateType != MoveType.Fix")][SerializeField] float rotateSpeed = 5;
        [ShowIf("@this.rotateTrackerActive == true")][ShowIf("@this.rotateUpdateType == UpdateType.Manuel")][SerializeField] float rotateUpdateRate = 0.02f;
        [ShowIf("@this.rotateTrackerActive == true")][SerializeField] Vector3 rotExtraOffset;
        [Space(6)]

        [Title("Scale Tracker")]
        [SerializeField] bool scaleTrackerActive;
        [ShowIf("@this.scaleTrackerActive")][SerializeField] Transform scaleObj;
        [ShowIf("@this.scaleTrackerActive")][SerializeField] Transform scaleTarget;
        [ShowIf("@this.scaleTrackerActive")][SerializeField] Vector3 scaleOffset;
        [ShowIf("@this.scaleTrackerActive")][SerializeField] MoveType scaleType = MoveType.Fix;
        [ShowIf("@this.scaleTrackerActive")][ShowIf("@this.scaleType != MoveType.Fix")][SerializeField] float scaleSpeed = 5;
        [ShowIf("@this.scaleTrackerActive")][SerializeField] UpdateType scaleUpdateType = UpdateType.Late;
        [ShowIf("@this.scaleTrackerActive == true")][ShowIf("@this.scaleUpdateType == UpdateType.Manuel")][SerializeField] float scaleUpdateRate = 0.02f;
        [ShowIf("@this.scaleTrackerActive == true")][SerializeField] Vector3 scaleExtraOffset;

        #endregion

        #region Awake & Start & Init
        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            //Nothing
        }

        private void Init()
        {
            if (initOffsets)
                posOffset = posObj.position - posTarget.position;

            extraOffsetController = GetComponent<TrackerExtraOffsetController>();
            UpdateCurrentTransforms(beforeCheck: false, delay: 0).Forget();
        }
        #endregion

        #region Updates
        private void LateUpdate()
        {
            if (posTrackerActive && posUpdateType == UpdateType.Late)
                PosTracker();

            if (rotateTrackerActive && rotateUpdateType == UpdateType.Late)
                RotationTracker();

            if (scaleTrackerActive && scaleUpdateType == UpdateType.Late)
                ScaleTracker();
        }
        private void FixedUpdate()
        {
            if (posTrackerActive && posUpdateType == UpdateType.Fixed)
                PosTracker();

            if (rotateTrackerActive && rotateUpdateType == UpdateType.Fixed)
                RotationTracker();

            if (scaleTrackerActive && scaleUpdateType == UpdateType.Fixed)
                ScaleTracker();
        }
        private void Update()
        {
            if (posTrackerActive && posUpdateType == UpdateType.Update)
                PosTracker();

            if (rotateTrackerActive && rotateUpdateType == UpdateType.Update)
                RotationTracker();

            if (scaleTrackerActive && scaleUpdateType == UpdateType.Update)
                ScaleTracker();
        }

        #endregion

        #region Position Tracker
        public void PosTracker()
        {
            if (!posObj)
                return;

            Vector3 pos = posObj.position;
            posExtraOffset = extraOffsetController.posOffset;
            Vector3 targetPos = posTarget.position + posOffset + posExtraOffset;


            pos = PosUpdate(startPos: pos, endPos: targetPos);
            posObj.position = pos;

        }


        public Vector3 PosUpdate(Vector3 startPos, Vector3 endPos)
        {
            Vector3 pos = startPos;
            float deltaTime = GetDeltaTime(posUpdateType);
            switch (posMoveType)
            {
                case MoveType.Fix:
                    pos = endPos;
                    break;
                case MoveType.MoveToWards:
                    pos = Vector3.MoveTowards(startPos, endPos, posMoveSpeed * deltaTime);
                    break;
                case MoveType.Lerp:
                    pos = Vector3.Lerp(startPos, endPos, posMoveSpeed * deltaTime);
                    break;
                default:
                    pos = endPos;
                    break;
            }

            return pos;
        }

        #endregion

        #region Rotate Tracker
        private void RotationTracker()
        {
            if (!rotObj)
                return;

            Quaternion rot = rotObj.rotation;
            Quaternion endRot = rotTarget.rotation;

            rot = UpdateRotate(rot, endRot);
            rotObj.rotation = rot;
        }

        private Quaternion UpdateRotate(Quaternion startRot, Quaternion endRotate)
        {
            Quaternion q = startRot;

            float deltaTime = GetDeltaTime(rotateUpdateType);
            switch (rotateType)
            {
                case MoveType.Fix:
                    q = rotTarget.rotation;
                    break;
                case MoveType.MoveToWards:
                    q = Quaternion.RotateTowards(q, endRotate, rotateSpeed * deltaTime);
                    break;
                case MoveType.Lerp:
                    q = Quaternion.Lerp(q, endRotate, rotateSpeed * deltaTime);
                    break;
                default:
                    q = rotTarget.rotation;
                    break;
            }

            return q;
        }


        #endregion

        #region Scale Tracker
        private void ScaleTracker()
        {
            if (!scaleObj)
                return;

            Vector3 startScale = scaleObj.localScale;
            Vector3 endScale = scaleTarget.localScale;

            Vector3 scale = ScaleUpdate(startScale: startScale, endScale: endScale);
            scaleObj.localScale = scale;
        }

        private Vector3 ScaleUpdate(Vector3 startScale,Vector3 endScale)
        {
            Vector3 scale = startScale;
            endScale += scaleOffset;
            float deltaTime = GetDeltaTime(scaleUpdateType);

            switch (scaleType) {

                case MoveType.Lerp:
                    scale = Vector3.Lerp(scale, endScale, scaleSpeed * deltaTime);
                    break;
                case MoveType.MoveToWards:
                    scale = Vector3.MoveTowards(scale, endScale, scaleSpeed * deltaTime);
                    break;
                case MoveType.Fix:
                    scale = endScale;
                    break;
                default: 
                    scale = endScale;
                    break;
            }

            return scale;
        }

        #endregion

        #region EVERY :O
        private float GetDeltaTime(UpdateType type)
        {
            float deltaTime = 0;

            switch (type)
            {
                case UpdateType.Late:
                    deltaTime = Time.deltaTime;
                    break;
                case UpdateType.Update:
                    deltaTime = Time.deltaTime;
                    break;
                case UpdateType.Fixed:
                    deltaTime = Time.fixedDeltaTime;
                    break;
                default:
                    break;
            }
            return deltaTime;
        }
        public async UniTaskVoid SetActiveTracker(TrackerType type, bool active, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            switch (type) {
                case TrackerType.Pos:
                    posTrackerActive = active;
                    break;
                case TrackerType.Rotate:
                    rotateTrackerActive = active;
                    break;
                case TrackerType.Scale:
                    scaleTrackerActive = active;
                    break;
            }
        }

        public async UniTaskVoid SetActiveOnlyOne(TrackerType type, bool active, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            rotateTrackerActive = false;
            posTrackerActive = false;
            scaleTrackerActive = false;

            switch (type)
            {
                case TrackerType.Pos:
                    posTrackerActive = active;
                    break;
                case TrackerType.Rotate:
                    rotateTrackerActive = active;
                    break;
                case TrackerType.Scale:
                    scaleTrackerActive = active;
                    break;
            }
        }

        public async UniTaskVoid SetActiveAll(bool active, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            posTrackerActive = false;
            rotateTrackerActive = false;
            scaleTrackerActive = false;
        }

        public async UniTaskVoid SetMoveType(TrackerType trackerType, MoveType moveType, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            switch (trackerType)
            {
                case TrackerType.Pos:
                    posMoveType = moveType;
                    break;
                case TrackerType.Rotate:
                    rotateType = moveType;
                    break;
                case TrackerType.Scale:
                    scaleType = moveType;
                    break;
            }

        }

        public async UniTaskVoid SetAllMoveType(MoveType moveType,float delay = 0) {

            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            posMoveType = moveType;
            rotateType = moveType;
            scaleType = moveType;
        }

        public async UniTaskVoid UpdateCurrentTransforms(bool beforeCheck = true,float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (beforeCheck)
            {
                if (!posObj)
                    posObj = transform;

                if(!rotObj)
                    rotObj = transform;

                if(!scaleObj)
                    scaleObj = transform;
            }
            else
            {
                posObj = transform;
                rotObj = transform;
                scaleObj = transform;
            }
        }

        #endregion

        #region Assign & Clear & Offset

        public async UniTaskVoid AssignTransform(TrackerType type, Transform target,float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            switch (type)
            {
                case TrackerType.Pos:
                    posTarget = target;
                    break;
                case TrackerType.Rotate:
                    rotTarget = target;
                    break;
                case TrackerType.Scale:
                    scaleTarget = target;
                    break;
            }

        }

        public async UniTaskVoid AssignTransform(TrackerType type, Transform target,Vector3 offset,float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            switch (type)
            {
                case TrackerType.Pos:
                    posTarget = target;
                    break;
                case TrackerType.Rotate:
                    rotTarget = target;
                    break;
                case TrackerType.Scale:
                    scaleTarget = target;
                    break;
            }

            SetOffset(type: type, offset: offset, delay: 0).Forget();

        }

        public async UniTaskVoid ClearAllTransform(float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            posTarget= null;
            rotTarget= null;
            scaleTarget= null;
        }

        public async UniTaskVoid SetOffset(TrackerType type,Vector3 offset,float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            switch (type)
            {
                case TrackerType.Pos:
                    posOffset = offset;
                    break;
                case TrackerType.Rotate:
                    rotateOffset = offset;
                    break;
                case TrackerType.Scale:
                    scaleOffset = offset;
                    break;
            }
        }

        public async UniTaskVoid ResetOffsets(float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            posOffset = Vector3.zero;
            rotateOffset = Vector3.zero;
            scaleOffset = Vector3.zero;
        }

        public void ResetTracker()
        {
            SetActiveAll(active: false).Forget();
            ResetOffsets().Forget();
            ClearAllTransform().Forget();
        }

        #endregion
    }
}