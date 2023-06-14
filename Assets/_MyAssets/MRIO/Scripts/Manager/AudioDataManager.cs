using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class AudioDataManager : SingletonMonoBehaviour<AudioDataManager>
{
    [SerializeField] AudioDataDBSO audioDataDBSO;

    public AudioData GetAudioData(string identifier)
    {
        return audioDataDBSO.GetAudioData(identifier);
    }
}
