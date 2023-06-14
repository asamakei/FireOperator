using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(UniversalAdditionalCameraData))]
public class CameraRendererSetter : MonoBehaviour
{
    UniversalAdditionalCameraData universalAdditionalCameraData;
    // Start is called before the first frame update
    void Start()
    {
        universalAdditionalCameraData = GetComponent<UniversalAdditionalCameraData>();
        int index = Mathf.Min(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs.Length - 1, Variables.currentStageIndex);
        StageData stageData = MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[index].stageVariableData.stageData;
        if (stageData == null) return;
        try
        {
            universalAdditionalCameraData.SetRenderer(stageData.rendererNumber);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Renderer Exception");
        }
    }

}
