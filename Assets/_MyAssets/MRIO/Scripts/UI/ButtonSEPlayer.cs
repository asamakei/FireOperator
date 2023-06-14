using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
[RequireComponent(typeof(StageButtonBase))]
public class ButtonSEPlayer : MonoBehaviour
{
    [SerializeField] string seIdentifier;
    StageButtonBase buf;
    private void Awake()
    {
        if (TryGetComponent<StageButtonBase>(out StageButtonBase stageButtonBase))
        {
            stageButtonBase.onPushed += OnPushed;
            buf = stageButtonBase;
        }
    }
    private void OnDestroy()
    {
        buf.onPushed -= OnPushed;
    }
    void OnPushed()
    {
        AudioData data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(seIdentifier);
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
    }
}
