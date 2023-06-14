using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
using System;
public class TouchableFriend : TouchableRope
{
    [SerializeField] string SEIdentifier;
    public Action onDied;
    protected override void ThermalEvent(float diff)
    {
        base.ThermalEvent(diff);
        if (_thermalEnergy >= MaxEnergy)
        {
            AudioData audioData = AudioDataManager.Instance.GetAudioData(SEIdentifier);
            if (audioData == null) return;
            SEManager.Instance.Play(audioData.audioClip, audioData.volume);
            if (onDied != null) onDied();
            this.enabled = false;
        }
    }
}
