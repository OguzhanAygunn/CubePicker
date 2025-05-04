using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPanel : MonoBehaviour
{
    public static WinPanel instance;

    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }
}
