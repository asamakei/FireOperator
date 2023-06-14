using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivater : MonoBehaviour
{
    private void Start()
    {
        ButtonUIManager.i.ChangeActivatingButton(MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs[Variables.currentStageIndex].stageVariableData.activatingStates);
    }
}
