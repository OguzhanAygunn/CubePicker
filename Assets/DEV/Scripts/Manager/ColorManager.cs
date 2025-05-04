using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance { get { return instance; } }
    public static ColorManager instance;


    [SerializeField] List<ColorInfo> colors;

    private void Awake()
    {
        instance = (!instance) ? this : instance;
    }


    public static ColorInfo GetColorInfo(string id)
    {
        return instance.colors.Find(color => color.id == id);
    }

    public static ColorInfo GetColorInfo(int index)
    {
        return instance.colors.Find(color => color.index == index);
    }

    public static Color GetColor(string id)
    {
        ColorInfo info = GetColorInfo(id);

        if (info == null)
            return Color.black;

        return info.color;
    }

    public static Color GetColor(int index)
    {
        ColorInfo info = GetColorInfo(index);

        if (info == null)
            return Color.black;

        return info.color;
    }

    public static Color GetRandomColor()
    {
        Color color = Color.white;
        int index = Random.Range(0, instance.colors.Count);
        color = GetColor(index);


        return color;
    }


}

[System.Serializable]
public class ColorInfo
{
    public string id;
    public int index;
    public Color color;
}
