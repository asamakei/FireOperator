using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Slider))]
public class BGMSlider : MonoBehaviour, ISliderModifier
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
        if (slider != null) slider.value = SaveDataManager.Instance.saveData.optionData.bgmVolumeRate;
    }
    public void OnValueChanged(float rate)
    {
        SaveDataManager.Instance.saveData.optionData.bgmVolumeRate = rate;
        percentText.SetText(((1 - rate) * 100).ToString("F0") + "%");
    }
}
