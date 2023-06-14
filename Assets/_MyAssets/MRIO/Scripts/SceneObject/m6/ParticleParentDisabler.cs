using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleParentDisabler : MonoBehaviour
{
    private void Awake()
    {
        var main = GetComponent<ParticleSystem>().main;

        // StopAction��Callback�ɐݒ肵�Ă���K�v������
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
