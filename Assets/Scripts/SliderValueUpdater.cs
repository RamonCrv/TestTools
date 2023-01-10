using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValueUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderValueText;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        UpdateSliderValueText();
    }

    public void UpdateSliderValueText()
    {
        sliderValueText.text = slider.value.ToString("F2");
    }
}
