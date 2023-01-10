using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfigManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup panelCanvasGroup;

    [SerializeField] private TMP_InputField colorInputField;
    [SerializeField] private Image colorImage;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Slider thresholdSlider;
    [SerializeField] private Slider smoothingSlider;

    [SerializeField] private Material webcamMaterial;
    [SerializeField] private Material backgroundMaterial;

    public void GenerateColor()
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(colorInputField.text, out newColor))
        {
            colorImage.color = newColor;
        }
      
    }

    public void TakeNewPhoto()
    {
        ScreenShot.TakeScreenShot("BackgroundImages", "backgroundImage");
        StartCoroutine(GetNewPhotoCounter());
    }

    private IEnumerator GetNewPhotoCounter()
    {
        yield return new WaitForSeconds(1);
        backgroundImage.sprite = GetBackgroundImage();

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            if (panelCanvasGroup.alpha == 0)
            {
                panelCanvasGroup.alpha = 1;
            }
            else
            {
                panelCanvasGroup.alpha = 0;
            }
        }
    }


}
