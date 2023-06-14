using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(UIManager))]
public class UIStateInitializer : MonoBehaviour
{
    UIManager buf;
    private void Awake()
    {
        buf = GetComponent<UIManager>();
        buf.OnSetCanvas += OnSetCanvas;
    }

    void OnSetCanvas()
    {
        if (buf == null) return;
        buf.DefaultUIState = SaveDataManager.Instance.titleState;
        buf.OnSetCanvas -= OnSetCanvas;
    }
}
