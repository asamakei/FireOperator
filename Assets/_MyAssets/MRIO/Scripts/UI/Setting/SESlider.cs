using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Slider))]
public class SESlider : MonoBehaviour, ISliderModifier
{
    [SerializeField] TextMeshProUGUI percentText;
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnValueChanged);
    }
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChanged);
    }
    private void OnEnable()
    {
        if (slider != null && SaveDataManager.Instance != null) slider.value = SaveDataManager.Instance.saveData.optionData.seVolumeRate;
    }
    public void OnValueChanged(float rate)
    {
        if (SaveDataManager.Instance != null) SaveDataManager.Instance.saveData.optionData.seVolumeRate = rate;
        percentText.SetText(((1 - rate) * 100).ToString("F0") + "%");
    }
}
