using Cinemachine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{
    
    public class DefaultValuesHandler : MonoBehaviour
    {
        public bool ManuelInit { get {  return manuelInit; } }

        [SerializeField] bool showValues;
        [SerializeField] bool manuelInit;

        [ShowIf(nameof(showValues))]public Vector3 position;
        [ShowIf(nameof(showValues))] public Vector3 localPosition;

        [ShowIf(nameof(showValues))] public Quaternion rotation;
        [ShowIf(nameof(showValues))] public Quaternion localRotation;

        [ShowIf(nameof(showValues))] public Vector3 eulerAngles;
        [ShowIf(nameof(showValues))] public Vector3 localEulerAngles;

        [ShowIf(nameof(showValues))] public Vector3 localScale;
        [ShowIf(nameof(showValues))] public Vector3 lossyScale;

        [ShowIf(nameof(showValues))] public List<DefaultColorValue> colors = new List<DefaultColorValue>();
        [ShowIf(nameof(showValues))] public Color color;
        [ShowIf(nameof(showValues))] [ColorUsage(true,true)] public Color emissionColor;

        [ShowIf(nameof(showValues))] public float defaultFOV;
        [ShowIf(nameof(showValues))] public float defaultDistance;

        [ShowIf(nameof(showValues))] public RigidbodyConstraints constraints;


        private Renderer renderer;
        private Camera camera;
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineFramingTransposer transposer;
        private Rigidbody rigid;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            position = transform.position;
            localPosition = transform.localPosition;

            rotation = transform.rotation;
            localRotation = transform.localRotation;

            eulerAngles = transform.eulerAngles;
            localEulerAngles = transform.localEulerAngles;

            localScale = transform.localScale;
            lossyScale = transform.lossyScale;


            renderer = GetComponent<Renderer>();


            if (renderer)
            {
                if (renderer is not SpriteRenderer)
                {
                    color = renderer.material.color;
                    //emissionColor = renderer.material.GetColor("_EmissionColor");
                    int index = 0;
                    foreach (Material material in renderer.materials)
                    {
                        if (material.name.Substring(0, 7) == "Outline")
                            continue;

                        DefaultColorValue colorValue = new DefaultColorValue()
                        {
                            index = index,
                            color = material.color,
                            //emissionColor = material.GetColor("_EmissionColor")
                        };
                        colors.Add(colorValue);
                        index++;
                    }
                }
            }

            camera = GetComponent<Camera>();

            if (camera)
            {
                defaultFOV = camera.fieldOfView;
            }


            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera)
                transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            if (transposer)
                defaultDistance = transposer.m_CameraDistance;

            rigid = GetComponent<Rigidbody>();
            if (rigid)
                constraints = rigid.constraints;

                

        }

        public Color GetColor(int index)
        {
            Color color = colors.Find(c => c.index == index).color;

            return color;
        }

        public Color GetHDRColor(int index)
        {
            Color color = colors.Find(c => c.index == index).emissionColor;

            return color; ;
        }

        public void ResetPos()
        {
            transform.position = position;
            transform.localPosition = localPosition;
        }

        public void ResetRotation()
        {
            transform.rotation = rotation;
            transform.localRotation = localRotation;
        }

        public void ResetScale()
        {
            transform.localScale = localScale;
        }

        public void ResetColor()
        {
            if (renderer)
            {
                renderer.material.color = color;

                int index = 0;
                if(colors.Count > 0)
                {
                    foreach(Material mat in renderer.materials)
                    {
                        Color color = GetColor(index);
                        mat.color = color;
                        index++;
                    }
                }
            }
        }

        public void AllReset()
        {
            ResetPos();
            ResetRotation();
            ResetScale();
            ResetColor();
        }
    }


    [System.Serializable]
    public class DefaultColorValue
    {
        public int index;
        public Color color;
        [ColorUsage(true, true)] public Color emissionColor;
    }
}

