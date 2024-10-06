using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sliderText = null;
    [SerializeField] private Slider slider = null; // Reference to the Slider component
    [SerializeField] private float maxSliderAmount = 100.0f;

    private void Start()
    {
        // Ensure the initial value is set correctly
        SliderChange(slider.value);

        // Add listener for slider value changes
        slider.onValueChanged.AddListener(SliderChange);
    }

    public void SliderChange(float value)
    {
        float localValue = value * maxSliderAmount;
        sliderText.text = localValue.ToString("0") + "%";
    }
}