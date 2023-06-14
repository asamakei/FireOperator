using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
using DG.Tweening;
using System;
public class ClearCanvasManager : CanvasManagerBase
{
    public Action onClearOpen;
    public Action onClearClose;
    
    public override void OnStart()
    {
        base.SetStateAction(UIState.Clear);
    }

    public override void OnUpdate()
    {
    }
    protected override void OnOpen()
    {
        if (onClearOpen != null) onClearOpen();
    }

    protected override void OnClose()
    {
        if (onClearClose != null) onClearClose();
    }
}
