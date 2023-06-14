using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(StageLoadButton))]

public class TitleStageDataSetter : MonoBehaviour
{
    [SerializeField] StageVariableDataSO stageVariableDataSO;
    [SerializeField] UIState uiState = UIState.Select;
    StageLoadButton buf;
    private void Awake()
    {
        buf = GetComponent<StageLoadButton>();
        buf.loadingStageVariableData = stageVariableDataSO.stageVariableData;
        buf.onPushed += OnPushed;

    }
    void OnPushed()
    {
        SaveDataManager.Instance.titleState = uiState;
        if (buf != null) buf.onPushed -= OnPushed;
    }
}
