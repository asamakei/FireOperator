using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeStageUnlocker : KeycodeExecuterBase
{
    [SerializeField] StageVariableDataSO unlockStageVariableDataSO;
    [SerializeField][TextArea] string unlockMessage;
    public override void Execute()
    {
        StageLockManager.i.UnlockStage(unlockStageVariableDataSO.stageVariableData);
        EditableTextWindow.i.EditText(unlockStageVariableDataSO.stageVariableData.stageData.stageName + unlockMessage);
        EditableTextWindow.i.Activate();
        SaveDataManager.Instance.Save(Values.SAVENUMBER);
    }
}
