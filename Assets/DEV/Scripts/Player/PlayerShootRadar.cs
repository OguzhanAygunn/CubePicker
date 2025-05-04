using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EVERY;
using Sirenix.OdinInspector;
public class PlayerShootRadar : Radar 
{
    [Title("Shooter")]
    [SerializeField] bool shooterActive;
    [SerializeField] List<EnemyController> enemies;
    [SerializeField] float shootTimeRate = 0.2f;
    [SerializeField] private PlayerBlocker blocker;
    [SerializeField] private float counter;
    [SerializeField] private float timeRate;
    private void Start()
    {
        blocker = PlayerController.instance.Blocker;
        timeRate = shootTimeRate;
    }

    private void Update()
    {
        if (!shooterActive)
            return;

        //TimeRateUpdate();
        enemies.Clear();
        radarObjects.ForEach(obj => enemies.Add(obj.GetComponentInParent<EnemyController>()));
        CounterController();
    }

    private void TimeRateUpdate()
    {
        int count = enemies.Count;

        count = count == 0 ? 1 : count;

        timeRate = shootTimeRate / count;
    }

    private void CounterController()
    {
        counter = Mathf.MoveTowards(counter, timeRate, Time.deltaTime);

        if(counter == timeRate)
        {
            Shoot();
        }
    }

    private void CounterReset()
    {
        counter = 0;
    }

    public void SetActiveShooter(bool active)
    {
        shooterActive = active;
    }

    private EnemyController GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemies.Count);
        EnemyController enemy = enemies[randomIndex];
        return enemy;
    }

    private void Shoot()
    {
        CounterReset();

        if (enemies.Count == 0)
            return;

        foreach(EnemyController enemy in enemies)
        {
            Block block = PlayerController.instance.Blocker.GetNearestBlock(enemy.transform);
            block?.TryShoot(enemy);
        }
        
        //EnemyController enemy = GetRandomEnemy();
        
    }

}
