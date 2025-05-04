using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace EVERY
{
    public class FXManager : MonoBehaviour
    {
        public static FXManager instance;


        [SerializeField] List<FXInfo> fxInfos;
        [SerializeField] List<FX> allFX;

        private Transform fxParent;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            instance = (!instance) ? this : instance;
            fxParent = new GameObject("FX Parent").transform;

            Pool();
        }

        private void Pool()
        {
            foreach (FXInfo info in fxInfos)
            {
                int index = 0;

                while (index < info.count)
                {
                    index++;

                    GameObject obj = Instantiate(info.prefab);
                    Tracker tracker = obj.AddComponent<Tracker>();
                    tracker.SetActiveAll(active: false, delay: 0).Forget();

                    obj.transform.SetParent(fxParent);
                    obj.SetActive(false);

                    FX fx = new FX()
                    {
                        id = info.id,
                        effect = obj,
                        particle = obj.GetComponent<ParticleSystem>()
                    };

                    fx.Init();
                    allFX.Add(fx);
                }

            }
        }

        public ParticleSystem GetFX(string id)
        {
            return allFX.Find(fx => fx.id == id && !fx.particle.gameObject.activeInHierarchy)?.particle;
        }


        public static async UniTaskVoid PlayFX(string id, Vector3 pos, float desTime)
        {
            ParticleSystem particle = instance.GetFX(id);
            if (!particle)
                return;


            particle.transform.position = pos;
            particle.gameObject.SetActive(true);
            particle.Play(withChildren: true);

            await UniTask.Delay(TimeSpan.FromSeconds(desTime));
            if (!particle)
                return;
            particle.gameObject.SetActive(false);
        }

        public static async UniTaskVoid PlayFX(string id, Transform parent, float desTime)
        {
            ParticleSystem particle = instance.GetFX(id);
            if (!particle)
                return;


            particle.transform.SetParent(parent);
            particle.transform.localPosition = Vector3.zero;
            particle.gameObject.SetActive(true);
            particle.Play(withChildren: true);

            await UniTask.Delay(TimeSpan.FromSeconds(desTime));
            particle.transform.SetParent(instance.fxParent);
            particle.gameObject.SetActive(false);
        }

        public static async UniTaskVoid PlayFX(string id, Transform parent, Vector3 pos, float desTime)
        {
            ParticleSystem particle = instance.GetFX(id);
            if (!particle)
                return;

            particle.transform.SetParent(parent);
            particle.transform.localPosition = pos;
            particle.gameObject.SetActive(true);
            particle.Play(withChildren: true);

            await UniTask.Delay(TimeSpan.FromSeconds(desTime));
            particle.transform.SetParent(instance.fxParent);
            particle.gameObject.SetActive(false);
        }

        public static async UniTaskVoid PlayFXWithTracker(string id, Transform target, float desTime, bool posTracker = true, bool rotTracker = false, bool scaleTracker = false)
        {
            ParticleSystem particle = instance.GetFX(id: id);
            if (!particle)
                return;


            particle.gameObject.SetActive(true);
            particle.Play(withChildren: true);

            Tracker tracker = particle.GetComponent<Tracker>();
            instance.TrackerUpdate(tracker: tracker,target:target, posTracker: posTracker, rotTracker: rotTracker, scaleTracker: scaleTracker);

            await UniTask.Delay(TimeSpan.FromSeconds(desTime));

            tracker.ResetTracker();
            particle.gameObject.SetActive(false);
            
        }


        public static async UniTaskVoid PlayFXWithTracker(string id, Transform target,Vector3 offset, float desTime, bool posTracker = true, bool rotTracker = false, bool scaleTracker = false)
        {

            ParticleSystem particle = instance.GetFX(id: id);
            if (!particle)
                return;

            particle.gameObject.SetActive(true);
            

            Tracker tracker = particle.GetComponent<Tracker>();
            instance.TrackerUpdate(tracker: tracker, target: target,offset:offset, posTracker: posTracker, rotTracker: rotTracker, scaleTracker: scaleTracker);
            particle.Clear(withChildren: true);
            particle.Play(withChildren: true);
            await UniTask.Delay(TimeSpan.FromSeconds(desTime));

            tracker.ResetTracker();
            particle.gameObject.SetActive(false);
        }


        public void TrackerUpdate(Tracker tracker,Transform target,bool posTracker,bool rotTracker, bool scaleTracker, Vector3 offset = default(Vector3))
        {
            offset = offset == default(Vector3) ? Vector3.zero : offset;

            tracker.SetActiveAll(active: false).Forget();

            if (posTracker)
            {
                tracker.SetActiveTracker(TrackerType.Pos, active: true).Forget();
                tracker.AssignTransform(TrackerType.Pos, target: target,offset:offset).Forget();
            }

            if (rotTracker)
            {
                tracker.SetActiveTracker(TrackerType.Rotate, active: true).Forget();
                tracker.AssignTransform(TrackerType.Pos, target: target).Forget();
            }
            
            if (scaleTracker)
            {
                tracker.SetActiveTracker(TrackerType.Scale, active: true).Forget();
                tracker.AssignTransform(TrackerType.Pos, target: target).Forget();
            }

            tracker.PosTracker();
        }
    }


    [System.Serializable]
    public class FXInfo
    {
        public string id;
        public GameObject prefab;
        public int count;
    }

    [System.Serializable]
    public class FX
    {
        public string id;
        public GameObject effect;
        public ParticleSystem particle;

        private Vector3 defaultScale;

        public void Init()
        {
            if (particle)
                defaultScale = particle.transform.localScale;
            else if (effect)
                defaultScale = effect.transform.localScale;
        }
    }


    //i will make this...
    public class FXSpawnValues
    {
        public string fxID;
        public string objectID;
        public GameObject obj;
        public Vector3 pos;
        public ParticleSystem particle;
        public float desTime;

        public void UpdateValues(string id)
        {

            id = id.Replace(" ", String.Empty);

            string[] fxIDs = id.Split(':');

            fxID = fxIDs[0];
            particle = FXManager.instance.GetFX(fxID);
            obj = ObjectInfoHandlerManager.GetObject(objectID);

            float x = (float)Convert.ToDouble(fxIDs[1]) / 100;
            float y = (float)Convert.ToDouble(fxIDs[2]) / 100;
            float z = (float)Convert.ToDouble(fxIDs[3]) / 100;


            pos = obj.transform.position;
            pos += new Vector3(x, y, z);




        }

    }

}