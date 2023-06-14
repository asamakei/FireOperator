using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCanvasManager : CanvasManagerBase
{
    public override void OnStart()
    {
        base.SetStateAction(UIState.Select);
    }

    public override void OnUpdate()
    {
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
    }
}
