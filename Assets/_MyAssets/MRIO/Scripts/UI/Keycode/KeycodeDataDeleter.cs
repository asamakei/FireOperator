using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeDataDeleter : KeycodeExecuterBase
{
    [SerializeField][TextArea] string deleteMessage;
    [SerializeField] string deleteFailedMessage;
    public override void Execute()
    {
        if (SaveDataManager.Instance.Delete(Values.SAVENUMBER))
        {
            EditableTextWindow.i.EditText(deleteMessage);
            EditableTextWindow.i.Activate();
            SaveDataManager.Instance.DeleteTempSaveData();
        }
        else
        {
            EditableTextWindow.i.EditText(deleteFailedMessage);
            EditableTextWindow.i.Activate();
        }
    }
}
