using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "MyGame/AudioDBSO", fileName = "AudioDB")]
public class AudioDataDBSO : ScriptableObject
{
    public AudioData[] audioDatas;
    public AudioData GetAudioData(string identifier)
    {
        foreach(AudioData audioData in audioDatas)
        {
            if(audioData.identifier == identifier)
            {
                return audioData;
            }
        }
        return null;
    }
}
