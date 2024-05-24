using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [SerializeField] private Slider slider;


    private void Start()
    {
        //slider = GetComponent<Slider>();
    }


    public void InitializeBarStat(float actualValue, float maxValue)
    {
        SetMaxValue(maxValue);
        ChangeActualValue(actualValue);
    }

    public void SetMaxValue (float maxValue)
    {
        slider.maxValue = maxValue;
    }

    public void ChangeActualValue(float actualValue)
    {
        slider.value = actualValue;
    }
}