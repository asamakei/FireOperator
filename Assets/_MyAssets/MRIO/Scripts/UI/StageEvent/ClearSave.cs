using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSave : MonoBehaviour
{
    ClearCanvasManager clearCanvasManager;
    private void Start()
    {
        clearCanvasManager = GetComponent<ClearCanvasManager>();
        if (clearCanvasManager != null)
        {
            clearCanvasManager.onClearOpen += OnClearOpen;
            clearCanvasManager.onClearClose += OnClearClose;
        }
    }
    public void OnClearOpen()
    { 
        SaveDataManager.Instance.Save(Values.SAVENUMBER);
    }

    public void OnClearClose()
    {
        if (clearCanvasManager != null)
        {
            clearCanvasManager.onClearOpen -= OnClearOpen;
            clearCanvasManager.onClearClose -= OnClearClose;
        }
    }
}
