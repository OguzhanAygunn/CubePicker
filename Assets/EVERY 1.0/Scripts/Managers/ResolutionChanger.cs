using UnityEngine;

public class ResolutionChanger : MonoBehaviour
{
    // Ýstediðin çözünürlük deðerleri
    public int targetWidth = 1280;
    public int targetHeight = 720;
    public bool fullscreen = false; // Tam ekran mý olsun?

    void Start()
    {
        // Oyunu belirtilen çözünürlükte baþlat
        Screen.SetResolution(targetWidth, targetHeight, fullscreen);
    }

    // Örnek: Kullanýcý "R" tuþuna basýnca çözünürlüðü deðiþtir
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Çözünürlük deðiþtiriliyor!");

            // Farklý bir çözünürlüðe geçelim örnek olsun diye
            Screen.SetResolution(1920, 1080, fullscreen);
        }
    }
}
