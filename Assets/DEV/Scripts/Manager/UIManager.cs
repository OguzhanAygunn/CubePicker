using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] GameObject failPanelObj;
    [SerializeField] GameObject winPanelObj;
    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }


    

    public void Fail()
    {
        failPanelObj.SetActive(true);
    }

    public void Win()
    {
        winPanelObj.SetActive(true);
    }

    

}
