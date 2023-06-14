using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    public static CoinView Instance;
    private void Awake()
    {
        Instance = this;
        UpdateView();

    }
    private void OnEnable()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        textMeshProUGUI.SetText(SaveDataManager.Instance.saveData.shopData.coinNum.ToString());
    }
}
