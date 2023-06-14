using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeDebugUnlock : KeycodeExecuterBase
{
    public override void Execute()
    {
        foreach (StageVariableDataSO stageVariableDataSO in MasterDataManager.Instance.stageVariableDataDBSO.stageVariableDataSOs)
        {
            StageLockManager.i.ForceUnlockStage(stageVariableDataSO.stageVariableData);
            SaveDataManager.Instance.saveData.wasTutorialPlayed = true;
        }

    }
}
