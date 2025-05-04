using Cysharp.Threading.Tasks;
using EVERY;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoPanel : MonoBehaviour
{
    public static GameInfoPanel instance;

    [Title("Main")]
    [SerializeField] List<GameInfo> infos;


    private void Awake()
    {
        instance = (!instance) ? this : instance;
        AllDeActiveInfos();
    }

    private void AllDeActiveInfos()
    {
        infos.ForEach(info => info.gameObject.SetActive(value: false));
    }

    public GameInfo GetGameInfo(GamePhase type)
    {
        return infos.Find(info => info.Type == type);
    }

    public async UniTaskVoid PlayGameInfo(GamePhase type,float delay=0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        GameInfo info = GetGameInfo(type);

        if (info == null)
            return;
        info.gameObject.SetActive(value: true);
    }
}
