using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxSatisfaction(int maxSatisfaction)
    {
        slider.maxValue = maxSatisfaction;
        slider.value = maxSatisfaction;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetSatisfaction(int satisfaction)
    {
        slider.value = satisfaction;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
