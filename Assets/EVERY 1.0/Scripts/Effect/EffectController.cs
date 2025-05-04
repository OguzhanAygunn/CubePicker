using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EVERY
{
    public class EffectController : MonoBehaviour
    {
        public ShaderEffectController Shader { get { return shader; } }
        public SizeEffectController Sizer { get { return sizer; } }

        [SerializeField] ShaderEffectController shader;
        [SerializeField] SizeEffectController sizer;

        [SerializeField] List<Every_EffectInfo> effects;


        public Every_EffectInfo GetEffect(string id)
        {
            return effects.Find(effect => effect.id == id);
        }

        [Button(size:ButtonSizes.Large)]
        public void Play(string id = "test")
        {

            Every_EffectInfo info = GetEffect(id: id);

            if (info == null)
                return;

            string shaderID = info.shaderEffectID;
            string sizeID = info.sizeEffectID;

            shader.Play(id: shaderID);
            sizer.PlayEffect(id: sizeID);

        }
    }


    [System.Serializable]
    public class Every_EffectInfo
    {
        public string id;
        public string shaderEffectID;
        public string sizeEffectID;
    }
}

