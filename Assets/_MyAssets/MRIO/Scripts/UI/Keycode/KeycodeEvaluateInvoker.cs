using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(KeycodeEvaluater))]
public class KeycodeEvaluateInvoker : MonoBehaviour
{
    [SerializeField] StageButtonBase stageButtonBase;
    KeycodeEvaluater keycodeEvaluater;

    private void OnEnable()
    {
        keycodeEvaluater = GetComponent<KeycodeEvaluater>();
        stageButtonBase.onPushed += keycodeEvaluater.ExecuteKeycode;
    }
    private void OnDisable()
    {
        stageButtonBase.onPushed -= keycodeEvaluater.ExecuteKeycode;
    }
}
