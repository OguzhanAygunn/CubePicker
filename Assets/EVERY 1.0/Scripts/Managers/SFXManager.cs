using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    public static SFXManager Instance { get { return instance; } }
    [SerializeField] AudioSource source;
    [SerializeField] List<SFXInfo> clips;



    public static SFXInfo GetSFXInfo(string id)
    {
        return instance.clips.Find(x => x.id == id);
    }




}


[System.Serializable]
public class SFXInfo
{
    [Title("Main")]
    public bool showMains;
    [ShowIf(nameof(showMains))]public string id;
    [ShowIf(nameof(showMains))] public AudioClip clip;
    [ShowIf(nameof(showMains))][Range(0f, 1f)] public float volume;

    [Space(6)]

    [Title("Detail")]
    public bool showDetail;
    [ShowIf(nameof(showDetail))] public bool isPlayable;
    [ShowIf(nameof(showDetail))] public int count;
    [ShowIf(nameof(showDetail))] public int maxCount;
}
