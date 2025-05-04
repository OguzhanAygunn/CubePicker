using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace EVERY
{
    [RequireComponent(typeof(EffectController))]
    public class ShaderEffectController : MonoBehaviour
    {
        [SerializeField] List<ShaderEffect> effects;
        private void Start()
        {
            effects.ForEach(effect => effect.Init());
            
            
        }


        [Button(size: ButtonSizes.Large)]
        public void Play(string id)
        {
            ShaderEffect effect = effects.Find(e => e.id == id);

            if (effect == null)
                return;

            effect.Play().Forget();
        }
    }


    [System.Serializable]
    public class ShaderEffect
    {
        [Title("Main")]
        public string id;
        public List<ShaderEffectInfo> effects;
        [Space(6)]

        [Title("Other")]
        public string easeID;
        public bool loop;
        [ShowIf(nameof(loop))] public float loopDelay;
        public void Init()
        {
            effects.ForEach(effect => effect.Init(this));
        }

        public async UniTaskVoid Play()
        {
            ShaderEffectInfo info = effects.Last();
            foreach (ShaderEffectInfo effect in effects) {

                effect.Play();
                await UniTask.WaitUntil(() => effect.active == false);                
            }

            if (loop)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(loopDelay));
                Play().Forget();
            }       
        }


        [Button(size: ButtonSizes.Large)]
        public void KillTweens()
        {
            
            loop = false;
            //DOTween.Complete();
            //DOTween.Kill(targetOrId: easeID, complete: true);
        }
    }


    [System.Serializable]
    public class ShaderEffectInfo
    {
        #region Main Values
        [Title("Main")]
        public bool active;
        public List<Renderer> renderers;
        [Space(6)]

        [Title("Types")]
        public ShaderEffectType effectType = ShaderEffectType.MaterialColor;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty")] public ShaderEffectValueType valueType = ShaderEffectValueType.Color;

        #endregion

        #region Material
        [Space(8)]

        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor")]
        [Title("Material")]
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor")][LabelText("Use Default Color")] public bool useTheDefaultMaterialColor;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor && this.useTheDefaultMaterialColor == false")][LabelText("Color")] public Color materialColor = Color.red;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor")][LabelText("Duration")] public float materialDuration = 1f;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor")][LabelText("Delay")] public float materialDelay = 1f;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor")][LabelText("Curve Type")] public AnimCurveType materialCurveType = AnimCurveType.Curve;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor && this.materialCurveType == AnimCurveType.Ease")][LabelText("Ease")] public Ease materialEase;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor && this.materialCurveType == AnimCurveType.Curve")][LabelText("Curve")] public AnimationCurve materialCurve;
        [ShowIf("@this.effectType == ShaderEffectType.MaterialColor && this.materialCurveType == AnimCurveType.CurveID")][LabelText("Curve ID")] public string materialCurveID;

        [Space(8)]
        #endregion

        #region Shader Color
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color")]
        [Title("Shader Color")]
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color")][LabelText("Prop ID")] public string shaderColorPropID = "_EmissionColor";
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color")][LabelText("Color")][ColorUsage(true,true)] 
        public Color shaderColor = Color.red;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color")][LabelText("Duration")] public float shaderColorDuration = 1f;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color")][LabelText("Delay")] public float shaderColorDelay = 1f;

        [Space(8)]

        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color")][LabelText("Curve Type")] 
        public AnimCurveType shaderColorCurveType = AnimCurveType.Curve;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color && this.shaderColorCurveType == AnimCurveType.Ease")][LabelText("Ease")] 
        public Ease shaderColorEase;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color && this.shaderColorCurveType == AnimCurveType.Curve")][LabelText("Curve")] 
        public AnimationCurve shaderColorCurve;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Color && this.shaderColorCurveType == AnimCurveType.CurveID")][LabelText("Curve ID")] 
        public string shaderColorCurveID;


        [Space(8)]
        #endregion

        #region Shader Float
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float")]
        [Title("Shader Color")]
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float")][LabelText("Prop ID")] public string shaderFloatPropID = "_Emission";
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float")][LabelText("Float Val")] public float shaderFloat = 1;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float")][LabelText("Duration")] public float shaderFloatDuration = 1f;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float")][LabelText("Delay")] public float shaderFloatDelay = 1f;

        [Space(8)]

        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float")]
        [LabelText("Curve Type")]
        public AnimCurveType shaderFloatCurveType = AnimCurveType.Curve;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float && this.shaderFloatCurveType == AnimCurveType.Ease")]
        [LabelText("Ease")]
        public Ease floatColorEase;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float && this.shaderFloatCurveType == AnimCurveType.Curve")]
        [LabelText("Curve")]
        public AnimationCurve floatColorCurve;
        [ShowIf("@this.effectType == ShaderEffectType.ShaderProperty && this.valueType == ShaderEffectValueType.Float && this.shaderFloatCurveType == AnimCurveType.CurveID")]
        [LabelText("Curve ID")]
        public string floatColorCurveID;


        [Space(15)]
        #endregion

        #region EVENTS
        [Title("Events")]
        public bool eventsPanelActive;
        [ShowIf(nameof(eventsPanelActive))] public List<EventInfo> events;
        #endregion

        #region Other Values
        private List<DefaultValuesHandler> defaults = new List<DefaultValuesHandler>();
        [SerializeField] private string easeID;
        ShaderEffect effect;
        #endregion

        #region Other Funcs
        public void Init(ShaderEffect sEffect)
        {

            effect = sEffect;
            easeID = effect.easeID;
            foreach (Renderer mr in renderers)
            {
                DefaultValuesHandler fvh = mr.GetComponent<DefaultValuesHandler>();
                defaults.Add(fvh);
            }
        }

        public DefaultValuesHandler GetDVH(int index)
        {
            return defaults[index];
        }

        #endregion

        #region Play
        public void Play()
        {
            if (active)
                return;
            easeID = effect.easeID;
            if (effectType is ShaderEffectType.MaterialColor)
            {
                PlayMaterial().Forget();
            }
            else if(effectType is ShaderEffectType.ShaderProperty)
            {
                if(valueType is ShaderEffectValueType.Color)
                {
                    PlayShaderColor().Forget();
                }
                else if(valueType is ShaderEffectValueType.Float)
                {
                    PlayerShaderFloat().Forget();
                }
            }
        }


        public async UniTaskVoid PlayMaterial()
        {
            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(materialDelay));


            Color targetColor = materialColor;
            DefaultValuesHandler dvh = null;
            int matIndex = 0;
            int mrIndex = 0;

            foreach (Renderer mr in renderers) {

                dvh = mr.GetComponent<DefaultValuesHandler>();

                foreach(Material material in mr.materials)
                {
                    targetColor = useTheDefaultMaterialColor ? dvh.color : targetColor;

                    if (materialCurveType is AnimCurveType.Ease)
                    {
                        material.DOColor(targetColor, materialDuration).SetEase(materialEase).SetId(easeID);
                    }
                    else if (materialCurveType is AnimCurveType.Curve)
                    {
                        material.DOColor(targetColor, materialDuration).SetEase(materialCurve).SetId(easeID);
                    }
                    else if (materialCurveType is AnimCurveType.CurveID)
                    {
                        AnimationCurve curve = CurveManager.GetCurve(materialCurveID);
                        material.DOColor(targetColor, materialDuration).SetEase(curve).SetId(easeID);
                    }
                    matIndex++;
                }
                mrIndex++;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(materialDuration));
            active = false;
        }

        public async UniTaskVoid PlayShaderColor()
        {
            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(shaderColorDelay));

            Color targetColor = materialColor;
            foreach (Renderer mr in renderers)
            {

                foreach (Material material in mr.materials)
                {
                    if (materialCurveType is AnimCurveType.Ease)
                    {
                        material.DOColor(shaderColor, shaderColorPropID, shaderColorDuration).SetEase(shaderColorEase).SetId(easeID);
                    }
                    else if (materialCurveType is AnimCurveType.Curve)
                    {
                        material.DOColor(shaderColor, shaderColorPropID, shaderColorDuration).SetEase(shaderColorCurve).SetId(easeID);
                    }
                    else if (materialCurveType is AnimCurveType.CurveID)
                    {
                        AnimationCurve curve = CurveManager.GetCurve(shaderColorCurveID);
                        material.DOColor(shaderColor, shaderColorPropID, shaderColorDuration).SetEase(curve).SetId(easeID);
                    }
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(shaderColorDuration));
            active = false;
        }

        public async UniTaskVoid PlayerShaderFloat()
        {
            active = true;
            await UniTask.Delay(TimeSpan.FromSeconds(shaderColorDelay));

            foreach (Renderer mr in renderers)
            {

                foreach (Material material in mr.materials)
                {
                    if (materialCurveType is AnimCurveType.Ease)
                    {
                        material.DOFloat(shaderFloat, shaderFloatPropID, shaderFloatDuration).SetEase(floatColorEase).SetId(easeID);
                    }
                    else if (materialCurveType is AnimCurveType.Curve)
                    {
                        material.DOFloat(shaderFloat, shaderFloatPropID, shaderFloatDuration).SetEase(floatColorCurve).SetId(easeID);
                    }
                    else if (materialCurveType is AnimCurveType.CurveID)
                    {
                        AnimationCurve curve = CurveManager.GetCurve(floatColorCurveID);
                        material.DOFloat(shaderFloat, shaderFloatPropID, shaderFloatDuration).SetEase(curve).SetId(easeID);
                    }
                }
            }

            await UniTask.Delay(TimeSpan.FromSeconds(shaderFloatDuration));
            active = false;
        }
        #endregion
    }

}
