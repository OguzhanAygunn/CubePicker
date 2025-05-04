using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyCollisions : MonoBehaviour
{
    [SerializeField] EnemyController controller;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Tackle Trigger"))
        {
            controller.Movement.TryToGroundPhase();
        }
    }
}
