using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Title("Pool")]
    [SerializeField] List<EnemyPoolInfo> poolInfos;
    [SerializeField] List<EnemyInfo> enemyInfos;
    [SerializeField] List<EnemyController> enemies;

    [Space(7)]

    [Title("Spawner")]
    [SerializeField] Transform spawnerTrs;
    [SerializeField] Transform spawnerPoint;
    [SerializeField] float minSpawnRadius;
    [SerializeField] float maxSpawnRadius;
    private Transform enemiesParent;

    [Space(7)]

    [Title("Jump Anim")]
    public float jumpPower;
    public float jumpDuration;

    [Space(7)]

    [Title("Kill Anim")]
    public LayerMask waterLayer;
    public float killPower;


    private void Awake()
    {
        instance = (!instance) ? this : instance;
        enemiesParent = new GameObject("Enemies Parent").transform;

        Pool();
    }

    private void FixedUpdate()
    {
        enemies.ForEach(enemy => { enemy.Movement.Move(); });
    }


    private void Pool()
    {
        foreach(EnemyPoolInfo pool in poolInfos)
        {
            int index = 0;

            while (index < pool.count)
            {

                index++;

                GameObject enemyObj = Instantiate(pool.prefab, enemiesParent);
                enemyObj.gameObject.SetActive(false);

                EnemyInfo enemyInfo = new EnemyInfo()
                {
                    id  = pool.id,
                    enemy = enemyObj,
                };
                enemyInfos.Add(enemyInfo);
            
            }
        }
    }

    public GameObject GetEnemyObj(string id)
    {
        GameObject gameObj = enemyInfos.Find(info => info.id == id && !info.enemy.gameObject.activeInHierarchy)?.enemy;

        return gameObj;

    }

    public void UpdateSpawnerTrs()
    {
        float radius = UnityEngine.Random.Range(minSpawnRadius, maxSpawnRadius);

        Vector3 scale = spawnerTrs.localScale;
        scale.z = radius;
        spawnerTrs.localScale = scale;
        Vector3 randomRotate = spawnerTrs.localEulerAngles;
        randomRotate.y = UnityEngine.Random.Range(-180f, 180f);
        spawnerTrs.localEulerAngles = randomRotate;


        Vector3 spawnPoint = spawnerPoint.position;

    }

    public void AddEnemy(EnemyController ec)
    {
        if (enemies.Contains(ec))
            return;

        enemies.Add(ec);
    }

    public void RemoveEnemy(EnemyController ec)
    {
        if (!enemies.Contains(ec))
            return;

        enemies.Remove(ec);
    }

    public int GetEnemyCount()
    {
        return enemies.Count;
    }

    public void SpawnEnemy(string id)
    {
        GameObject enemyObj = GetEnemyObj(id);

        if (!enemyObj)
            return;

        EnemyController enemySc = enemyObj.GetComponent<EnemyController>();
        
        UpdateSpawnerTrs();



        enemyObj.SetActive(true);
        enemyObj.transform.position = spawnerPoint.position;
        enemySc.Init();
        enemySc.Spawn();
        enemies.Add(enemySc);
    }

    public async UniTaskVoid SpawnEnemies(string id, int count,float mainDelay = 0,float enemyDelay = 0)
    {

        if (mainDelay > 0)
            await UniTask.Delay(TimeSpan.FromSeconds(mainDelay));

        int index = 0;

        while(index < count)
        {
            index++;

            SpawnEnemy(id: id);

            if (enemyDelay > 0)
                await UniTask.Delay(TimeSpan.FromSeconds(enemyDelay));
        }
    }
}


[System.Serializable]
public class EnemyPoolInfo
{
    public string id;
    public GameObject prefab;
    public int count;
}

[System.Serializable]
public class EnemyInfo
{
    public string id;
    public GameObject enemy;
}

