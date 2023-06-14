using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
public abstract class SEPlayer : MonoBehaviour
{
    [SerializeField] protected string seIdentifier;
    protected virtual void OnAction()
    {
        if (AudioDBManager.Instance == null) return;
        AudioData data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(seIdentifier);
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
    }
}
