using Cysharp.Threading.Tasks;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace EVERY
{

    public class FXSpawner : MonoBehaviour
    {
        public List<FXAnim> anims;


        [Button(size: ButtonSizes.Large)]
        public async void PlayAnim(string id, float delay = 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            FXAnim anim = GetAnim(id);

            if (anim == null)
                return;

            anim.Play().Forget();
        }


        public FXAnim GetAnim(string id)
        {
            return anims.Find(x => x.id == id);
        }

    }


    [System.Serializable]
    public class FXAnim
    {
        public bool active;
        public string id;

        public List<FXAnimInfo> anims;

        public async UniTaskVoid Play()
        {
            active = true;
            FXAnimInfo lastAnimInfo = anims.Last();
            foreach (FXAnimInfo anim in anims)
            {

                anim.PlayFX().Forget();
                await UniTask.WaitUntil(() => anim.active == false);
            }

            await UniTask.Delay(TimeSpan.FromSeconds(lastAnimInfo.duration));
            active = false;
        }

    }

    [System.Serializable]
    public class FXAnimInfo
    {
        public bool active;
        public FXSpawnType spawnType;

        [Title("FX Spawn 1")]
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_1")][LabelText("Id")] public string fx1_fxID;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_1")][LabelText("Pos")] public Vector3 fx1_pos;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_1")][LabelText("Des Time")] public float fx1_desTime;

        [Title("FX Spawn 2")]
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_2")][LabelText("Id")] public string fx2_fxID;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_2")][LabelText("Parent")] public Transform fx2_parent;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_2")][LabelText("Des Time")] public float fx2_desTime;

        [Title("FX Spawn 3")]
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_3")][LabelText("Id")] public string fx3_fxID;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_3")][LabelText("Parent")] public Transform fx3_parent;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_3")][LabelText("Pos")] public Vector3 fx3_pos;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFX_3")][LabelText("Des Time")] public float fx3_desTime;

        [Title("FX Spawn Tracker 1")]
        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Id")] public string fxT1_fxID;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Parent")] public Transform fxT1_parent;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Pos")] public Vector3 fxT1_offset;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Des Time")] public float fxT1_desTime;

        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Pos Tracker")] public bool fxT1_posTracker;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Rot Tracker")] public bool fxT1_rotTracker;
        [ShowIf("@this.spawnType == FXSpawnType.PlayFXWithTracker_1")][LabelText("Scale Tracker")] public bool fxT1_scaleTracker;

        [Space(5)]

        [Title("Time Values")]
        public float delay;
        public float duration;

        public async UniTaskVoid PlayFX()
        {
            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (spawnType is FXSpawnType.PlayFX_1)
            {
                FXManager.PlayFX(id: fx1_fxID, pos: fx1_pos, desTime: fx1_desTime).Forget();
            }
            else if (spawnType is FXSpawnType.PlayFX_2)
            {
                FXManager.PlayFX(id: fx2_fxID, parent: fx2_parent, desTime: fx2_desTime).Forget();
            }
            else if (spawnType is FXSpawnType.PlayFX_3)
            {
                FXManager.PlayFX(id: fx3_fxID, parent: fx3_parent, pos: fx3_pos, desTime: fx3_desTime).Forget();
            }
            else if (spawnType is FXSpawnType.PlayFXWithTracker_1)
            {
                FXManager.PlayFXWithTracker(id: fxT1_fxID, target: fxT1_parent, offset: fxT1_offset, posTracker: fxT1_posTracker, rotTracker: fxT1_rotTracker, scaleTracker: fxT1_scaleTracker, desTime: fxT1_desTime).Forget();
            }
            else if (spawnType is FXSpawnType.PlayFXWithTracker_2)
            {
                //nothing -_-
            }

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            active = false;

        }
    }
}