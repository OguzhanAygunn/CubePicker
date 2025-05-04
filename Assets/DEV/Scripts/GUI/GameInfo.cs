using EVERY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    public GamePhase Type { get { return type; } }

    [Title("Main")]
    [SerializeField] GamePhase type;

    private void Awake()
    {
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }


}
