using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageClearEventBase : MonoBehaviour
{
    [SerializeField] protected StageClearEventType stageClearEventType = StageClearEventType.General;

    public StageClearEventType StageClearEventType
    {
        get { return stageClearEventType; }
    }

    public abstract void OnClearOpen();
    public abstract void OnClearClose();
}
