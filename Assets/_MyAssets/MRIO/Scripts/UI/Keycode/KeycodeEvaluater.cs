using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
[RequireComponent(typeof(TMP_InputField))]
public class KeycodeEvaluater : MonoBehaviour
{
    [SerializeField] string failedMessage;
    KeycodeExecuterBase[] keycodeExecuterBases;
    TMP_InputField inputField;
    private void Awake()
    {
        keycodeExecuterBases = GetComponentsInChildren<KeycodeExecuterBase>();
        inputField = GetComponent<TMP_InputField>();
    }
    private void OnEnable()
    {
        inputField.text = "";
    }
    public void ExecuteKeycode()
    {
        InputKeycode(inputField.text);
    }
    void InputKeycode(string keycode)
    {
        foreach(KeycodeExecuterBase keycodeExecuterBase in keycodeExecuterBases)
        {
            if (keycodeExecuterBase.Keycode == keycode)
            {
                keycodeExecuterBase.Execute();
                return;
            }
        }
        EditableTextWindow.i.EditText(failedMessage);
        EditableTextWindow.i.Activate();
    }
}
