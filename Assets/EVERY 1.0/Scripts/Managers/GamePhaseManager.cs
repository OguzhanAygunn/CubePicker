using Cysharp.Threading.Tasks;
using EVERY;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePhaseManager : MonoBehaviour
{
    public static GamePhaseManager instance;
    public GamePhase currentPhase;
    private void Awake()
    {
        instance = (!instance) ? this : instance;

        SetPhase(GamePhase.Collect, 2.2f).Forget();
    }



    public async UniTaskVoid SetPhase(GamePhase newPhase,float delay = 0)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(delay));

        currentPhase = newPhase;

        if (currentPhase is GamePhase.Collect)
            PlayCollectPhase();
        else if (currentPhase is GamePhase.Fight)
            PlayFightPhase();
    }

    public void PlayCollectPhase()
    {
        GameInfoPanel.instance.PlayGameInfo(type: currentPhase).Forget();
        TimePanel.instance.PlayTime(type: currentPhase, delay: 1f).Forget();
        PlayerController.instance.ShootRadar.SetActiveShooter(active: false);
    }

    public void PlayFightPhase()
    {
        GameInfoPanel.instance.PlayGameInfo(type: currentPhase).Forget();
        CameraController.instance.PlayDistanceAnim("Fight Size").Forget();
        PlayerController.instance.RadarV2.SetActiveMagnet(active: false);
        PlayerController.instance.RadarV1.SetActive(active: false);
        PlayerController.instance.BlockRadar.SetActive(active: false).Forget();
        PlayerController.instance.ShootRadar.SetActiveShooter(active: true);
        BlockManager.instance.CollectPhaseEnd();
        CollectibleManager.instance.CollectPhaseEnd();
        EnemyManager.instance.SpawnEnemies(id: "Enemy - A", count: 18, mainDelay: 0.5f, enemyDelay: 0.35f).Forget();
    }
}