using UnityEngine;

public class RenderScaleManager : MonoBehaviour
{
    public Camera mainCamera;   // Ana kameray� buraya atayaca��z
    [Range(0.1f, 1.0f)]
    public float renderScale = 0.7f; // %70 ��z�n�rl�kte render
    private RenderTexture lowResTexture;
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Ana kameray� RenderTexture'a ba�la
        CreateLowResTexture();

        // Display'e bir bo� kamera ekle
        CreateDummyCamera();

        lowResTexture.hideFlags = HideFlags.DontSave;
    }

    void CreateDummyCamera()
    {
        GameObject dummyCamObj = new GameObject("DummyCamera");
        Camera dummyCam = dummyCamObj.AddComponent<Camera>();
        dummyCam.clearFlags = CameraClearFlags.Nothing;
        dummyCam.cullingMask = 0;
        dummyCam.depth = -100;
    }

    void CreateLowResTexture()
    {
        // �nce var olan RenderTexture varsa temizle
        if (lowResTexture != null)
        {
            lowResTexture.Release();
            Destroy(lowResTexture);
        }

        // Ekran ��z�n�rl���n� al, renderScale uygula
        int width = Mathf.RoundToInt(Screen.width * renderScale);
        int height = Mathf.RoundToInt(Screen.height * renderScale);

        // Yeni RenderTexture olu�tur
        lowResTexture = new RenderTexture(width, height, 16);
        lowResTexture.filterMode = FilterMode.Bilinear; // Upscale i�in yumu�atma

        // Kameraya RenderTexture'u ata
        mainCamera.targetTexture = lowResTexture;
    }

    void OnGUI()
    {
        if (lowResTexture != null)
        {
            // RenderTexture'u tam ekran �iz
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), lowResTexture, ScaleMode.StretchToFill);
        }
    }

    // �stersen runtime'da render scale de�i�tirmek i�in �rnek:
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            renderScale = Mathf.Clamp(renderScale + 0.1f, 0.1f, 1.0f);
            CreateLowResTexture();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            renderScale = Mathf.Clamp(renderScale - 0.1f, 0.1f, 1.0f);
            CreateLowResTexture();
        }
    }
}
