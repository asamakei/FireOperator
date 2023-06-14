using KanKikuchi.AudioManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Baloon : Touchable
{
    [SerializeField] GameObject highLightObject;
    [SerializeField] string popSEIdentifier;
    const string BREAK = "Break";
    Animator animator;
    Transform _transform;
    AudioData data;
    private void Awake()
    {
        _transform = transform;
        animator = GetComponent<Animator>();
        data = AudioDBManager.Instance.audioDataDBSO.GetAudioData(popSEIdentifier);
        

    }
    private void OnEnable()
    {
        highLightObject.SetActive(true);
    }

    protected override void ThermalEvent(float diff)
    {
        if (_thermalEnergy >= MaxEnergy)
        {
            Pop();
        }

    }
    void Pop()
    {
        animator.SetTrigger(BREAK);
        highLightObject.SetActive(false);
        
    }

    public void SetInActive()
    {
        if (data != null) SEManager.Instance.Play(data.audioClip, data.volume);
        gameObject.SetActive(false);
    }
}
