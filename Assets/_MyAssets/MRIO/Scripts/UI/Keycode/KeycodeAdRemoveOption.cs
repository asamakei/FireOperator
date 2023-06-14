using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeAdRemoveOption : KeycodeExecuterBase
{
    [SerializeField] string unlockMessage;
    public override void Execute()
    {
        SaveDataManager.Instance.saveData.isNoAdSkipMode = true;
        SaveDataManager.Instance.Save(Values.SAVENUMBER);
        EditableTextWindow.i.EditText( unlockMessage);
        EditableTextWindow.i.Activate();
    }
}
