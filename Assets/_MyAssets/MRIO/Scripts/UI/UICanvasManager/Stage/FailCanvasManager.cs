using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KanKikuchi.AudioManager;

[System.Serializable]
public class FailMessage
{
    public FailState failState;
    public string failMessage;
}
public class FailCanvasManager : CanvasManagerBase
{
    [SerializeField] LevelSelectManager levelSelectManager;
    [SerializeField] float activateDuration = 0.5f;
    [SerializeField] TextMeshProUGUI textMeshProUGUI;
    [SerializeField] FailMessage[] failMessages;
    [SerializeField] string defaultMessage = "Failed";
    [SerializeField] string failedSEIdentifier = "Fail";
    public override void OnStart()
    {
        base.SetStateAction(UIState.Fail);
    }

    public override void OnUpdate()
    {
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
        if (levelSelectManager != null && levelSelectManager.TryGetComponent<IActivater>(out IActivater activater)) activater.DeActivate(activateDuration);
    }

    protected override void OnOpen()
    {
        AudioData data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(failedSEIdentifier);
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
        if (textMeshProUGUI != null)
        {
            textMeshProUGUI.SetText(defaultMessage);
            foreach(FailMessage failMessage in failMessages)
            {
                if (failMessage.failState == Variables.failState) textMeshProUGUI.SetText(failMessage.failMessage);
            }
        }
        gameObject.SetActive(true);
        if (levelSelectManager != null && levelSelectManager.TryGetComponent<IActivater>(out IActivater activater)) activater.Activate(activateDuration); 
    }
}
