using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraColorSetter : MonoBehaviour
{
    private void OnEnable()
    {
        Camera mainCamera = Camera.main;
        int index = Mathf.Min(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1, Variables.currentStageIndex);
        mainCamera.backgroundColor = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData.stageData.backColor;
    }
}
