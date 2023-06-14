using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(ClearCanvasManager))]
public class StageEventPresenter : MonoBehaviour
{
    StageClearEventBase[] stageClearEventBases;
    ClearCanvasManager clearCanvasManager;
    StageClearEventBase buf;
    private void Awake()
    { 
        clearCanvasManager = GetComponent<ClearCanvasManager>();
        stageClearEventBases = GetComponents<StageClearEventBase>();
        StageVariableDataSO stageVariableDataSO = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex];
        buf = Array.Find(stageClearEventBases, stageClearEventBase => stageClearEventBase.StageClearEventType == stageVariableDataSO.stageVariableData.stageData.stageClearEventType);

        if (buf != null && clearCanvasManager != null)
        {
            clearCanvasManager.onClearOpen += buf.OnClearOpen;
            clearCanvasManager.onClearClose += buf.OnClearClose;
        }
        gameObject.SetActive(false);

    }
   

    private void OnDestroy()
    {
        if (buf != null && clearCanvasManager != null)
        {
            clearCanvasManager.onClearOpen -= buf.OnClearOpen;
            clearCanvasManager.onClearClose -= buf.OnClearClose;
        }
        buf = null;
    }
}
