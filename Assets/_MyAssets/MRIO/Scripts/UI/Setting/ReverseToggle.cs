using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Toggle))]
public class ReverseToggle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    Toggle toggle;
    // Start is called before the first frame update
    void Start()
    {
        
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnValueChanged);
        toggle.isOn = SaveDataManager.Instance.saveData.optionData.shouldReverseUI;
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnValueChanged);
    }
    private void OnEnable()
    {
        if (toggle != null)
        {
            toggle.isOn = SaveDataManager.Instance.saveData.optionData.shouldReverseUI;
        }
    }
    public void OnValueChanged(bool isOn)
    {
        SaveDataManager.Instance.saveData.optionData.shouldReverseUI = isOn;
    }
}
