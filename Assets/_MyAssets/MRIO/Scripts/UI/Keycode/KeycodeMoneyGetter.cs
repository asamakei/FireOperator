using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycodeMoneyGetter : KeycodeExecuterBase
{
    [SerializeField] int moneyAmount;
    [SerializeField][TextArea] string message;
    [SerializeField][TextArea] string failedMessage;
    [SerializeField] int saveId;
    [SerializeField] bool canMultipleGet;

    public override void Execute()
    {
        if (SaveDataManager.Instance.saveData.shopData.getterids == null) SaveDataManager.Instance.saveData.shopData.getterids = new List<int>();
        if (SaveDataManager.Instance.saveData.shopData.getterids.Contains(saveId) && !canMultipleGet)
        {
            EditableTextWindow.i.EditText(failedMessage);
            EditableTextWindow.i.Activate();
            return;
        }
        else
        {
            SaveDataManager.Instance.saveData.shopData.getterids.Add(saveId);
            SaveDataManager.Instance.saveData.shopData.coinNum += moneyAmount;
            EditableTextWindow.i.EditText(message);
            EditableTextWindow.i.Activate();
            SaveDataManager.Instance.Save(Values.SAVENUMBER);
        }
    }
}
