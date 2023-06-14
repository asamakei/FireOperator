using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvasManager : CanvasManagerBase
{
    public override void OnStart()
    {
        base.SetStateAction(UIState.Title);
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
