using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KeycodeExecuterBase : MonoBehaviour
{
    [SerializeField] string keycode;
    public string Keycode
    {
        get { return keycode; }
    }

    public abstract void Execute();
}
