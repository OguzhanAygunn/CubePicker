using EVERY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyBlocker))]
public class EnemyController : MonoBehaviour
{

    public EnemyMovement Movement { get { return movement; } }
    public EnemyCollisions Collisions { get { return collisions; } }
    public EnemyHealth Health { get { return health; } }
    public EnemyBlocker Blocker { get { return blocker; } }
    public EffectController Effect { get { return effect; } }

    [Title("Components")]
    [SerializeField] EnemyMovement movement;
    [SerializeField] EnemyCollisions collisions;
    [SerializeField] EnemyHealth health;
    [SerializeField] EnemyBlocker blocker;
    [SerializeField] EffectController effect;

    private void OnValidate()
    {
        if(!movement)
            movement = GetComponent<EnemyMovement>();

        if(!collisions)
            collisions = GetComponent<EnemyCollisions>();

        if(!health)
            health = GetComponent<EnemyHealth>();

        if(!blocker)
            blocker = GetComponent<EnemyBlocker>();
    }

    public void Init()
    {
        movement.Init();
    }

    public void Spawn()
    {
        //effect.Sizer.PlayEffect("Spawn Size");
    }

    public void Kill()
    {
        
        movement.Kill();
        EnemyManager.instance.RemoveEnemy(this);
        GameManager.instance.WinControl();
    }
}
