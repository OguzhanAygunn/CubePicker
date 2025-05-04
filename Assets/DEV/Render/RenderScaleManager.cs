using UnityEngine;

public class RenderScaleManager : MonoBehaviour
{
    public Camera mainCamera;   // Ana kamerayý buraya atayacaðýz
    [Range(0.1f, 1.0f)]
    public float renderScale = 0.7f; // %70 çözünürlükte render
    private RenderTexture lowResTexture;
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Ana kamerayý RenderTexture'a baðla
        CreateLowResTexture();

        // Display'e bir boþ kamera ekle
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
        // Önce var olan RenderTexture varsa temizle
        if (lowResTexture != null)
        {
            lowResTexture.Release();
            Destroy(lowResTexture);
        }

        // Ekran çözünürlüðünü al, renderScale uygula
        int width = Mathf.RoundToInt(Screen.width * renderScale);
        int height = Mathf.RoundToInt(Screen.height * renderScale);

        // Yeni RenderTexture oluþtur
        lowResTexture = new RenderTexture(width, height, 16);
        lowResTexture.filterMode = FilterMode.Bilinear; // Upscale için yumuþatma

        // Kameraya RenderTexture'u ata
        mainCamera.targetTexture = lowResTexture;
    }

    void OnGUI()
    {
        if (lowResTexture != null)
        {
            // RenderTexture'u tam ekran çiz
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), lowResTexture, ScaleMode.StretchToFill);
        }
    }

    // Ýstersen runtime'da render scale deðiþtirmek için örnek:
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
