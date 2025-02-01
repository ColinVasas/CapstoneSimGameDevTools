using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeightSliderValue : MonoBehaviour
{
    public Slider SliderComponent;
    public TMP_Text TextComponent;
    
    public void UpdateText()
    {
        TextComponent.text = SliderComponent.value.ToString();
    }
}
