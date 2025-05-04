using UnityEngine;

public class ResolutionChanger : MonoBehaviour
{
    // �stedi�in ��z�n�rl�k de�erleri
    public int targetWidth = 1280;
    public int targetHeight = 720;
    public bool fullscreen = false; // Tam ekran m� olsun?

    void Start()
    {
        // Oyunu belirtilen ��z�n�rl�kte ba�lat
        Screen.SetResolution(targetWidth, targetHeight, fullscreen);
    }

    // �rnek: Kullan�c� "R" tu�una bas�nca ��z�n�rl��� de�i�tir
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("��z�n�rl�k de�i�tiriliyor!");

            // Farkl� bir ��z�n�rl��e ge�elim �rnek olsun diye
            Screen.SetResolution(1920, 1080, fullscreen);
        }
    }
}
