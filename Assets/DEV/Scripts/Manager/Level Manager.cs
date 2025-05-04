using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }


    [SerializeField] List<GameObject> levelObjects;
    [SerializeField] int startLevel;


    private void Awake()
    {
        instance = (!instance) ? this : instance;
        startLevel--;
        startLevel = Mathf.Clamp(startLevel, 0, levelObjects.Count);
        ActiveLevel(startLevel);
    }

    public void ActiveLevel(int levelIndex)
    {
        GameObject levelParent = levelObjects[levelIndex];
        levelParent.SetActive(true);
    }

}
