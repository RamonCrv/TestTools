using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Klak.TestTools;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private Image colorImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Slider thresholdSlider;
    [SerializeField] private Slider smoothingSlider;
    [SerializeField] private Material webcamMaterial;
    [SerializeField] private Material backgroundMaterial;
    [SerializeField] private Image backgroundImageReal;
    [SerializeField] private Texture webcamRenderTexture;


    private void Start()
    {
        //backgroundMaterial.SetTexture("_MainTex", GetBackgroundImage().texture);
        //backgroundImageReal.sprite = GetBackgroundImage();
        OnEnableConfigurationsMenu();
    }
    public Color GetPixelColorAtMousePosition(Camera camera)
    {
        // Capture the screen
        RenderTexture renderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Get the mouse position and convert to UV coordinate
        Vector2 mousePosition = Input.mousePosition;
        Vector2 uv = camera.ScreenToViewportPoint(mousePosition);
        //uv.y = 1 - uv.y; // Flip Y coordinate to match texture coordinates

        // Get the color of the pixel at the UV coordinate
        Color pixelColor = texture.GetPixelBilinear(uv.x, uv.y);

        // Clean up
        RenderTexture.active = null;
        camera.targetTexture = null;
        RenderTexture.ReleaseTemporary(renderTexture);
        Destroy(texture);

        return pixelColor;
    }

    public void TakeNewPhoto()
    {
        //ScreenShot.TakeScreenShot("BackgroundImages", "backgroundImage");
        
        StartCoroutine(GetNewPhotoCounter());
    }

    private IEnumerator GetNewPhotoCounter()
    {
        yield return new WaitForSeconds(1);
        //backgroundImage.sprite = GetBackgroundImage();
        SetImagesSprite();

    }

    private void SetImagesSprite()
    {
        Sprite newSprite = TextureToSpriteConverter(webcamRenderTexture);
        backgroundImage.sprite = newSprite;
        
    }

    private Sprite GetBackgroundImage()
    {
        List<Sprite> savedImages = new List<Sprite>();
        savedImages = ImageLoader.LoadImages("BackgroundImages");
        if (savedImages.Count == 0)
        {
            Debug.Log("No Image found In directory");
            return null;

        }
        else
        {
            return savedImages[0];
        }
    }

    public void SaveSettings()
    {
        SetChromaKeyValues();
    }

    private void SetChromaKeyValues()
    {
        webcamMaterial.SetColor("_MaskCol", colorImage.color);
        webcamMaterial.SetFloat("_Sensitivity", thresholdSlider.value);
        webcamMaterial.SetFloat("_Smooth", smoothingSlider.value);
        backgroundMaterial.SetTexture("_MainTex", GetBackgroundImage().texture);
        backgroundImageReal.sprite = backgroundImage.sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            if (panelCanvasGroup.alpha == 0)
            {
                OnEnableConfigurationsMenu();
            }
            else
            {
                panelCanvasGroup.alpha = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) == true)
        {
           colorImage.color = GetPixelColorAtMousePosition(Camera.main);
        }
    }

    private void OnEnableConfigurationsMenu()
    {
        panelCanvasGroup.alpha = 1;
        //backgroundImage.sprite = GetBackgroundImage();
        colorImage.color = webcamMaterial.GetColor("_MaskCol");
        thresholdSlider.value = webcamMaterial.GetFloat("_Sensitivity");
        smoothingSlider.value = webcamMaterial.GetFloat("_Smooth");

        
    }

    private Sprite TextureToSpriteConverter(Texture renderTexture)
    {
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = (RenderTexture)renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        // Create a new Sprite from the Texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        // Assign the new Sprite to a GameObject's SpriteRenderer
        return sprite;
    }


}
