using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour
{
    public static FailPanel instance;

    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
    }

}
